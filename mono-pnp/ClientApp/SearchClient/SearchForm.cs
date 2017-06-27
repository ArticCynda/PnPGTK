using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ClientApp
{
    public partial class SearchForm : Form
    {
        public SearchForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClientApp.SearchWS.SearchServiceSoapClient
                client = new ClientApp.SearchWS.SearchServiceSoapClient();

            ClientApp.SearchWS.SecuritySoapHeader
                securityHeader = new ClientApp.SearchWS.SecuritySoapHeader();
            ClientApp.SearchWS.UsernameToken
                usernameToken = new ClientApp.SearchWS.UsernameToken();
            usernameToken.Username = this.txtUsername.Text;
            usernameToken.Password = this.txtPassword.Text;

            ClientApp.SearchWS.PartnerInformationSoapHeader
                partnerInformation = new ClientApp.SearchWS.PartnerInformationSoapHeader();
            partnerInformation.PartnerID = new Guid(this.txtPartnerId.Text);
            securityHeader.UsernameToken = usernameToken;

            try
            {
                ClientApp.SearchWS.EDAProductInfo
                    productInfo = client.GetProductInfoByDigikeyPartNumber(securityHeader, partnerInformation, this.txtDigikeyPartNumber.Text);

                if (productInfo.BaseProductInfo.DigiKeyPartNumber != null)
                {
                    this.txtManufacturerName.Text = productInfo.BaseProductInfo.ManufacturerName;
                    this.txtManufacturerPartNumber.Text = productInfo.BaseProductInfo.ManufacturerPartNumber;
                    this.txtQuantityAvailable.Text = productInfo.BaseProductInfo.QuantityOnHand.ToString();
                    this.txtUnitPrice.Text = productInfo.Pricing.PriceBreakList[0].UnitPrice.ToString();
                }
                else
                {
                    MessageBox.Show("Part not found.");
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
