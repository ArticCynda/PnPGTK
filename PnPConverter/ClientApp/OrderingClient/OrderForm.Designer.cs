namespace OrderingClient
{
    partial class OrderForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtPartnerId = new System.Windows.Forms.MaskedTextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPartnerId = new System.Windows.Forms.Label();
            this.lblWebId = new System.Windows.Forms.Label();
            this.txtWebId = new System.Windows.Forms.TextBox();
            this.lblAccessId = new System.Windows.Forms.Label();
            this.txtAccessId = new System.Windows.Forms.TextBox();
            this.lblDateCreated = new System.Windows.Forms.Label();
            this.txtDateCreated = new System.Windows.Forms.TextBox();
            this.btnCreateOrder = new System.Windows.Forms.Button();
            this.tabBilling = new System.Windows.Forms.TabControl();
            this.tabDetails = new System.Windows.Forms.TabPage();
            this.gridDetails = new System.Windows.Forms.DataGridView();
            this.tabShipping = new System.Windows.Forms.TabPage();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTelephone = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtOrderType = new System.Windows.Forms.TextBox();
            this.cbOrderType = new System.Windows.Forms.ComboBox();
            this.adrsShipping = new OrderingClient.AddressControl();
            this.label1 = new System.Windows.Forms.Label();
            this.txtShippingMethod = new System.Windows.Forms.TextBox();
            this.cbShippingMethod = new System.Windows.Forms.ComboBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPaymentType = new System.Windows.Forms.TextBox();
            this.cbPaymentType = new System.Windows.Forms.ComboBox();
            this.adrsBilling = new OrderingClient.AddressControl();
            this.btnAddDetails = new System.Windows.Forms.Button();
            this.btnSaveShipping = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSaveBilling = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.tabBilling.SuspendLayout();
            this.tabDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetails)).BeginInit();
            this.tabShipping.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPartnerId
            // 
            this.txtPartnerId.Location = new System.Drawing.Point(143, 6);
            this.txtPartnerId.Mask = "{AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA}";
            this.txtPartnerId.Name = "txtPartnerId";
            this.txtPartnerId.Size = new System.Drawing.Size(240, 20);
            this.txtPartnerId.TabIndex = 31;
            this.txtPartnerId.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(143, 58);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(240, 20);
            this.txtPassword.TabIndex = 34;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(7, 61);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 40;
            this.lblPassword.Text = "Password";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(7, 35);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 39;
            this.lblUsername.Text = "Username";
            // 
            // lblPartnerId
            // 
            this.lblPartnerId.AutoSize = true;
            this.lblPartnerId.Location = new System.Drawing.Point(7, 9);
            this.lblPartnerId.Name = "lblPartnerId";
            this.lblPartnerId.Size = new System.Drawing.Size(55, 13);
            this.lblPartnerId.TabIndex = 38;
            this.lblPartnerId.Text = "Partner ID";
            // 
            // lblWebId
            // 
            this.lblWebId.AutoSize = true;
            this.lblWebId.Location = new System.Drawing.Point(399, 9);
            this.lblWebId.Name = "lblWebId";
            this.lblWebId.Size = new System.Drawing.Size(42, 13);
            this.lblWebId.TabIndex = 37;
            this.lblWebId.Text = "Web Id";
            // 
            // txtWebId
            // 
            this.txtWebId.Location = new System.Drawing.Point(535, 6);
            this.txtWebId.Name = "txtWebId";
            this.txtWebId.ReadOnly = true;
            this.txtWebId.Size = new System.Drawing.Size(240, 20);
            this.txtWebId.TabIndex = 32;
            this.txtWebId.TabStop = false;
            // 
            // lblAccessId
            // 
            this.lblAccessId.AutoSize = true;
            this.lblAccessId.Location = new System.Drawing.Point(399, 35);
            this.lblAccessId.Name = "lblAccessId";
            this.lblAccessId.Size = new System.Drawing.Size(54, 13);
            this.lblAccessId.TabIndex = 42;
            this.lblAccessId.Text = "Access Id";
            // 
            // txtAccessId
            // 
            this.txtAccessId.Location = new System.Drawing.Point(535, 32);
            this.txtAccessId.Name = "txtAccessId";
            this.txtAccessId.ReadOnly = true;
            this.txtAccessId.Size = new System.Drawing.Size(240, 20);
            this.txtAccessId.TabIndex = 41;
            this.txtAccessId.TabStop = false;
            // 
            // lblDateCreated
            // 
            this.lblDateCreated.AutoSize = true;
            this.lblDateCreated.Location = new System.Drawing.Point(399, 61);
            this.lblDateCreated.Name = "lblDateCreated";
            this.lblDateCreated.Size = new System.Drawing.Size(70, 13);
            this.lblDateCreated.TabIndex = 44;
            this.lblDateCreated.Text = "Date Created";
            // 
            // txtDateCreated
            // 
            this.txtDateCreated.Location = new System.Drawing.Point(535, 58);
            this.txtDateCreated.Name = "txtDateCreated";
            this.txtDateCreated.ReadOnly = true;
            this.txtDateCreated.Size = new System.Drawing.Size(240, 20);
            this.txtDateCreated.TabIndex = 43;
            this.txtDateCreated.TabStop = false;
            // 
            // btnCreateOrder
            // 
            this.btnCreateOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCreateOrder.Location = new System.Drawing.Point(10, 401);
            this.btnCreateOrder.Name = "btnCreateOrder";
            this.btnCreateOrder.Size = new System.Drawing.Size(77, 23);
            this.btnCreateOrder.TabIndex = 45;
            this.btnCreateOrder.Text = "Create Order";
            this.btnCreateOrder.UseVisualStyleBackColor = true;
            this.btnCreateOrder.Click += new System.EventHandler(this.btnCreateOrder_Click);
            // 
            // tabBilling
            // 
            this.tabBilling.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabBilling.Controls.Add(this.tabDetails);
            this.tabBilling.Controls.Add(this.tabShipping);
            this.tabBilling.Controls.Add(this.tabPage1);
            this.tabBilling.Location = new System.Drawing.Point(10, 84);
            this.tabBilling.Name = "tabBilling";
            this.tabBilling.SelectedIndex = 0;
            this.tabBilling.Size = new System.Drawing.Size(857, 311);
            this.tabBilling.TabIndex = 49;
            // 
            // tabDetails
            // 
            this.tabDetails.Controls.Add(this.gridDetails);
            this.tabDetails.Location = new System.Drawing.Point(4, 22);
            this.tabDetails.Name = "tabDetails";
            this.tabDetails.Padding = new System.Windows.Forms.Padding(3);
            this.tabDetails.Size = new System.Drawing.Size(849, 285);
            this.tabDetails.TabIndex = 0;
            this.tabDetails.Text = "Details";
            this.tabDetails.UseVisualStyleBackColor = true;
            // 
            // gridDetails
            // 
            this.gridDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetails.Location = new System.Drawing.Point(6, 6);
            this.gridDetails.Name = "gridDetails";
            this.gridDetails.Size = new System.Drawing.Size(837, 273);
            this.gridDetails.TabIndex = 49;
            // 
            // tabShipping
            // 
            this.tabShipping.Controls.Add(this.txtEmail);
            this.tabShipping.Controls.Add(this.label4);
            this.tabShipping.Controls.Add(this.txtTelephone);
            this.tabShipping.Controls.Add(this.label3);
            this.tabShipping.Controls.Add(this.label2);
            this.tabShipping.Controls.Add(this.txtOrderType);
            this.tabShipping.Controls.Add(this.cbOrderType);
            this.tabShipping.Controls.Add(this.adrsShipping);
            this.tabShipping.Controls.Add(this.label1);
            this.tabShipping.Controls.Add(this.txtShippingMethod);
            this.tabShipping.Controls.Add(this.cbShippingMethod);
            this.tabShipping.Location = new System.Drawing.Point(4, 22);
            this.tabShipping.Name = "tabShipping";
            this.tabShipping.Padding = new System.Windows.Forms.Padding(3);
            this.tabShipping.Size = new System.Drawing.Size(849, 285);
            this.tabShipping.TabIndex = 1;
            this.tabShipping.Text = "Shipping";
            this.tabShipping.UseVisualStyleBackColor = true;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(402, 124);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(283, 20);
            this.txtEmail.TabIndex = 59;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(334, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 60;
            this.label4.Text = "Email";
            // 
            // txtTelephone
            // 
            this.txtTelephone.Location = new System.Drawing.Point(402, 98);
            this.txtTelephone.Name = "txtTelephone";
            this.txtTelephone.Size = new System.Drawing.Size(283, 20);
            this.txtTelephone.TabIndex = 57;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(334, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 58;
            this.label3.Text = "Telephone";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(334, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 56;
            this.label2.Text = "Order Type";
            // 
            // txtOrderType
            // 
            this.txtOrderType.Location = new System.Drawing.Point(332, 22);
            this.txtOrderType.Name = "txtOrderType";
            this.txtOrderType.ReadOnly = true;
            this.txtOrderType.Size = new System.Drawing.Size(50, 20);
            this.txtOrderType.TabIndex = 55;
            // 
            // cbOrderType
            // 
            this.cbOrderType.FormattingEnabled = true;
            this.cbOrderType.Location = new System.Drawing.Point(388, 21);
            this.cbOrderType.Name = "cbOrderType";
            this.cbOrderType.Size = new System.Drawing.Size(297, 21);
            this.cbOrderType.TabIndex = 54;
            // 
            // adrsShipping
            // 
            this.adrsShipping.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.adrsShipping.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.adrsShipping.DataSource = null;
            this.adrsShipping.Location = new System.Drawing.Point(6, 6);
            this.adrsShipping.Name = "adrsShipping";
            this.adrsShipping.Size = new System.Drawing.Size(320, 273);
            this.adrsShipping.TabIndex = 50;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(334, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 53;
            this.label1.Text = "Shipping Method";
            // 
            // txtShippingMethod
            // 
            this.txtShippingMethod.Location = new System.Drawing.Point(332, 64);
            this.txtShippingMethod.Name = "txtShippingMethod";
            this.txtShippingMethod.ReadOnly = true;
            this.txtShippingMethod.Size = new System.Drawing.Size(50, 20);
            this.txtShippingMethod.TabIndex = 52;
            // 
            // cbShippingMethod
            // 
            this.cbShippingMethod.FormattingEnabled = true;
            this.cbShippingMethod.Location = new System.Drawing.Point(388, 63);
            this.cbShippingMethod.Name = "cbShippingMethod";
            this.cbShippingMethod.Size = new System.Drawing.Size(297, 21);
            this.cbShippingMethod.TabIndex = 51;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.txtPaymentType);
            this.tabPage1.Controls.Add(this.cbPaymentType);
            this.tabPage1.Controls.Add(this.adrsBilling);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(849, 285);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Billing";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(334, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 59;
            this.label5.Text = "Payment Type";
            // 
            // txtPaymentType
            // 
            this.txtPaymentType.Location = new System.Drawing.Point(332, 22);
            this.txtPaymentType.Name = "txtPaymentType";
            this.txtPaymentType.ReadOnly = true;
            this.txtPaymentType.Size = new System.Drawing.Size(50, 20);
            this.txtPaymentType.TabIndex = 58;
            // 
            // cbPaymentType
            // 
            this.cbPaymentType.FormattingEnabled = true;
            this.cbPaymentType.Location = new System.Drawing.Point(388, 21);
            this.cbPaymentType.Name = "cbPaymentType";
            this.cbPaymentType.Size = new System.Drawing.Size(297, 21);
            this.cbPaymentType.TabIndex = 57;
            // 
            // adrsBilling
            // 
            this.adrsBilling.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.adrsBilling.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.adrsBilling.DataSource = null;
            this.adrsBilling.Location = new System.Drawing.Point(6, 6);
            this.adrsBilling.Name = "adrsBilling";
            this.adrsBilling.Size = new System.Drawing.Size(320, 273);
            this.adrsBilling.TabIndex = 51;
            // 
            // btnAddDetails
            // 
            this.btnAddDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddDetails.Location = new System.Drawing.Point(93, 401);
            this.btnAddDetails.Name = "btnAddDetails";
            this.btnAddDetails.Size = new System.Drawing.Size(77, 23);
            this.btnAddDetails.TabIndex = 51;
            this.btnAddDetails.Text = "Add Details";
            this.btnAddDetails.UseVisualStyleBackColor = true;
            this.btnAddDetails.Click += new System.EventHandler(this.btnAddDetails_Click);
            // 
            // btnSaveShipping
            // 
            this.btnSaveShipping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveShipping.Location = new System.Drawing.Point(176, 401);
            this.btnSaveShipping.Name = "btnSaveShipping";
            this.btnSaveShipping.Size = new System.Drawing.Size(93, 23);
            this.btnSaveShipping.TabIndex = 52;
            this.btnSaveShipping.Text = "Save Shipping";
            this.btnSaveShipping.UseVisualStyleBackColor = true;
            this.btnSaveShipping.Click += new System.EventHandler(this.btnSaveShipping_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(792, 401);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 51;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSaveBilling
            // 
            this.btnSaveBilling.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveBilling.Location = new System.Drawing.Point(275, 401);
            this.btnSaveBilling.Name = "btnSaveBilling";
            this.btnSaveBilling.Size = new System.Drawing.Size(75, 23);
            this.btnSaveBilling.TabIndex = 53;
            this.btnSaveBilling.Text = "Save Billing";
            this.btnSaveBilling.UseVisualStyleBackColor = true;
            this.btnSaveBilling.Click += new System.EventHandler(this.btnSaveBilling_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSubmit.Location = new System.Drawing.Point(402, 401);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 55;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(143, 32);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(240, 20);
            this.txtUsername.TabIndex = 33;
            // 
            // OrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 436);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.btnSaveBilling);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSaveShipping);
            this.Controls.Add(this.btnAddDetails);
            this.Controls.Add(this.tabBilling);
            this.Controls.Add(this.btnCreateOrder);
            this.Controls.Add(this.lblDateCreated);
            this.Controls.Add(this.txtDateCreated);
            this.Controls.Add(this.lblAccessId);
            this.Controls.Add(this.txtAccessId);
            this.Controls.Add(this.txtPartnerId);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblPartnerId);
            this.Controls.Add(this.lblWebId);
            this.Controls.Add(this.txtWebId);
            this.Name = "OrderForm";
            this.Text = "Sample Application";
            this.tabBilling.ResumeLayout(false);
            this.tabDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetails)).EndInit();
            this.tabShipping.ResumeLayout(false);
            this.tabShipping.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox txtPartnerId;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPartnerId;
        private System.Windows.Forms.Label lblWebId;
        private System.Windows.Forms.TextBox txtWebId;
        private System.Windows.Forms.Label lblAccessId;
        private System.Windows.Forms.TextBox txtAccessId;
        private System.Windows.Forms.Label lblDateCreated;
        private System.Windows.Forms.TextBox txtDateCreated;
        private System.Windows.Forms.Button btnCreateOrder;
        private System.Windows.Forms.TabControl tabBilling;
        private System.Windows.Forms.TabPage tabDetails;
        private System.Windows.Forms.DataGridView gridDetails;
        private System.Windows.Forms.TabPage tabShipping;
        private System.Windows.Forms.Button btnAddDetails;
        private System.Windows.Forms.Button btnSaveShipping;

        private OrderingClient.AddressControl adrsShipping;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TabPage tabPage1;
        private AddressControl adrsBilling;
        private System.Windows.Forms.Button btnSaveBilling;
        private System.Windows.Forms.ComboBox cbShippingMethod;
        private System.Windows.Forms.TextBox txtShippingMethod;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtOrderType;
        private System.Windows.Forms.ComboBox cbOrderType;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTelephone;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPaymentType;
        private System.Windows.Forms.ComboBox cbPaymentType;
        private System.Windows.Forms.TextBox txtUsername;
    }
}