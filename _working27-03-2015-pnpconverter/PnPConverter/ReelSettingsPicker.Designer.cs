namespace PnPConverter
{
  partial class ReelSettingsPicker
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.tabNewReel = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.pnlPartSize = new System.Windows.Forms.Panel();
      this.numSizeY = new System.Windows.Forms.NumericUpDown();
      this.numSizeX = new System.Windows.Forms.NumericUpDown();
      this.lblCurXSize = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.lblCurYSize = new System.Windows.Forms.Label();
      this.txtManufacturer = new System.Windows.Forms.TextBox();
      this.lblCurManufacturer = new System.Windows.Forms.Label();
      this.lblSupply = new System.Windows.Forms.Label();
      this.comPackageType = new System.Windows.Forms.ComboBox();
      this.txtOrderCode = new System.Windows.Forms.TextBox();
      this.lblCurSupplier = new System.Windows.Forms.Label();
      this.lblCurOrderCode = new System.Windows.Forms.Label();
      this.txtSupplier = new System.Windows.Forms.TextBox();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.numHeight = new System.Windows.Forms.NumericUpDown();
      this.numRotation = new System.Windows.Forms.NumericUpDown();
      this.lblRotation = new System.Windows.Forms.Label();
      this.comNozzle = new System.Windows.Forms.ComboBox();
      this.comSpeed = new System.Windows.Forms.ComboBox();
      this.lblMm = new System.Windows.Forms.Label();
      this.lblHeight = new System.Windows.Forms.Label();
      this.lblCurSpeed = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.lblCurNozzle = new System.Windows.Forms.Label();
      this.pnlOffsets = new System.Windows.Forms.Panel();
      this.numOffsetY = new System.Windows.Forms.NumericUpDown();
      this.numOffsetX = new System.Windows.Forms.NumericUpDown();
      this.numFeedSpacing = new System.Windows.Forms.NumericUpDown();
      this.label9 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.lblCurFeedSpacing = new System.Windows.Forms.Label();
      this.lblCurOffset = new System.Windows.Forms.Label();
      this.tabNewReel.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.pnlPartSize.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numSizeY)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numSizeX)).BeginInit();
      this.tabPage2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numRotation)).BeginInit();
      this.pnlOffsets.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numOffsetY)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numOffsetX)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numFeedSpacing)).BeginInit();
      this.SuspendLayout();
      // 
      // tabNewReel
      // 
      this.tabNewReel.Controls.Add(this.tabPage1);
      this.tabNewReel.Controls.Add(this.tabPage2);
      this.tabNewReel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabNewReel.Location = new System.Drawing.Point(0, 0);
      this.tabNewReel.Name = "tabNewReel";
      this.tabNewReel.SelectedIndex = 0;
      this.tabNewReel.Size = new System.Drawing.Size(390, 206);
      this.tabNewReel.TabIndex = 7;
      // 
      // tabPage1
      // 
      this.tabPage1.AutoScroll = true;
      this.tabPage1.Controls.Add(this.pnlOffsets);
      this.tabPage1.Controls.Add(this.pnlPartSize);
      this.tabPage1.Controls.Add(this.txtManufacturer);
      this.tabPage1.Controls.Add(this.lblCurManufacturer);
      this.tabPage1.Controls.Add(this.lblSupply);
      this.tabPage1.Controls.Add(this.comPackageType);
      this.tabPage1.Controls.Add(this.txtOrderCode);
      this.tabPage1.Controls.Add(this.lblCurSupplier);
      this.tabPage1.Controls.Add(this.lblCurOrderCode);
      this.tabPage1.Controls.Add(this.txtSupplier);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(382, 180);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Supply Chain";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // pnlPartSize
      // 
      this.pnlPartSize.Controls.Add(this.numSizeY);
      this.pnlPartSize.Controls.Add(this.numSizeX);
      this.pnlPartSize.Controls.Add(this.lblCurXSize);
      this.pnlPartSize.Controls.Add(this.label1);
      this.pnlPartSize.Controls.Add(this.lblCurYSize);
      this.pnlPartSize.Location = new System.Drawing.Point(7, 89);
      this.pnlPartSize.Name = "pnlPartSize";
      this.pnlPartSize.Size = new System.Drawing.Size(369, 29);
      this.pnlPartSize.TabIndex = 29;
      this.pnlPartSize.Visible = false;
      // 
      // numSizeY
      // 
      this.numSizeY.DecimalPlaces = 2;
      this.numSizeY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
      this.numSizeY.Location = new System.Drawing.Point(209, 3);
      this.numSizeY.Name = "numSizeY";
      this.numSizeY.Size = new System.Drawing.Size(71, 20);
      this.numSizeY.TabIndex = 46;
      this.numSizeY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // numSizeX
      // 
      this.numSizeX.DecimalPlaces = 2;
      this.numSizeX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
      this.numSizeX.Location = new System.Drawing.Point(83, 3);
      this.numSizeX.Name = "numSizeX";
      this.numSizeX.Size = new System.Drawing.Size(71, 20);
      this.numSizeX.TabIndex = 45;
      this.numSizeX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // lblCurXSize
      // 
      this.lblCurXSize.AutoSize = true;
      this.lblCurXSize.Location = new System.Drawing.Point(11, 5);
      this.lblCurXSize.Name = "lblCurXSize";
      this.lblCurXSize.Size = new System.Drawing.Size(67, 13);
      this.lblCurXSize.TabIndex = 24;
      this.lblCurXSize.Text = "Part size: x =";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(286, 5);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(23, 13);
      this.label1.TabIndex = 28;
      this.label1.Text = "mm";
      // 
      // lblCurYSize
      // 
      this.lblCurYSize.AutoSize = true;
      this.lblCurYSize.Location = new System.Drawing.Point(160, 5);
      this.lblCurYSize.Name = "lblCurYSize";
      this.lblCurYSize.Size = new System.Drawing.Size(43, 13);
      this.lblCurYSize.TabIndex = 25;
      this.lblCurYSize.Text = "mm, y =";
      // 
      // txtManufacturer
      // 
      this.txtManufacturer.Location = new System.Drawing.Point(90, 13);
      this.txtManufacturer.Name = "txtManufacturer";
      this.txtManufacturer.Size = new System.Drawing.Size(282, 20);
      this.txtManufacturer.TabIndex = 15;
      // 
      // lblCurManufacturer
      // 
      this.lblCurManufacturer.AutoSize = true;
      this.lblCurManufacturer.Location = new System.Drawing.Point(11, 16);
      this.lblCurManufacturer.Name = "lblCurManufacturer";
      this.lblCurManufacturer.Size = new System.Drawing.Size(73, 13);
      this.lblCurManufacturer.TabIndex = 2;
      this.lblCurManufacturer.Text = "Manufacturer:";
      // 
      // lblSupply
      // 
      this.lblSupply.AutoSize = true;
      this.lblSupply.Location = new System.Drawing.Point(8, 68);
      this.lblSupply.Name = "lblSupply";
      this.lblSupply.Size = new System.Drawing.Size(76, 13);
      this.lblSupply.TabIndex = 3;
      this.lblSupply.Text = "Package type:";
      // 
      // comPackageType
      // 
      this.comPackageType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comPackageType.FormattingEnabled = true;
      this.comPackageType.Items.AddRange(new object[] {
            "Reel, 8 mm",
            "Reel, 12 mm",
            "Reel, 16 mm",
            "Tube",
            "Tray"});
      this.comPackageType.Location = new System.Drawing.Point(90, 65);
      this.comPackageType.Name = "comPackageType";
      this.comPackageType.Size = new System.Drawing.Size(283, 21);
      this.comPackageType.TabIndex = 9;
      this.comPackageType.SelectedIndexChanged += new System.EventHandler(this.PackageTypeChanged);
      // 
      // txtOrderCode
      // 
      this.txtOrderCode.Location = new System.Drawing.Point(262, 39);
      this.txtOrderCode.Name = "txtOrderCode";
      this.txtOrderCode.Size = new System.Drawing.Size(110, 20);
      this.txtOrderCode.TabIndex = 19;
      // 
      // lblCurSupplier
      // 
      this.lblCurSupplier.AutoSize = true;
      this.lblCurSupplier.Location = new System.Drawing.Point(35, 42);
      this.lblCurSupplier.Name = "lblCurSupplier";
      this.lblCurSupplier.Size = new System.Drawing.Size(48, 13);
      this.lblCurSupplier.TabIndex = 16;
      this.lblCurSupplier.Text = "Supplier:";
      // 
      // lblCurOrderCode
      // 
      this.lblCurOrderCode.AutoSize = true;
      this.lblCurOrderCode.Location = new System.Drawing.Point(193, 42);
      this.lblCurOrderCode.Name = "lblCurOrderCode";
      this.lblCurOrderCode.Size = new System.Drawing.Size(63, 13);
      this.lblCurOrderCode.TabIndex = 18;
      this.lblCurOrderCode.Text = "Order code:";
      // 
      // txtSupplier
      // 
      this.txtSupplier.Location = new System.Drawing.Point(90, 39);
      this.txtSupplier.Name = "txtSupplier";
      this.txtSupplier.Size = new System.Drawing.Size(97, 20);
      this.txtSupplier.TabIndex = 17;
      // 
      // tabPage2
      // 
      this.tabPage2.AutoScroll = true;
      this.tabPage2.Controls.Add(this.numHeight);
      this.tabPage2.Controls.Add(this.numRotation);
      this.tabPage2.Controls.Add(this.lblRotation);
      this.tabPage2.Controls.Add(this.comNozzle);
      this.tabPage2.Controls.Add(this.comSpeed);
      this.tabPage2.Controls.Add(this.lblMm);
      this.tabPage2.Controls.Add(this.lblHeight);
      this.tabPage2.Controls.Add(this.lblCurSpeed);
      this.tabPage2.Controls.Add(this.label4);
      this.tabPage2.Controls.Add(this.lblCurNozzle);
      this.tabPage2.Location = new System.Drawing.Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(382, 180);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Placement";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // numHeight
      // 
      this.numHeight.DecimalPlaces = 2;
      this.numHeight.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
      this.numHeight.Location = new System.Drawing.Point(69, 76);
      this.numHeight.Name = "numHeight";
      this.numHeight.Size = new System.Drawing.Size(79, 20);
      this.numHeight.TabIndex = 38;
      this.numHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // numRotation
      // 
      this.numRotation.Location = new System.Drawing.Point(69, 106);
      this.numRotation.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
      this.numRotation.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
      this.numRotation.Name = "numRotation";
      this.numRotation.Size = new System.Drawing.Size(79, 20);
      this.numRotation.TabIndex = 37;
      this.numRotation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // lblRotation
      // 
      this.lblRotation.AutoSize = true;
      this.lblRotation.Location = new System.Drawing.Point(12, 108);
      this.lblRotation.Name = "lblRotation";
      this.lblRotation.Size = new System.Drawing.Size(50, 13);
      this.lblRotation.TabIndex = 36;
      this.lblRotation.Text = "Rotation:";
      // 
      // comNozzle
      // 
      this.comNozzle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comNozzle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comNozzle.FormattingEnabled = true;
      this.comNozzle.Items.AddRange(new object[] {
            "Large",
            "Medium",
            "Small",
            "Tiny"});
      this.comNozzle.Location = new System.Drawing.Point(69, 14);
      this.comNozzle.Name = "comNozzle";
      this.comNozzle.Size = new System.Drawing.Size(296, 21);
      this.comNozzle.TabIndex = 35;
      // 
      // comSpeed
      // 
      this.comSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comSpeed.FormattingEnabled = true;
      this.comSpeed.Items.AddRange(new object[] {
            "Very Fast",
            "Fast",
            "Medium",
            "Slow",
            "Very Slow"});
      this.comSpeed.Location = new System.Drawing.Point(69, 44);
      this.comSpeed.Name = "comSpeed";
      this.comSpeed.Size = new System.Drawing.Size(296, 21);
      this.comSpeed.TabIndex = 34;
      // 
      // lblMm
      // 
      this.lblMm.AutoSize = true;
      this.lblMm.Location = new System.Drawing.Point(154, 83);
      this.lblMm.Name = "lblMm";
      this.lblMm.Size = new System.Drawing.Size(23, 13);
      this.lblMm.TabIndex = 27;
      this.lblMm.Text = "mm";
      // 
      // lblHeight
      // 
      this.lblHeight.AutoSize = true;
      this.lblHeight.Location = new System.Drawing.Point(21, 78);
      this.lblHeight.Name = "lblHeight";
      this.lblHeight.Size = new System.Drawing.Size(41, 13);
      this.lblHeight.TabIndex = 25;
      this.lblHeight.Text = "Height:";
      // 
      // lblCurSpeed
      // 
      this.lblCurSpeed.AutoSize = true;
      this.lblCurSpeed.Location = new System.Drawing.Point(22, 47);
      this.lblCurSpeed.Name = "lblCurSpeed";
      this.lblCurSpeed.Size = new System.Drawing.Size(41, 13);
      this.lblCurSpeed.TabIndex = 23;
      this.lblCurSpeed.Text = "Speed:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(154, 108);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(45, 13);
      this.label4.TabIndex = 24;
      this.label4.Text = "degrees";
      // 
      // lblCurNozzle
      // 
      this.lblCurNozzle.AutoSize = true;
      this.lblCurNozzle.Location = new System.Drawing.Point(21, 17);
      this.lblCurNozzle.Name = "lblCurNozzle";
      this.lblCurNozzle.Size = new System.Drawing.Size(42, 13);
      this.lblCurNozzle.TabIndex = 20;
      this.lblCurNozzle.Text = "Nozzle:";
      // 
      // pnlOffsets
      // 
      this.pnlOffsets.Controls.Add(this.numOffsetY);
      this.pnlOffsets.Controls.Add(this.numOffsetX);
      this.pnlOffsets.Controls.Add(this.numFeedSpacing);
      this.pnlOffsets.Controls.Add(this.label9);
      this.pnlOffsets.Controls.Add(this.label8);
      this.pnlOffsets.Controls.Add(this.label7);
      this.pnlOffsets.Controls.Add(this.lblCurFeedSpacing);
      this.pnlOffsets.Controls.Add(this.lblCurOffset);
      this.pnlOffsets.Location = new System.Drawing.Point(6, 116);
      this.pnlOffsets.Name = "pnlOffsets";
      this.pnlOffsets.Size = new System.Drawing.Size(369, 61);
      this.pnlOffsets.TabIndex = 30;
      // 
      // numOffsetY
      // 
      this.numOffsetY.DecimalPlaces = 2;
      this.numOffsetY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
      this.numOffsetY.Location = new System.Drawing.Point(204, 3);
      this.numOffsetY.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
      this.numOffsetY.Name = "numOffsetY";
      this.numOffsetY.Size = new System.Drawing.Size(71, 20);
      this.numOffsetY.TabIndex = 52;
      this.numOffsetY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // numOffsetX
      // 
      this.numOffsetX.DecimalPlaces = 2;
      this.numOffsetX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
      this.numOffsetX.Location = new System.Drawing.Point(83, 3);
      this.numOffsetX.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
      this.numOffsetX.Name = "numOffsetX";
      this.numOffsetX.Size = new System.Drawing.Size(71, 20);
      this.numOffsetX.TabIndex = 51;
      this.numOffsetX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // numFeedSpacing
      // 
      this.numFeedSpacing.Location = new System.Drawing.Point(83, 30);
      this.numFeedSpacing.Name = "numFeedSpacing";
      this.numFeedSpacing.Size = new System.Drawing.Size(71, 20);
      this.numFeedSpacing.TabIndex = 50;
      this.numFeedSpacing.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(160, 32);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(23, 13);
      this.label9.TabIndex = 49;
      this.label9.Text = "mm";
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(281, 6);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(23, 13);
      this.label8.TabIndex = 48;
      this.label8.Text = "mm";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(160, 6);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(46, 13);
      this.label7.TabIndex = 47;
      this.label7.Text = "mm, y = ";
      // 
      // lblCurFeedSpacing
      // 
      this.lblCurFeedSpacing.AutoSize = true;
      this.lblCurFeedSpacing.Location = new System.Drawing.Point(-5, 32);
      this.lblCurFeedSpacing.Name = "lblCurFeedSpacing";
      this.lblCurFeedSpacing.Size = new System.Drawing.Size(74, 13);
      this.lblCurFeedSpacing.TabIndex = 46;
      this.lblCurFeedSpacing.Text = "Feed spacing:";
      // 
      // lblCurOffset
      // 
      this.lblCurOffset.AutoSize = true;
      this.lblCurOffset.Location = new System.Drawing.Point(30, 5);
      this.lblCurOffset.Name = "lblCurOffset";
      this.lblCurOffset.Size = new System.Drawing.Size(55, 13);
      this.lblCurOffset.TabIndex = 45;
      this.lblCurOffset.Text = "Offset: x =";
      // 
      // ReelSettingsPicker
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tabNewReel);
      this.Name = "ReelSettingsPicker";
      this.Size = new System.Drawing.Size(390, 206);
      this.tabNewReel.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage1.PerformLayout();
      this.pnlPartSize.ResumeLayout(false);
      this.pnlPartSize.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numSizeY)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numSizeX)).EndInit();
      this.tabPage2.ResumeLayout(false);
      this.tabPage2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numRotation)).EndInit();
      this.pnlOffsets.ResumeLayout(false);
      this.pnlOffsets.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numOffsetY)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numOffsetX)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numFeedSpacing)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabNewReel;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TextBox txtManufacturer;
    private System.Windows.Forms.Label lblCurManufacturer;
    private System.Windows.Forms.Label lblSupply;
    private System.Windows.Forms.ComboBox comPackageType;
    private System.Windows.Forms.TextBox txtOrderCode;
    private System.Windows.Forms.Label lblCurSupplier;
    private System.Windows.Forms.Label lblCurOrderCode;
    private System.Windows.Forms.TextBox txtSupplier;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.NumericUpDown numRotation;
    private System.Windows.Forms.Label lblRotation;
    private System.Windows.Forms.ComboBox comNozzle;
    private System.Windows.Forms.ComboBox comSpeed;
    private System.Windows.Forms.Label lblMm;
    private System.Windows.Forms.Label lblHeight;
    private System.Windows.Forms.Label lblCurSpeed;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label lblCurNozzle;
    private System.Windows.Forms.Label lblCurYSize;
    private System.Windows.Forms.Label lblCurXSize;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Panel pnlPartSize;
    private System.Windows.Forms.NumericUpDown numSizeY;
    private System.Windows.Forms.NumericUpDown numSizeX;
    private System.Windows.Forms.NumericUpDown numHeight;
    private System.Windows.Forms.Panel pnlOffsets;
    private System.Windows.Forms.NumericUpDown numOffsetY;
    private System.Windows.Forms.NumericUpDown numOffsetX;
    private System.Windows.Forms.NumericUpDown numFeedSpacing;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label lblCurFeedSpacing;
    private System.Windows.Forms.Label lblCurOffset;
  }
}
