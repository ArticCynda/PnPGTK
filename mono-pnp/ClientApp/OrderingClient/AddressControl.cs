using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OrderingClient
{
    public partial class AddressControl : UserControl
    {
        private object dataSource;

        public AddressControl()
        {
            InitializeComponent();
        }

        [RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue("")]
        [AttributeProvider(typeof(OrderingClient.SalesWS.Address))]
        public object DataSource
        {
            get
            {
                return dataSource;
            }
            set
            {
                dataSource = value;
                
                txtFirstName.DataBindings.Clear();
                txtLastName.DataBindings.Clear();
                txtCompany.DataBindings.Clear();
                txtMailStop.DataBindings.Clear();
                txtAdrsLine1.DataBindings.Clear();
                txtAdrsLine2.DataBindings.Clear();
                txtCity.DataBindings.Clear();
                txtState.DataBindings.Clear();
                txtZip.DataBindings.Clear();
                txtCountry.DataBindings.Clear();

                if (dataSource != null)
                {
                    txtFirstName.DataBindings.Add("Text", dataSource, "FirstName");
                    txtLastName.DataBindings.Add("Text", dataSource, "LastName");
                    txtCompany.DataBindings.Add("Text", dataSource, "CompanyName");
                    txtMailStop.DataBindings.Add("Text", dataSource, "AddressLine1");
                    txtAdrsLine1.DataBindings.Add("Text", dataSource, "AddressLine2");
                    txtAdrsLine2.DataBindings.Add("Text", dataSource, "AddressLine3");
                    txtCity.DataBindings.Add("Text", dataSource, "CityName");
                    txtState.DataBindings.Add("Text", dataSource, "StateOrProvinceCode");
                    txtZip.DataBindings.Add("Text", dataSource, "PostalCode");
                    txtCountry.DataBindings.Add("Text", dataSource, "ISOCountryCode");
                }
            }
        }
    }
}
