namespace PnPConverter
{
  partial class StackLoader
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
      this.chkLocked = new System.Windows.Forms.CheckBox();
      this.lblNo = new System.Windows.Forms.Label();
      this.lblReelType = new System.Windows.Forms.Label();
      this.comReel = new System.Windows.Forms.ComboBox();
      this.picActive = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.picActive)).BeginInit();
      this.SuspendLayout();
      // 
      // chkLocked
      // 
      this.chkLocked.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this.chkLocked.AutoSize = true;
      this.chkLocked.Location = new System.Drawing.Point(3, 4);
      this.chkLocked.Name = "chkLocked";
      this.chkLocked.Size = new System.Drawing.Size(15, 14);
      this.chkLocked.TabIndex = 0;
      this.chkLocked.UseVisualStyleBackColor = true;
      this.chkLocked.CheckedChanged += new System.EventHandler(this.LockedChanged);
      // 
      // lblNo
      // 
      this.lblNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this.lblNo.AutoSize = true;
      this.lblNo.Location = new System.Drawing.Point(24, 4);
      this.lblNo.Name = "lblNo";
      this.lblNo.Size = new System.Drawing.Size(13, 13);
      this.lblNo.TabIndex = 1;
      this.lblNo.Text = "8";
      // 
      // lblReelType
      // 
      this.lblReelType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this.lblReelType.AutoSize = true;
      this.lblReelType.Location = new System.Drawing.Point(43, 4);
      this.lblReelType.Name = "lblReelType";
      this.lblReelType.Size = new System.Drawing.Size(57, 13);
      this.lblReelType.TabIndex = 2;
      this.lblReelType.Text = "Reel 8 mm";
      // 
      // comReel
      // 
      this.comReel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comReel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comReel.FormattingEnabled = true;
      this.comReel.Location = new System.Drawing.Point(106, 1);
      this.comReel.Name = "comReel";
      this.comReel.Size = new System.Drawing.Size(234, 21);
      this.comReel.TabIndex = 3;
      this.comReel.SelectedIndexChanged += new System.EventHandler(this.SelectedItemChanged);
      // 
      // picActive
      // 
      this.picActive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.picActive.BackColor = System.Drawing.SystemColors.Control;
      this.picActive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.picActive.Image = global::PnPConverter.Properties.Resources.ok;
      this.picActive.Location = new System.Drawing.Point(346, 1);
      this.picActive.Name = "picActive";
      this.picActive.Size = new System.Drawing.Size(21, 21);
      this.picActive.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.picActive.TabIndex = 4;
      this.picActive.TabStop = false;
      this.picActive.Visible = false;
      // 
      // StackLoader
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.picActive);
      this.Controls.Add(this.comReel);
      this.Controls.Add(this.lblReelType);
      this.Controls.Add(this.lblNo);
      this.Controls.Add(this.chkLocked);
      this.Name = "StackLoader";
      this.Size = new System.Drawing.Size(370, 23);
      ((System.ComponentModel.ISupportInitialize)(this.picActive)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.CheckBox chkLocked;
    private System.Windows.Forms.Label lblNo;
    private System.Windows.Forms.Label lblReelType;
    private System.Windows.Forms.ComboBox comReel;
    private System.Windows.Forms.PictureBox picActive;
  }
}
