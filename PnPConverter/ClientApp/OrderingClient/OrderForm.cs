using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OrderingClient
{
    namespace SalesWS
    {
        public partial class Shipping
        {
            protected bool modified;

            [System.Xml.Serialization.SoapIgnore()]
            public bool Modified { get { return modified; } }

            public Shipping()
            {
                modified = false;
            }

            public void OnPropertyChanged(Object sender, PropertyChangedEventArgs e)
            {
                modified = true;
            }
        }

        public partial class Billing
        {
            protected bool modified;

            [System.Xml.Serialization.SoapIgnore()]
            public bool Modified { get { return modified; } }

            public Billing()
            {
                modified = false;
            }

            public void OnPropertyChanged(Object sender, PropertyChangedEventArgs e)
            {
                modified = true;
            }
        }

        public partial class SalesOrder
        {
            protected bool modified;

            [System.Xml.Serialization.SoapIgnore()]
            public bool Modified { get { return modified; } }

            public SalesOrder()
            {
                modified = false;
            }

            public void OnPropertyChanged(Object sender, PropertyChangedEventArgs e)
            {
                modified = true;
            }
        }
    }

    public partial class OrderForm : Form
    {
        //
        // web service client proxy
        //
        private OrderingClient.SalesWS.SalesServiceSoapClient client;
        //
        // security header
        //
        private OrderingClient.SalesWS.SecuritySoapHeader securityHeader;
        //
        // partner information header
        //
        private OrderingClient.SalesWS.PartnerInformationSoapHeader partnerInformation;

        private SalesWS.ShippingMethods
            shippingMethods;
        private SalesWS.BillingPaymentTypesAvailable
            paymentTypes;
        private string[]
            shippingOrderTypes;
        //
        // current order
        //
        private SalesWS.SalesOrder salesorder;
        public SalesWS.SalesOrder SalesOrder
        {
            get { return salesorder; }
            set {
                salesorder = value;

                if (salesorder != null)
                {
                    //
                    // load shipping methods for combo box
                    //
                    shippingMethods = client.GetShippingMethodsAvailable(securityHeader, partnerInformation, salesorder.WebId, salesorder.AccessId, "InitialShipment");
                    foreach (string s in shippingMethods.ErrorMessages)
                    {
                        MessageBox.Show(s);
                    }

                    //
                    // load payment types for combo box
                    //
                    paymentTypes = client.GetBillingPaymentTypesAvailable(securityHeader, partnerInformation, salesorder.WebId, salesorder.AccessId);
                    foreach (string s in paymentTypes.ErrorMessages)
                    {
                        MessageBox.Show(s);
                    }

                    //
                    // Order Types (Personal or Company)
                    //
                    shippingOrderTypes = client.GetShippingOrderTypes(securityHeader, partnerInformation).OrderTypesList;

                    BindControls();

                    SetButtonState();
                }
            }
        }

        public OrderForm()
        {
            InitializeComponent();

            //
            // web service client proxy
            //
            client = new OrderingClient.SalesWS.SalesServiceSoapClient();

            SalesOrder = null;

            SetButtonState();
        }

        private void SetButtonState()
        {
            btnCreateOrder.Enabled = true;

            btnAddDetails.Enabled = SalesOrder != null;

            btnSaveShipping.Enabled = (SalesOrder != null) && SalesOrder.Shipping.Modified;
            btnSaveBilling.Enabled = (SalesOrder != null) && SalesOrder.Billing.Modified;

            btnSubmit.Enabled = (SalesOrder != null) && !(SalesOrder.Shipping.Modified || SalesOrder.Billing.Modified);

            btnRefresh.Enabled = SalesOrder != null;
        }

        public void OnSalesOrderPropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            SetButtonState();
        }

        private void BindControls()
        {
            txtWebId.DataBindings.Clear();
            txtWebId.DataBindings.Add("Text", SalesOrder, "WebId");
            txtAccessId.DataBindings.Clear();
            txtAccessId.DataBindings.Add("Text", SalesOrder, "AccessId");
            txtDateCreated.DataBindings.Clear();
            txtDateCreated.DataBindings.Add("Text", SalesOrder, "DateCreated");

            //
            // detail tab
            //
            gridDetails.DataSource = SalesOrder.Details.DetailsList;
            
            //
            // shipping tab
            //
            adrsShipping.DataSource = SalesOrder.Shipping.Address;
            txtTelephone.DataBindings.Clear();
            txtTelephone.DataBindings.Add("Text", SalesOrder.Shipping.Address, "Telephone");
            txtEmail.DataBindings.Clear();
            txtEmail.DataBindings.Add("Text", SalesOrder.Shipping.Address, "Email");
            // shipping method
            txtShippingMethod.DataBindings.Clear();
            txtShippingMethod.DataBindings.Add("Text", SalesOrder.Shipping, "ShippingMethod");
            cbShippingMethod.DataSource = this.shippingMethods.ShippingMethodsList;
            cbShippingMethod.ValueMember = "method";
            cbShippingMethod.DisplayMember = "description";
            cbShippingMethod.DataBindings.Clear();
            cbShippingMethod.DataBindings.Add("SelectedValue", SalesOrder.Shipping, "ShippingMethod");
            // order type
            txtOrderType.DataBindings.Clear();
            txtOrderType.DataBindings.Add("Text", SalesOrder.Shipping, "OrderType");
            cbOrderType.DataSource = this.shippingOrderTypes;
            cbOrderType.DataBindings.Clear();
            cbOrderType.DataBindings.Add("Text", SalesOrder.Shipping, "OrderType");

            SalesOrder.Shipping.PropertyChanged += new PropertyChangedEventHandler(SalesOrder.Shipping.OnPropertyChanged);
            SalesOrder.Shipping.PropertyChanged += new PropertyChangedEventHandler(OnSalesOrderPropertyChanged);
            SalesOrder.Shipping.Address.PropertyChanged += new PropertyChangedEventHandler(SalesOrder.Shipping.OnPropertyChanged);
            SalesOrder.Shipping.Address.PropertyChanged += new PropertyChangedEventHandler(OnSalesOrderPropertyChanged);

            //
            // billing tab
            //
            adrsBilling.DataSource = SalesOrder.Billing.Address;
            // payment type
            txtPaymentType.DataBindings.Clear();
            txtPaymentType.DataBindings.Add("Text", SalesOrder.Billing, "PaymentType");
            cbPaymentType.DataSource = this.paymentTypes.PaymentTypesList;
            cbPaymentType.DataBindings.Clear();
            cbPaymentType.DataBindings.Add("Text", SalesOrder.Billing, "PaymentType");

            SalesOrder.Billing.PropertyChanged += new PropertyChangedEventHandler(SalesOrder.Billing.OnPropertyChanged);
            SalesOrder.Billing.PropertyChanged += new PropertyChangedEventHandler(OnSalesOrderPropertyChanged);
            SalesOrder.Billing.Address.PropertyChanged += new PropertyChangedEventHandler(SalesOrder.Billing.OnPropertyChanged);
            SalesOrder.Billing.Address.PropertyChanged += new PropertyChangedEventHandler(OnSalesOrderPropertyChanged);

            SalesOrder.PropertyChanged += new PropertyChangedEventHandler(SalesOrder.OnPropertyChanged);
        }

        public void SetupSoapHeaders()
        {
            //
            // setup security header
            //
            securityHeader = new OrderingClient.SalesWS.SecuritySoapHeader();
            OrderingClient.SalesWS.UsernameToken
                usernameToken = new OrderingClient.SalesWS.UsernameToken();
            usernameToken.Username = this.txtUsername.Text;
            usernameToken.Password = this.txtPassword.Text;
            securityHeader.UsernameToken = usernameToken;

            //
            // setup partner information header
            //
            partnerInformation = new OrderingClient.SalesWS.PartnerInformationSoapHeader();
            partnerInformation.PartnerID = new Guid(this.txtPartnerId.Text);
        }

        public void CreateOrder()
        {
            SetupSoapHeaders();

            //
            // create a new order
            //
            SalesWS.SalesOrder
                salesorder = client.CreateDigiKeyOrder(securityHeader, partnerInformation);
            foreach (string s in salesorder.ErrorMessages)
            {
                MessageBox.Show(s);
            }

            SalesOrder = salesorder;
        }

        public void AddDetail(string partNumber, int quantity, string customerReference)
        {
            SalesWS.Detail
                detail = client.AddDetail(securityHeader, partnerInformation, SalesOrder.WebId, SalesOrder.AccessId, quantity, partNumber, customerReference);

            if (detail.ErrorMessages.Count() > 0)
            {
                foreach (string s in detail.ErrorMessages)
                {
                    MessageBox.Show(s);
                }
            }
            else
            {
                List<SalesWS.Detail>
                    details = SalesOrder.Details.DetailsList.ToList();
                details.Add(detail);

                SalesOrder.Details.DetailsList = details.ToArray();
            }
        }

        public void SaveShipping()
        {

            //shipping.AllowSaturdayDelivery
            //shipping.BackorderHoldDays
            //shipping.BackorderShippingBillingType
            //shipping.BackorderShipperAccountNumber
            //shipping.BackorderShippingBillingThirdPartyAccount
            //shipping.BackorderShippingMethod
            //shipping.CustomerNumber
            //shipping.GeoCode
            //shipping.OrderType
            //shipping.ShipperAccountNumber
            //shipping.ShippingBillingThirdPartyAccount
            //shipping.ShippingBillingType
            //shipping.ShippingType
            //shipping.ShippingMethod
            //shipping.VATNumber
            //shipping.Website

            SalesWS.Shipping
                tempShipping = client.SetShipping(securityHeader, partnerInformation, SalesOrder.WebId, SalesOrder.AccessId, SalesOrder.Shipping);

            if (tempShipping.ErrorMessages.Count() > 0)
            {
                foreach (string s in tempShipping.ErrorMessages)
                {
                    MessageBox.Show(s);
                }
            }

            SalesOrder.Shipping = tempShipping;
        }

        public void SaveBilling()
        {
            //shipping.AllowSaturdayDelivery
            //shipping.BackorderHoldDays
            //shipping.BackorderShippingBillingType
            //shipping.BackorderShipperAccountNumber
            //shipping.BackorderShippingBillingThirdPartyAccount
            //shipping.BackorderShippingMethod
            //shipping.CustomerNumber
            //shipping.GeoCode
            //shipping.OrderType
            //shipping.ShipperAccountNumber
            //shipping.ShippingBillingThirdPartyAccount
            //shipping.ShippingBillingType
            //shipping.ShippingType
            //shipping.ShippingMethod
            //shipping.VATNumber
            //shipping.Website

            SalesWS.Billing
                tempBilling = client.SetBilling(securityHeader, partnerInformation, SalesOrder.WebId, SalesOrder.AccessId, SalesOrder.Billing);

            if (tempBilling.ErrorMessages.Count() > 0)
            {
                foreach (string s in tempBilling.ErrorMessages)
                {
                    MessageBox.Show(s);
                }
            }

            SalesOrder.Billing = tempBilling;
        }

        private void RefreshSalesOrder()
        {
            SalesWS.SalesOrder
                result = client.GetOrder(securityHeader, partnerInformation, SalesOrder.WebId, SalesOrder.AccessId);

            if (result.ErrorMessages.Count() > 0)
            {
                foreach (string s in result.ErrorMessages)
                {
                    MessageBox.Show(s);
                }
            }
            else
            {
                this.SalesOrder = result;
            }
        }

        private void btnCreateOrder_Click(object sender, EventArgs e)
        {
            CreateOrder();
            RefreshSalesOrder();
        }

        private void btnAddDetails_Click(object sender, EventArgs e)
        {
            AddDetail("LM556CM-ND", 10, "Sample Part #1");
            AddDetail("NOPART-ND", 10, "Sample Part #2");
            AddDetail("HM830-ND", 10, "Sample Part #3");

            RefreshSalesOrder();
        }

        private void btnSaveShipping_Click(object sender, EventArgs e)
        {
            SaveShipping();

            RefreshSalesOrder();
        }

        private void btnSaveBilling_Click(object sender, EventArgs e)
        {
            SaveBilling();

            RefreshSalesOrder();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshSalesOrder();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            SetupSoapHeaders();

            SalesWS.SalesOrder
                result = client.SubmitOrder(securityHeader, partnerInformation, salesorder.WebId, salesorder.AccessId);

            if (result.ErrorMessages.Count() > 0)
            {
                foreach (string s in result.ErrorMessages)
                {
                    MessageBox.Show(s);
                }
            }
            else
            {
                this.SalesOrder = result;
            }
        }
    }
}

