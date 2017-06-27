namespace ClientApp
{
    partial class SearchForm
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
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPartnerId = new System.Windows.Forms.Label();
            this.lblUnitPrice = new System.Windows.Forms.Label();
            this.lblQuantityAvailable = new System.Windows.Forms.Label();
            this.lblManufacturerPartNumber = new System.Windows.Forms.Label();
            this.lblManufacturerName = new System.Windows.Forms.Label();
            this.lblDigikeyPartNumber = new System.Windows.Forms.Label();
            this.txtUnitPrice = new System.Windows.Forms.TextBox();
            this.txtQuantityAvailable = new System.Windows.Forms.TextBox();
            this.txtManufacturerPartNumber = new System.Windows.Forms.TextBox();
            this.txtManufacturerName = new System.Windows.Forms.TextBox();
            this.txtDigikeyPartNumber = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtPartnerId
            // 
            this.txtPartnerId.Location = new System.Drawing.Point(147, 7);
            this.txtPartnerId.Mask = "{AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA}";
            this.txtPartnerId.Name = "txtPartnerId";
            this.txtPartnerId.Size = new System.Drawing.Size(240, 20);
            this.txtPartnerId.TabIndex = 14;
            this.txtPartnerId.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(147, 33);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(240, 20);
            this.txtUsername.TabIndex = 16;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(147, 59);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(240, 20);
            this.txtPassword.TabIndex = 18;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(11, 62);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 30;
            this.lblPassword.Text = "Password";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(11, 36);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 29;
            this.lblUsername.Text = "Username";
            // 
            // lblPartnerId
            // 
            this.lblPartnerId.AutoSize = true;
            this.lblPartnerId.Location = new System.Drawing.Point(11, 10);
            this.lblPartnerId.Name = "lblPartnerId";
            this.lblPartnerId.Size = new System.Drawing.Size(55, 13);
            this.lblPartnerId.TabIndex = 28;
            this.lblPartnerId.Text = "Partner ID";
            // 
            // lblUnitPrice
            // 
            this.lblUnitPrice.AutoSize = true;
            this.lblUnitPrice.Location = new System.Drawing.Point(11, 192);
            this.lblUnitPrice.Name = "lblUnitPrice";
            this.lblUnitPrice.Size = new System.Drawing.Size(53, 13);
            this.lblUnitPrice.TabIndex = 27;
            this.lblUnitPrice.Text = "Unit Price";
            // 
            // lblQuantityAvailable
            // 
            this.lblQuantityAvailable.AutoSize = true;
            this.lblQuantityAvailable.Location = new System.Drawing.Point(11, 166);
            this.lblQuantityAvailable.Name = "lblQuantityAvailable";
            this.lblQuantityAvailable.Size = new System.Drawing.Size(92, 13);
            this.lblQuantityAvailable.TabIndex = 26;
            this.lblQuantityAvailable.Text = "Quantity Available";
            // 
            // lblManufacturerPartNumber
            // 
            this.lblManufacturerPartNumber.AutoSize = true;
            this.lblManufacturerPartNumber.Location = new System.Drawing.Point(11, 140);
            this.lblManufacturerPartNumber.Name = "lblManufacturerPartNumber";
            this.lblManufacturerPartNumber.Size = new System.Drawing.Size(132, 13);
            this.lblManufacturerPartNumber.TabIndex = 25;
            this.lblManufacturerPartNumber.Text = "Manufacturer Part Number";
            // 
            // lblManufacturerName
            // 
            this.lblManufacturerName.AutoSize = true;
            this.lblManufacturerName.Location = new System.Drawing.Point(11, 114);
            this.lblManufacturerName.Name = "lblManufacturerName";
            this.lblManufacturerName.Size = new System.Drawing.Size(101, 13);
            this.lblManufacturerName.TabIndex = 24;
            this.lblManufacturerName.Text = "Manufacturer Name";
            // 
            // lblDigikeyPartNumber
            // 
            this.lblDigikeyPartNumber.AutoSize = true;
            this.lblDigikeyPartNumber.Location = new System.Drawing.Point(11, 88);
            this.lblDigikeyPartNumber.Name = "lblDigikeyPartNumber";
            this.lblDigikeyPartNumber.Size = new System.Drawing.Size(108, 13);
            this.lblDigikeyPartNumber.TabIndex = 23;
            this.lblDigikeyPartNumber.Text = "Digi-Key Part Number";
            // 
            // txtUnitPrice
            // 
            this.txtUnitPrice.Location = new System.Drawing.Point(147, 189);
            this.txtUnitPrice.Name = "txtUnitPrice";
            this.txtUnitPrice.ReadOnly = true;
            this.txtUnitPrice.Size = new System.Drawing.Size(240, 20);
            this.txtUnitPrice.TabIndex = 21;
            this.txtUnitPrice.TabStop = false;
            // 
            // txtQuantityAvailable
            // 
            this.txtQuantityAvailable.Location = new System.Drawing.Point(147, 163);
            this.txtQuantityAvailable.Name = "txtQuantityAvailable";
            this.txtQuantityAvailable.ReadOnly = true;
            this.txtQuantityAvailable.Size = new System.Drawing.Size(240, 20);
            this.txtQuantityAvailable.TabIndex = 19;
            this.txtQuantityAvailable.TabStop = false;
            // 
            // txtManufacturerPartNumber
            // 
            this.txtManufacturerPartNumber.Location = new System.Drawing.Point(147, 137);
            this.txtManufacturerPartNumber.Name = "txtManufacturerPartNumber";
            this.txtManufacturerPartNumber.ReadOnly = true;
            this.txtManufacturerPartNumber.Size = new System.Drawing.Size(240, 20);
            this.txtManufacturerPartNumber.TabIndex = 17;
            this.txtManufacturerPartNumber.TabStop = false;
            // 
            // txtManufacturerName
            // 
            this.txtManufacturerName.Location = new System.Drawing.Point(147, 111);
            this.txtManufacturerName.Name = "txtManufacturerName";
            this.txtManufacturerName.ReadOnly = true;
            this.txtManufacturerName.Size = new System.Drawing.Size(240, 20);
            this.txtManufacturerName.TabIndex = 15;
            this.txtManufacturerName.TabStop = false;
            // 
            // txtDigikeyPartNumber
            // 
            this.txtDigikeyPartNumber.Location = new System.Drawing.Point(147, 85);
            this.txtDigikeyPartNumber.Name = "txtDigikeyPartNumber";
            this.txtDigikeyPartNumber.Size = new System.Drawing.Size(240, 20);
            this.txtDigikeyPartNumber.TabIndex = 20;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(287, 215);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 30);
            this.btnSearch.TabIndex = 22;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.button1_Click);
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 252);
            this.Controls.Add(this.txtPartnerId);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblPartnerId);
            this.Controls.Add(this.lblUnitPrice);
            this.Controls.Add(this.lblQuantityAvailable);
            this.Controls.Add(this.lblManufacturerPartNumber);
            this.Controls.Add(this.lblManufacturerName);
            this.Controls.Add(this.lblDigikeyPartNumber);
            this.Controls.Add(this.txtUnitPrice);
            this.Controls.Add(this.txtQuantityAvailable);
            this.Controls.Add(this.txtManufacturerPartNumber);
            this.Controls.Add(this.txtManufacturerName);
            this.Controls.Add(this.txtDigikeyPartNumber);
            this.Controls.Add(this.btnSearch);
            this.Name = "SearchForm";
            this.Text = "Sample Application";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox txtPartnerId;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPartnerId;
        private System.Windows.Forms.Label lblUnitPrice;
        private System.Windows.Forms.Label lblQuantityAvailable;
        private System.Windows.Forms.Label lblManufacturerPartNumber;
        private System.Windows.Forms.Label lblManufacturerName;
        private System.Windows.Forms.Label lblDigikeyPartNumber;
        private System.Windows.Forms.TextBox txtUnitPrice;
        private System.Windows.Forms.TextBox txtQuantityAvailable;
        private System.Windows.Forms.TextBox txtManufacturerPartNumber;
        private System.Windows.Forms.TextBox txtManufacturerName;
        private System.Windows.Forms.TextBox txtDigikeyPartNumber;
        private System.Windows.Forms.Button btnSearch;

    }
}

