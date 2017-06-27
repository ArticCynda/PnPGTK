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
  public partial class EditReelSettings : Form
  {
    private PnPReel _xReel;
    

    public EditReelSettings()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the Reel to be edited.
    /// </summary>
    public PnPReel Reel
    {
      get
      {
        _xReel.Manufacturer = this.xReelSettingsPicker.Manufacturer;
        _xReel.Supplier = this.xReelSettingsPicker.Supplier;
        _xReel.OrderCode = this.xReelSettingsPicker.OrderCode;
        
        _xReel.SupplyPackage = this.xReelSettingsPicker.Package;
        _xReel.PartSize = this.xReelSettingsPicker.PartSize;
        _xReel.Offset = this.xReelSettingsPicker.Offset;
        _xReel.FeedSpacing = this.xReelSettingsPicker.FeedSpacing;

        _xReel.Nozzle = this.xReelSettingsPicker.Nozzle;
        _xReel.Speed = this.xReelSettingsPicker.Speed;
        _xReel.Footprint.Height = this.xReelSettingsPicker.PartHeight;
        _xReel.Rotation = this.xReelSettingsPicker.Rotation;

        return _xReel; 
      }
      set
      {
        _xReel = value;
        
        this.xReelSettingsPicker.Manufacturer = _xReel.Manufacturer;
        this.xReelSettingsPicker.Supplier = _xReel.Supplier;
        this.xReelSettingsPicker.OrderCode = _xReel.OrderCode;
        
        this.xReelSettingsPicker.Package = _xReel.SupplyPackage;
        this.xReelSettingsPicker.PartSize = _xReel.PartSize;
        this.xReelSettingsPicker.Offset = _xReel.Offset;
        this.xReelSettingsPicker.FeedSpacing = _xReel.FeedSpacing;

        this.xReelSettingsPicker.Nozzle = _xReel.Nozzle;
        this.xReelSettingsPicker.Speed = _xReel.Speed;
        this.xReelSettingsPicker.PartHeight = _xReel.Footprint.Height;
        this.xReelSettingsPicker.Rotation = _xReel.Rotation;
      }
    }

    private void EditReelSettings_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.xReelSettingsPicker.Package == PnPSupplyPackage.Tray || this.xReelSettingsPicker.Package == PnPSupplyPackage.Tube)
      {
        if (this.xReelSettingsPicker.PartSize.X <= 0 || this.xReelSettingsPicker.PartSize.Y <= 0)
        {
          MessageBox.Show("Part size cannot be zero.");
          e.Cancel = true;
        }
      }

      if (this.xReelSettingsPicker.PartHeight <= 0)
      {
        MessageBox.Show("Part height cannot be zero.");
        e.Cancel = true;
      }
    }

  }
}
