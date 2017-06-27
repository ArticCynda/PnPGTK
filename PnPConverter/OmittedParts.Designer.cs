namespace PnPConverter
{
  partial class ExcludedPartsDialog
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
      System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "IC5",
            "CAP-1206",
            "Capacitor, SMD, 1206",
            "100n",
            "It\'s a Monday"}, -1);
      this.listOmittedParts = new System.Windows.Forms.ListView();
      this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
      this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
      this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
      this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
      this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
      this.label1 = new System.Windows.Forms.Label();
      this.btnIncludeSelected = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // listOmittedParts
      // 
      this.listOmittedParts.CheckBoxes = true;
      this.listOmittedParts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
      listViewItem4.StateImageIndex = 0;
      this.listOmittedParts.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem4});
      this.listOmittedParts.Location = new System.Drawing.Point(25, 43);
      this.listOmittedParts.Name = "listOmittedParts";
      this.listOmittedParts.Size = new System.Drawing.Size(460, 230);
      this.listOmittedParts.TabIndex = 0;
      this.listOmittedParts.UseCompatibleStateImageBehavior = false;
      this.listOmittedParts.View = System.Windows.Forms.View.Details;
      this.listOmittedParts.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.RestoreOmittedPartsButton);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Designator";
      this.columnHeader1.Width = 86;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Footprint";
      this.columnHeader2.Width = 77;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Description";
      this.columnHeader3.Width = 78;
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "Value";
      this.columnHeader4.Width = 68;
      // 
      // columnHeader5
      // 
      this.columnHeader5.Text = "ReasonForExclusion";
      this.columnHeader5.Width = 93;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(22, 13);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(445, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "The following parts are present in the Pick and Place file, but will not be place" +
          "d automatically.";
      // 
      // btnIncludeSelected
      // 
      this.btnIncludeSelected.Location = new System.Drawing.Point(25, 298);
      this.btnIncludeSelected.Name = "btnIncludeSelected";
      this.btnIncludeSelected.Size = new System.Drawing.Size(114, 23);
      this.btnIncludeSelected.TabIndex = 2;
      this.btnIncludeSelected.Text = "&Include Selected";
      this.btnIncludeSelected.UseVisualStyleBackColor = true;
      this.btnIncludeSelected.Click += new System.EventHandler(this.RestoreOmittedPart);
      // 
      // btnOK
      // 
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Location = new System.Drawing.Point(329, 298);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(410, 298);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // OmittedPartsDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(518, 333);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnIncludeSelected);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.listOmittedParts);
      this.Name = "OmittedPartsDialog";
      this.Text = "OmittedParts";
      this.Shown += new System.EventHandler(this.LoadOmittedParts);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ListView listOmittedParts;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.ColumnHeader columnHeader5;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btnIncludeSelected;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
  }
}