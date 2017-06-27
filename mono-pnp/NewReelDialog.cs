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
  public partial class NewReelDialog : Form
  {
    private List<PnPReel> _xReelCollection = new List<PnPReel>();
    private List<PnPPart> _xPartCollection = new List<PnPPart>();
    private PnPReel _xReel = new PnPReel();

    // when adding a jelly bean size here, make sure to add its data also to the GetJellyBeanSettings routine!
    private List<string> JELLY_SIZES = new List<string>() { "0402", "0603", "0805", "1206", "1210" };

    public NewReelDialog()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Gets or sets a list of reels that may be used as template to define a new reel.
    /// </summary>
    public List<PnPReel> ReelCollection
    {
      get{ return _xReelCollection; }
      set{ _xReelCollection = value; }
    }

    /// <summary>
    /// Gets or sets a list of parts that will be used for automatic configuration of new reel settings.
    /// </summary>
    public List<PnPPart> PartCollection
    {
      get{ return _xPartCollection; }
      set{ _xPartCollection = value; }
    }

    /// <summary>
    /// Updates the reel settings picker control with the current reel data.
    /// </summary>
    private void UpdateReelSettings()
    {
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

    /// <summary>
    /// Gets the reel defined by the user.
    /// </summary>
    public PnPReel Reel
    {
      set
      {
        _xReel = value;
        UpdateReelSettings();
      }
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
    }

    /// <summary>
    /// Attempt to configure reel settings automatically based on footprint and part value data in the PartType field.
    /// </summary>
    public PnPReel AutoConfigure(PnPPartType partType)
    {
      PnPReel xReel = new PnPReel();

      // if this footprint already exists in the library, copy its settings
      foreach (PnPReel xCurReel in _xReelCollection)
      {
        if (xCurReel.Footprint.Name.Trim().ToLower() == partType.Footprint.Name.Trim().ToLower())
        {
          // footprint match
          xReel.FeedSpacing = xCurReel.FeedSpacing;
          xReel.Nozzle = xCurReel.Nozzle;
          xReel.SupplyPackage = xCurReel.SupplyPackage;
          xReel.Speed = xCurReel.Speed;
          //_xReel.XOffset = xCurReel.XOffset;
          //_xReel.YOffset = xCurReel.YOffset;
          xReel.Offset = xCurReel.Offset;
          break;
        }
      }

      // try to deduct suitable values for jellybean parts based on their footprint
      List<string> sJellyBeans = new List<string>() { "CAP", "RES", "IND", "FUSE", "LED" };
      bool bIsJellyPart = false;
      foreach (string sCur in sJellyBeans)
        if (partType.Footprint.Name.ToUpper().Trim().StartsWith(sCur))
          bIsJellyPart = true;

      if (bIsJellyPart)
      {
        string sFootprintSize = partType.Footprint.Name.Trim().Substring(partType.Footprint.Name.Trim().Length - 4);
        JellyBeanSettings xSettings = GetJellyBeanSettings(sFootprintSize);
        xReel.Offset = xSettings.Offset;
        xReel.Nozzle = xSettings.Nozzle;
        xReel.Footprint.Height = xSettings.PartHeight;
        xReel.FeedSpacing = xSettings.FeedSpacing;
        xReel.Speed = xSettings.Speed;
        xReel.SupplyPackage = xSettings.Package;

        // DEBUG:
        // jelly parts rotated by 90° in Altium!
        xReel.Rotation = 90;
      }

      // try to deduct values for common IC types
      string sICFootprint = partType.Footprint.Name.Trim().ToUpper();
      if (
        sICFootprint.StartsWith("IC-SOIC-") ||
        sICFootprint.StartsWith("IC-TSSOP-") ||
        sICFootprint.StartsWith("IC-MSOP-") ||
        sICFootprint.StartsWith("IC-POWERSO-") ||
        sICFootprint.StartsWith("IC-SSOP-") ||
        sICFootprint.StartsWith("IC-VQFN-")
      )
      {
        xReel.Nozzle = PnPMachineNozzle.L;
        xReel.Footprint.Height = 2F;
        xReel.FeedSpacing = 18;
        xReel.Speed = 10;
        xReel.SupplyPackage = PnPSupplyPackage.Tray;
      }

      // search the parts list for matching (footprint, value) combinations to load manufacturer properties automatically
      foreach (PnPPart xCurPart in _xPartCollection)
      {
        if (xCurPart.PartNameOrValue == partType.PartNameOrValue && xCurPart.Footprint.Name == partType.Footprint.Name)
        {
          xReel.Manufacturer = xCurPart.Manufacturer;
          xReel.Supplier = xCurPart.Supplier;
          xReel.OrderCode = xCurPart.OrderCode;
          break;
        }
      }

      xReel.Speed = 50; // default

      xReel.GUID = Guid.NewGuid();

      return xReel;
    }

    /// <summary>
    /// Represents the default settings of a jelly bean part.
    /// </summary>
    public struct JellyBeanSettings
    {
      /// <summary>
      /// The size code of this jelly bean part, i.e. "0402", "1206" etc.
      /// </summary>
      public string SizeCode;
      
      /// <summary>
      /// The offset of the part with respect to the center of the feeder perforation.
      /// </summary>
      public FPoint Offset;

      /// <summary>
      /// The preferred nozzle to pick up the part with.
      /// </summary>
      public PnPMachineNozzle Nozzle;

      /// <summary>
      /// The height of the part.
      /// </summary>
      public float PartHeight;

      /// <summary>
      /// The distance between adjacent feeder perforations.
      /// </summary>
      public float FeedSpacing;

      /// <summary>
      /// The preferred speed to move the part with.
      /// </summary>
      public int Speed;

      /// <summary>
      /// The package in which the part is supplied.
      /// </summary>
      public PnPSupplyPackage Package;

      public override string ToString()
      {
        if (SizeCode.Equals(string.Empty))
          return "Generic";
        else
          return SizeCode;
      }
    }

    /// <summary>
    /// Retrieves the default settings for a jelly bean part of given size code.
    /// </summary>
    /// <param name="size">The size code of the part, i.e. "0402", "1206" etc.</param>
    /// <returns>The default settings of a jelly bean part with the specified size code.</returns>
    public JellyBeanSettings GetJellyBeanSettings(string size)
    {
      JellyBeanSettings xPart = new JellyBeanSettings();
      xPart.Nozzle = PnPMachineNozzle.Undefined;
      switch (size)
      {
        case "0402":
          xPart.Offset = new FPoint(0, 0.05F);
          xPart.Nozzle = PnPMachineNozzle.XS;
          xPart.PartHeight = 0.5F;
          xPart.FeedSpacing = 2;
          xPart.Speed = 100;
          xPart.Package = PnPSupplyPackage.Reel8mm;
          break;
        case "0603":
          xPart.Offset = new FPoint(-0.04F, -0.1F);
          xPart.Nozzle = PnPMachineNozzle.S;
          xPart.PartHeight = 0.95F;
          xPart.FeedSpacing = 4;
          xPart.Speed = 100;
          xPart.Package = PnPSupplyPackage.Reel8mm;
          break;
        case "0805":
          xPart.Offset = new FPoint(0.05F, 0);
          xPart.Nozzle = PnPMachineNozzle.S;
          xPart.PartHeight = 1.10F;
          xPart.FeedSpacing = 4;
          xPart.Speed = 90;
          xPart.Package = PnPSupplyPackage.Reel8mm;
          break;
        case "1206":
          xPart.Offset = new FPoint(0.23F, 0);
          xPart.Nozzle = PnPMachineNozzle.M;
          xPart.PartHeight = 1.30F;
          xPart.FeedSpacing = 4;
          xPart.Speed = 80;
          xPart.Package = PnPSupplyPackage.Reel8mm;
          break;
        case "1210":
          xPart.Offset = new FPoint(0.24f, 0.02F);
          xPart.Nozzle = PnPMachineNozzle.L;
          xPart.PartHeight = 1.40F;
          xPart.FeedSpacing = 4;
          xPart.Speed = 80;
          xPart.Package = PnPSupplyPackage.Reel8mm;
          break;
      }
      return xPart;
    }

    /// <summary>
    /// Loads the data for templates into the comboboxes.
    /// </summary>
    private void LoadTemplates(object sender, EventArgs e)
    {
      // load jelly bean data into the jelly bean combobox
      this.comJellys.Items.Clear();
      foreach (string sJellySize in JELLY_SIZES)
        this.comJellys.Items.Add(sJellySize);

      // load existing reels into the reel template combobox
      this.comReels.Items.Clear();
      foreach (PnPReel xReel in _xReelCollection)
        this.comReels.Items.Add(xReel);

      // initially disable the buttons until a value is selected
      this.btnJellyTemplate.Enabled = false;
      this.btnReelTemplate.Enabled = false;

      this.lblReelDefinition.Text = "Selecting reel settings for footprint " + _xReel.Footprint.Name + " and value " + _xReel.PartNameOrValue;      
    }

    /// <summary>
    /// Configure the new reel with known jelly bean values.
    /// </summary>
    private void SetJellyBeanTemplate(object sender, EventArgs e)
    {
      JellyBeanSettings xJelly = GetJellyBeanSettings(this.comJellys.SelectedItem.ToString());
      this.xReelSettingsPicker.Offset = xJelly.Offset;
      this.xReelSettingsPicker.Nozzle = xJelly.Nozzle;
      this.xReelSettingsPicker.PartHeight = xJelly.PartHeight;
      this.xReelSettingsPicker.FeedSpacing = xJelly.FeedSpacing;
      this.xReelSettingsPicker.Speed = xJelly.Speed;
      this.xReelSettingsPicker.Package = xJelly.Package;

      if (this.xReelSettingsPicker.Manufacturer == string.Empty) this.xReelSettingsPicker.Manufacturer = "Generic";
      if (this.xReelSettingsPicker.Supplier == string.Empty) this.xReelSettingsPicker.Supplier = "Generic";
      if (this.xReelSettingsPicker.OrderCode == string.Empty) this.xReelSettingsPicker.OrderCode = "Generic";
    }

    /// <summary>
    /// Configure the new reel with data from an existing reel as template.
    /// </summary>
    private void SetExistingReelTemplate(object sender, EventArgs e)
    {
      PnPReel xReel = (PnPReel)this.comReels.SelectedItem;
      //this.xReelSettingsPicker.Manufacturer = xReel.Manufacturer;
      //this.xReelSettingsPicker.Supplier = xReel.Supplier;
      //this.xReelSettingsPicker.OrderCode = xReel.OrderCode;

      if (xReel != null)
      {
        this.xReelSettingsPicker.Package = xReel.SupplyPackage;
        this.xReelSettingsPicker.PartSize = xReel.PartSize;
        this.xReelSettingsPicker.Offset = xReel.Offset;
        this.xReelSettingsPicker.FeedSpacing = xReel.FeedSpacing;

        this.xReelSettingsPicker.Nozzle = xReel.Nozzle;
        this.xReelSettingsPicker.Speed = xReel.Speed;
        this.xReelSettingsPicker.PartHeight = xReel.Footprint.Height;
        this.xReelSettingsPicker.Rotation = xReel.Rotation;
      } 
    }

    /// <summary>
    /// Enables the existing reel template button when a valid reel is selected in the combobox.
    /// </summary>
    private void ReelTemplateChanged(object sender, EventArgs e)
    {
      PnPReel xReel = (PnPReel)this.comReels.SelectedItem;
      this.btnReelTemplate.Enabled = (xReel != null);
    }

    /// <summary>
    /// Enables the jelly bean template button when a valid jelly bean code is selected in the combobox.
    /// </summary>
    private void JellyTemplateChanged(object sender, EventArgs e)
    {
      JellyBeanSettings xSettings = GetJellyBeanSettings(this.comJellys.SelectedItem.ToString());
      this.btnJellyTemplate.Enabled = (xSettings.Nozzle != PnPMachineNozzle.Undefined);
    }

    private void NewReelDialog_FormClosing(object sender, FormClosingEventArgs e)
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
