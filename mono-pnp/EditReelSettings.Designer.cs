namespace PnPConverter
{
  partial class EditReelSettings
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
      this.xReelSettingsPicker = new PnPConverter.ReelSettingsPicker();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // xReelSettingsPicker
      // 
      this.xReelSettingsPicker.FeedSpacing = 0F;
      this.xReelSettingsPicker.Location = new System.Drawing.Point(4, 12);
      this.xReelSettingsPicker.Manufacturer = "";
      this.xReelSettingsPicker.Name = "xReelSettingsPicker";
      this.xReelSettingsPicker.Nozzle = PnPConverter.PnPMachineNozzle.Undefined;
      this.xReelSettingsPicker.OrderCode = "";
      this.xReelSettingsPicker.Package = PnPConverter.PnPSupplyPackage.Tube;
      this.xReelSettingsPicker.PartHeight = 0F;
      this.xReelSettingsPicker.Rotation = 0;
      this.xReelSettingsPicker.Size = new System.Drawing.Size(390, 206);
      this.xReelSettingsPicker.Speed = 100;
      this.xReelSettingsPicker.Supplier = "";
      this.xReelSettingsPicker.TabIndex = 0;
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(306, 236);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnOK
      // 
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Location = new System.Drawing.Point(225, 236);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      // 
      // EditReelSettings
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(406, 271);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.xReelSettingsPicker);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.HelpButton = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "EditReelSettings";
      this.Text = "Edit Reel Settings";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditReelSettings_FormClosing);
      this.ResumeLayout(false);

    }

    #endregion

    private ReelSettingsPicker xReelSettingsPicker;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
  }
}