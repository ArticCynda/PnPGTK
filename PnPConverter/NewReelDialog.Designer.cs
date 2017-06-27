namespace PnPConverter
{
  partial class NewReelDialog
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
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.comJellys = new System.Windows.Forms.ComboBox();
      this.comReels = new System.Windows.Forms.ComboBox();
      this.btnReelTemplate = new System.Windows.Forms.Button();
      this.btnJellyTemplate = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.xReelSettingsPicker = new PnPConverter.ReelSettingsPicker();
      this.lblReelDefinition = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // btnOK
      // 
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Location = new System.Drawing.Point(234, 327);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(315, 327);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // comJellys
      // 
      this.comJellys.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comJellys.FormattingEnabled = true;
      this.comJellys.Location = new System.Drawing.Point(217, 55);
      this.comJellys.Name = "comJellys";
      this.comJellys.Size = new System.Drawing.Size(173, 21);
      this.comJellys.TabIndex = 3;
      this.comJellys.SelectedIndexChanged += new System.EventHandler(this.JellyTemplateChanged);
      // 
      // comReels
      // 
      this.comReels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comReels.FormattingEnabled = true;
      this.comReels.Location = new System.Drawing.Point(12, 55);
      this.comReels.Name = "comReels";
      this.comReels.Size = new System.Drawing.Size(173, 21);
      this.comReels.TabIndex = 4;
      this.comReels.SelectedIndexChanged += new System.EventHandler(this.ReelTemplateChanged);
      // 
      // btnReelTemplate
      // 
      this.btnReelTemplate.Location = new System.Drawing.Point(80, 83);
      this.btnReelTemplate.Name = "btnReelTemplate";
      this.btnReelTemplate.Size = new System.Drawing.Size(23, 23);
      this.btnReelTemplate.TabIndex = 5;
      this.btnReelTemplate.Text = "Use";
      this.btnReelTemplate.UseVisualStyleBackColor = true;
      this.btnReelTemplate.Click += new System.EventHandler(this.SetExistingReelTemplate);
      // 
      // btnJellyTemplate
      // 
      this.btnJellyTemplate.Location = new System.Drawing.Point(286, 83);
      this.btnJellyTemplate.Name = "btnJellyTemplate";
      this.btnJellyTemplate.Size = new System.Drawing.Size(23, 23);
      this.btnJellyTemplate.TabIndex = 6;
      this.btnJellyTemplate.Text = "U";
      this.btnJellyTemplate.UseVisualStyleBackColor = true;
      this.btnJellyTemplate.Click += new System.EventHandler(this.SetJellyBeanTemplate);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(34, 36);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(123, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "Existing reel as template:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(231, 36);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(151, 13);
      this.label2.TabIndex = 8;
      this.label2.Text = "Generic jelly bean as template:";
      // 
      // xReelSettingsPicker
      // 
      this.xReelSettingsPicker.FeedSpacing = 0F;
      this.xReelSettingsPicker.Location = new System.Drawing.Point(12, 115);
      this.xReelSettingsPicker.Manufacturer = "";
      this.xReelSettingsPicker.Name = "xReelSettingsPicker";
      this.xReelSettingsPicker.Nozzle = PnPConverter.PnPMachineNozzle.Undefined;
      this.xReelSettingsPicker.OrderCode = "";
      this.xReelSettingsPicker.Package = PnPConverter.PnPSupplyPackage.Tube;
      this.xReelSettingsPicker.PartHeight = 0F;
      this.xReelSettingsPicker.Rotation = 0;
      this.xReelSettingsPicker.Size = new System.Drawing.Size(390, 176);
      this.xReelSettingsPicker.Speed = 100;
      this.xReelSettingsPicker.Supplier = "";
      this.xReelSettingsPicker.TabIndex = 0;
      // 
      // lblReelDefinition
      // 
      this.lblReelDefinition.AutoSize = true;
      this.lblReelDefinition.Location = new System.Drawing.Point(9, 9);
      this.lblReelDefinition.Name = "lblReelDefinition";
      this.lblReelDefinition.Size = new System.Drawing.Size(35, 13);
      this.lblReelDefinition.TabIndex = 9;
      this.lblReelDefinition.Text = "label3";
      // 
      // NewReelDialog
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(415, 362);
      this.Controls.Add(this.lblReelDefinition);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.btnJellyTemplate);
      this.Controls.Add(this.btnReelTemplate);
      this.Controls.Add(this.comReels);
      this.Controls.Add(this.comJellys);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.xReelSettingsPicker);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.HelpButton = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "NewReelDialog";
      this.Text = "Define New Reel";
      this.Load += new System.EventHandler(this.LoadTemplates);
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NewReelDialog_FormClosing);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private ReelSettingsPicker xReelSettingsPicker;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.ComboBox comJellys;
    private System.Windows.Forms.ComboBox comReels;
    private System.Windows.Forms.Button btnReelTemplate;
    private System.Windows.Forms.Button btnJellyTemplate;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label lblReelDefinition;
  }
}