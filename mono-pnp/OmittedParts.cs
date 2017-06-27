using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PnPConverter
{
  /// <summary>
  /// A dialog window presenting the user an overview of excluded Parts and the possibility to re-include them.
  /// </summary>
  public partial class ExcludedPartsDialog : Form
  {
    private List<PnPPart> _xExcludedParts = new List<PnPPart>();
    private List<PnPPart> _xIncludedParts = new List<PnPPart>();

    /// <summary>
    /// A list of currently included Parts.
    /// </summary>
    public List<PnPPart> IncludedParts
    {
      get{ return _xIncludedParts; }
      set{ _xIncludedParts = value; }
    }

    /// <summary>
    /// A list of currently excluded parts.
    /// </summary>
    public List<PnPPart> ExcludedParts
    {
      get{ return _xExcludedParts; }
      set{ _xExcludedParts = value; }
    }

    public ExcludedPartsDialog()
    {
      InitializeComponent();
    }

    

    private void UpdateOmittedParts()
    {
      this.listOmittedParts.Items.Clear();
      foreach (PnPPart xCurPart in _xExcludedParts)
      {
        string[] sProperties = new string[5];
        sProperties[0] = xCurPart.Designator;
        sProperties[1] = xCurPart.Footprint.Name;
        sProperties[2] = xCurPart.Footprint.Description;
        sProperties[3] = xCurPart.PartNameOrValue;
        switch (xCurPart.ExclusionReason)
        {
          case ExclusionReason.NotSmd:
            sProperties[4] = "Not a recognized SMD part";
            break;
          case ExclusionReason.NotPhysical:
            sProperties[4] = "No physical body to be placed";
            break;
          case ExclusionReason.Mechanical:
            sProperties[4] = "Mechanical part";
            break;
          case ExclusionReason.UserDefined:
            sProperties[4] = "Manually excluded";
            break;
        }
        ListViewItem xCurPartEntry = new ListViewItem(sProperties);
        this.listOmittedParts.Items.Add(xCurPartEntry);
      }
    }

    private void LoadOmittedParts(object sender, EventArgs e)
    {
      UpdateOmittedParts();
    }

    private void RestoreOmittedPartsButton(object sender, ItemCheckedEventArgs e)
    {
      bool bCheckedParts = false;
      foreach (ListViewItem xCurPartEntry in this.listOmittedParts.Items)
        if (xCurPartEntry.Checked)
          bCheckedParts = true;

      this.btnIncludeSelected.Enabled = bCheckedParts;
    }

    private void RestoreOmittedPart(object sender, EventArgs e)
    {
      bool bIncluded = false;
      foreach (ListViewItem xCurPartEntry in this.listOmittedParts.Items)
      {
        if (xCurPartEntry.Checked)
        {
          bIncluded = true;
          foreach (PnPPart xCurPart in _xExcludedParts)
          {
            if (xCurPart.Designator.ToUpper() == xCurPartEntry.SubItems[0].Text.ToUpper())
            {
              _xExcludedParts.Remove(xCurPart);
              xCurPart.ExclusionReason = ExclusionReason.None;
              _xIncludedParts.Add(xCurPart);
              break;
            }
          }
        }
      }
      if (bIncluded)
        UpdateOmittedParts();
    }

    private void OmittedPartsDialog_Shown(object sender, EventArgs e)
    {
      UpdateOmittedParts();
    }
  }
}
