using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PnPConverter
{
  public partial class ProjectWizard
  {
    private void LoadReelCollection()
    {
      _xReelLibrary = PnPReel.LoadReels();
    }

    private void IdentifyMissingReels()
    {
      //_xMissingReelParts.Clear();
      //for (int iCur = 0; iCur < _xPartsList.Count; iCur++)
      //{
      //  PnPPart xCurPart = _xPartsList[iCur];
      //  bool bReelAvailable = false;
      //  foreach (PnPReel xCurReel in _xReelLibrary)
      //  {
      //    if (xCurReel.Footprint.Name == xCurPart.Footprint.Name && xCurReel.PartNumberOrValue == xCurPart.PartNumberOrValue)
      //    {
      //      bReelAvailable = true;
      //      // assign this Reel to the Part:
      //      xCurPart.AssignedReel = xCurReel.GUID;
      //      break;
      //    }
      //  }

      //  if (!bReelAvailable)
      //    _xMissingReelParts.Add(xCurPart);
      //}
    }

    private struct ReelInfo
    {
      public List<string> Designators;
      public PnPFootprint Footprint;
      public string PartNameOrValue;
      public int Index; // for use in the ListView
      public bool DataOK;

      public MissingReelSolution Solution;
      public Guid GUID;
      public bool AutomaticConfig;

      public Guid ReplacementReel;
      //public string ReplacementReelDescription;
      //public string FootprintString;
      //public string PartNumber;
      public PnPSupplyPackage PackageType;
      //public float Height;
      public float XOffset, YOffset;
      public PnPMachineNozzle Nozzle;
      public float FeedSpacing;
      //public int Quantity;
      public string Manufacturer;
      public string Supplier;
      public string OrderCode;
      public int Speed;
      public int Rotation;

      public float XSize, YSize; // experimental for automatic centring of nozzles for tray and tube parts   
    }

    private enum MissingReelSolution
    {
      StandIn,
      NewReel,
      Exclude,
      Undefined
    }



    private void LoadReelList()
    {
      this.listReelConfiguration.Items.Clear();
      _xReelList.Clear();

      //foreach (PnPPart xCurPart in _xMissingReelParts)
      foreach (PnPPart xCurPart in _xPartsList)
      {
        // search the list of missing reels for a match with the current part
        int iRegisteredIndex = -1;
        foreach (ReelInfo xRegisteredPart in _xReelList)
        {
          if (xRegisteredPart.Footprint.Equals(xCurPart.Footprint) && xRegisteredPart.PartNameOrValue == xCurPart.PartNumberOrValue)
          {
            iRegisteredIndex = xRegisteredPart.Index;
            break;
          }
        }

        // if the index was found (= reel was already registered) then add it, otherwise create a new one
        if (iRegisteredIndex > -1)
        {
          foreach (ReelInfo xPart in _xReelList)
            if (xPart.Index == iRegisteredIndex)
              xPart.Designators.Add(xCurPart.Designator);
        }
        else
        {
          ReelInfo xNewReel = new ReelInfo();
          xNewReel.Designators = new List<string>();
          xNewReel.Designators.Add(xCurPart.Designator);
          xNewReel.Footprint = xCurPart.Footprint;
          xNewReel.PartNameOrValue = xCurPart.PartNumberOrValue;
          xNewReel.Index = _xReelList.Count;
          xNewReel.AutomaticConfig = true;
          xNewReel.Solution = MissingReelSolution.Undefined;

          // see if a Reel with this footprint and value already exists in the library, if it does then mark the "replace" option and save the GUID:
          foreach (PnPReel xCurReel in _xReelLibrary)
          {
            if (xCurReel.Footprint.Name.ToLower() == xCurPart.Footprint.Name.ToLower() && xCurReel.PartNumberOrValue.ToLower() == xCurPart.PartNumberOrValue.ToLower())
            {
              xNewReel.Solution = MissingReelSolution.StandIn;
              xNewReel.ReplacementReel = xCurReel.GUID;
              //xNewReel.AutomaticConfig = false;
              xNewReel.DataOK = true;
            }
          }

          _xReelList.Add(xNewReel);
        }
      }

      // configure the icon
      ImageList xImages = new ImageList();
      xImages.Images.Add(GetOKImage());
      xImages.Images.Add(GetPendingImage());
      this.listReelConfiguration.SmallImageList = xImages;

      // now fill the ListView with parts
      for (int iCur = 0; iCur < _xReelList.Count; iCur++)
      {
        ReelInfo xReelInfo = _xReelList[iCur];
        StringBuilder sEntry = new StringBuilder("   "); // some space between text and icon
        for (int i = 0; i < xReelInfo.Designators.Count - 1; i++)
        {
          sEntry.Append(xReelInfo.Designators[i]);
          sEntry.Append(", ");
        }
        sEntry.Append(xReelInfo.Designators[xReelInfo.Designators.Count - 1]);

        string[] sNewEntry = new string[] { sEntry.ToString(), xReelInfo.Footprint.Name, xReelInfo.PartNameOrValue };

        ListViewItem xNewListEntry = new ListViewItem(sNewEntry);
        xNewListEntry.Tag = xReelInfo.Index;
        xNewListEntry.Tag = xReelInfo.Index;

        // mark existing reels as "ok":
        if (xReelInfo.Solution == MissingReelSolution.StandIn)
        {
          xNewListEntry.ImageIndex = 0; // ok image
        } 
        else
        {
          xNewListEntry.ImageIndex = -1; // ok image
        }

        this.listReelConfiguration.Items.Add(xNewListEntry);
      }

      // check if there are Parts without Reel assigned to them. If there are, highlight the first one, if not highlight "Next" button
      int iIndex = -1;
      foreach (ReelInfo xListEntry in _xReelList)
        if (xListEntry.Solution == MissingReelSolution.Undefined)
          if (iIndex == -1)
            iIndex = xListEntry.Index;
          else
            if (xListEntry.Index < iIndex)
              iIndex = xListEntry.Index;

      if (iIndex != -1)
      {
        // at least one Part with missing data, highlight that row:
        this.listReelConfiguration.Select();
        this.listReelConfiguration.Items[iIndex].Selected = true;
      }

      // check if there are überhaupt any unassigned items:
      bool bAllOK = true;
      foreach (ReelInfo xCurReel in _xReelList)
        if (!xCurReel.DataOK)
          bAllOK = false;

      if (bAllOK)
      {
        // no parts with missing reels!
        this.btnNext.Enabled = true;
        this.lblMissingReels.Text = "Packaging information for all parts is available, click Next to proceed.";
      }
      else
      {
        // parts with missing data!
        this.btnNext.Enabled = false;
        this.lblMissingReels.Text = "Packaging information is missing for the following part(s):";
      }
    }

    private void AutomaticReelConfig()
    {
      if (_iCurrentItem == -1)
        return;

      ReelInfo newReel = _xReelList[_iCurrentItem];
      if (!newReel.AutomaticConfig)
        return;

      // if this footprint already exists in the library, copy its settings
      foreach (PnPReel xCurReel in _xReelLibrary)
      {
        if (xCurReel.Footprint.Name.Trim().ToLower() == newReel.Footprint.Name.Trim().ToLower())
        {
          // footprint match
          newReel.FeedSpacing = xCurReel.FeedSpacing;
          newReel.Nozzle = xCurReel.Nozzle;
          newReel.PackageType = xCurReel.SupplyPackage;
          newReel.Speed = xCurReel.Speed;
          newReel.XOffset = xCurReel.XOffset;
          newReel.YOffset = xCurReel.YOffset;
          break;
        }
      }

      // try to deduct suitable values for jellybean parts based on their footprint
      List<string> sJellyBeans = new List<string>() { "CAP", "RES", "IND", "FUSE", "LED" };
      bool bIsJellyPart = false;
      foreach (string sCur in sJellyBeans)
        if (newReel.Footprint.Name.ToUpper().Trim().StartsWith(sCur))
          bIsJellyPart = true;

      //List<string> sJellyFormats = new List<string>() {"0402", "0603", "0805", "1206", "1210"};
      //bool bIsJellyFormat = false;
      //foreach (string sCur in sJellyFormats)
      //  if (newReel.Footprint.Name.Trim().EndsWith(sCur))
      //    bIsJellyFormat = true;

      if (bIsJellyPart)
      {
        string sFootprintSize = newReel.Footprint.Name.Trim().Substring(newReel.Footprint.Name.Trim().Length - 4);
        switch (sFootprintSize)
        {
          case "0402":
            newReel.XOffset = 0;
            newReel.YOffset = 0.05F;
            newReel.Nozzle = PnPMachineNozzle.XS;
            newReel.Footprint.Height = 0.5F;
            newReel.FeedSpacing = 2;
            newReel.Speed = 100;
            newReel.PackageType = PnPSupplyPackage.Reel8mm;
            break;
          case "0603":
            newReel.XOffset = -0.04F;
            newReel.YOffset = -0.1F;
            newReel.Nozzle = PnPMachineNozzle.S;
            newReel.Footprint.Height = 0.95F;
            newReel.FeedSpacing = 4;
            newReel.Speed = 100;
            newReel.PackageType = PnPSupplyPackage.Reel8mm;
            break;
          case "0805":
            newReel.XOffset = 0.05F;
            newReel.YOffset = 0;
            newReel.Nozzle = PnPMachineNozzle.S;
            newReel.Footprint.Height = 1.10F;
            newReel.FeedSpacing = 4;
            newReel.Speed = 90;
            newReel.PackageType = PnPSupplyPackage.Reel8mm;
            break;
          case "1206":
            newReel.XOffset = 0.23F;
            newReel.YOffset = 0;
            newReel.Nozzle = PnPMachineNozzle.M;
            newReel.Footprint.Height = 1.30F;
            newReel.FeedSpacing = 4;
            newReel.Speed = 80;
            newReel.PackageType = PnPSupplyPackage.Reel8mm;
            break;
          case "1210":
            newReel.XOffset = 0.24F;
            newReel.YOffset = 0.02F;
            newReel.Nozzle = PnPMachineNozzle.L;
            newReel.Footprint.Height = 1.40F;
            newReel.FeedSpacing = 4;
            newReel.Speed = 80;
            newReel.PackageType = PnPSupplyPackage.Reel8mm;
            break;
        }

        // DEBUG:
        // jelly parts rotated by 90° in Altium!
        newReel.Rotation = 90;
      }

      // try to deduct values for common IC types
      string sICFootprint = newReel.Footprint.Name.Trim().ToUpper();
      if (
        sICFootprint.StartsWith("IC-SOIC-") ||
        sICFootprint.StartsWith("IC-TSSOP-") ||
        sICFootprint.StartsWith("IC-MSOP-") ||
        sICFootprint.StartsWith("IC-POWERSO-") ||
        sICFootprint.StartsWith("IC-SSOP-") ||
        sICFootprint.StartsWith("IC-VQFN-")
      )
      {
        newReel.Nozzle = PnPMachineNozzle.L;
        newReel.Footprint.Height = 2F;
        newReel.FeedSpacing = 18;
        newReel.Speed = 10;
        newReel.PackageType = PnPSupplyPackage.Tray;
      }

      // search the parts list for matching (footprint, value) combinations to load manufacturer properties automatically
      foreach (PnPPart xCurPart in _xPartsList)
      {
        if (xCurPart.PartNumberOrValue == newReel.PartNameOrValue && xCurPart.Footprint.Name == newReel.Footprint.Name)
        {
          newReel.Manufacturer = xCurPart.Manufacturer;
          newReel.Supplier = xCurPart.Supplier;
          newReel.OrderCode = xCurPart.OrderCode;
          break;
        }
      }

      newReel.Speed = 50; // default

      newReel.GUID = Guid.NewGuid();
      newReel.AutomaticConfig = false;

      _xReelList[_iCurrentItem] = newReel;
    }

    /// <summary>
    /// Loads a list with Reels into the ComboBox.
    /// </summary>
    private void LoadReelLibraryList()
    {
      this.comCurReplace.Items.Clear();
      foreach (PnPReel xCurReel in _xReelLibrary)
        this.comCurReplace.Items.Add(xCurReel.ToString());
    }

    /// <summary>
    /// The item that is currently selected in the ListView.
    /// It is important to store this to be able to store data in the correct ReelInfo when the user picks a new item from the ListView!
    /// </summary>
    private int _iCurrentItem = -1;

    /// <summary>
    /// Stores the configuration that is selected for the current item into its ReelInfo container before switching to another item.
    /// </summary>
    private void StoreReelSettings()
    {
      if (_iCurrentItem == -1)
        return;

      // user has selected another part, so store the existing values into the reel info struct
      ReelInfo xCurReel = new ReelInfo();
      foreach (ReelInfo xReel in _xReelList)
        if (xReel.Index == _iCurrentItem)
          xCurReel = xReel;

      // store the selected solution:
      if (this.rdbReplacePart.Checked)
        xCurReel.Solution = MissingReelSolution.StandIn;
      else if (this.rdbAddNew.Checked)
        xCurReel.Solution = MissingReelSolution.NewReel;
      else if (this.rdbExclude.Checked)
        xCurReel.Solution = MissingReelSolution.Exclude;
      else
        xCurReel.Solution = MissingReelSolution.Undefined;

      // if an existing reel was selected, store this info
      // DEBUG: check GUID length
      if (this.comCurReplace.SelectedIndex > -1)
      {
        string sSelectedReel = this.comCurReplace.Items[this.comCurReplace.SelectedIndex].ToString();
        string sGUID = sSelectedReel.Substring(sSelectedReel.Length - 37, 36);
        xCurReel.ReplacementReel = new Guid(sGUID);
      }

      // settings for new reels:
      xCurReel.Manufacturer = this.xReelSettingsPicker.Manufacturer;
      xCurReel.OrderCode = this.xReelSettingsPicker.OrderCode;
      xCurReel.Supplier = this.xReelSettingsPicker.Supplier;
      xCurReel.Rotation = this.xReelSettingsPicker.Rotation;
      xCurReel.Speed = this.xReelSettingsPicker.Speed;
      xCurReel.PackageType = this.xReelSettingsPicker.Package;
      xCurReel.Nozzle = this.xReelSettingsPicker.Nozzle;
      xCurReel.Footprint.Height = this.xReelSettingsPicker.PartHeight;
      xCurReel.XOffset = this.xReelSettingsPicker.OffsetX;
      xCurReel.YOffset = this.xReelSettingsPicker.OffsetY;
      xCurReel.FeedSpacing = this.xReelSettingsPicker.FeedSpacing;
      xCurReel.XSize = this.xReelSettingsPicker.SizeX;
      xCurReel.YSize = this.xReelSettingsPicker.SizeY;

      _xReelList[_iCurrentItem] = xCurReel;
    }

    /// <summary>
    /// Loads the corresponding configuration for the item that the user selected from the ListView.
    /// </summary>
    private void LoadReelSettings()
    {
      if (_iCurrentItem == -1)
        return;

      // retrieve the reel info object that holds the data for this entry:
      //int iCur = (int)this.listMissingReels.SelectedItems[0].Tag;
      AutomaticReelConfig();
      ReelInfo xCurEntry = _xReelList[_iCurrentItem];

      // enable appropriate controls:
      this.rdbReplacePart.Checked = false;
      this.rdbAddNew.Checked = false;
      this.rdbExclude.Checked = false;
      this.comCurReplace.Enabled = false;
      this.xReelSettingsPicker.Enabled = false;

      switch (xCurEntry.Solution)
      {
        case MissingReelSolution.StandIn:
          this.rdbReplacePart.Checked = true;
          this.comCurReplace.Enabled = true;
          break;

        case MissingReelSolution.NewReel:
          this.rdbAddNew.Checked = true;
          this.xReelSettingsPicker.Enabled = true;
          break;

        case MissingReelSolution.Exclude:
          this.rdbExclude.Checked = true;
          break;
      }

      this.lblCurGUID.Text = "Unique reel ID: " + xCurEntry.GUID.ToString();
      this.xReelSettingsPicker.Manufacturer = xCurEntry.Manufacturer;
      this.xReelSettingsPicker.Supplier = xCurEntry.Supplier;
      this.xReelSettingsPicker.OrderCode = xCurEntry.OrderCode;
      this.xReelSettingsPicker.OffsetX = xCurEntry.XOffset;
      this.xReelSettingsPicker.OffsetY = xCurEntry.YOffset;
      this.xReelSettingsPicker.SizeX = xCurEntry.XSize;
      this.xReelSettingsPicker.SizeY = xCurEntry.YSize;
      this.xReelSettingsPicker.Nozzle = xCurEntry.Nozzle;
      this.xReelSettingsPicker.Package = xCurEntry.PackageType;
      this.xReelSettingsPicker.Speed = xCurEntry.Speed;
      this.xReelSettingsPicker.FeedSpacing = xCurEntry.FeedSpacing;
      this.xReelSettingsPicker.Rotation = xCurEntry.Rotation;
      this.xReelSettingsPicker.PartHeight = xCurEntry.Footprint.Height;

      if (this.rdbReplacePart.Checked)
        this.comCurReplace.SelectedIndex = -1;

      // search the entry in the combobox that matches the replacment GUID and select it
      if (!xCurEntry.ReplacementReel.Equals(Guid.Empty))
      {
        int iReelIndex = -1;
        for (int iCur = 0; iCur < this.comCurReplace.Items.Count; iCur++)
        {
          object xItem = this.comCurReplace.Items[iCur];
          if (xItem.ToString().EndsWith("[" + xCurEntry.ReplacementReel.ToString() + "]"))
            iReelIndex = iCur;
        }
        this.comCurReplace.SelectedIndex = iReelIndex;
      }
    }

    
    /// <summary>
    /// User picks another item from the ListView.
    /// </summary>
    private void SwitchReel(object sender, EventArgs e)
    {
      if (this.listReelConfiguration.SelectedItems.Count > 0)
      {
        // store settings at old index:
        StoreReelSettings();
        if (_iCurrentItem != -1)
        {
          bool bValidity = CheckSolution();
          if (bValidity)
            this.listReelConfiguration.Items[_iCurrentItem].ImageIndex = 0;
          else
            this.listReelConfiguration.Items[_iCurrentItem].ImageIndex = 1;

          ReelInfo xReel = _xReelList[_iCurrentItem];
          xReel.DataOK = bValidity;
          _xReelList[_iCurrentItem] = xReel;
        }

        // switch index to new item:
        _iCurrentItem = (int)this.listReelConfiguration.SelectedItems[0].Tag;
        this.lblMissingReels.Text = "Selected index: " + this.listReelConfiguration.SelectedItems[0].ToString();

        // load settings at new index:
        LoadReelSettings();

        // check if the Next button can be enabled if all data is correct:
        bool bDataValid = true;
        foreach (ReelInfo xCurReel in _xReelList)
          if (!xCurReel.DataOK)
            bDataValid = false;
        this.btnNext.Enabled = bDataValid;
      }
    }

    /// <summary>
    /// User ticked another Radio Button to change the configuration of this Reel.
    /// </summary>
    private void ChangeSolution(object sender, EventArgs e)
    {
      this.xReelSettingsPicker.Enabled = false;
      this.comCurReplace.Enabled = false;

      if (rdbAddNew.Checked)
      {
        this.xReelSettingsPicker.Enabled = true;
      }
      else if (rdbReplacePart.Checked)
      {
        this.comCurReplace.Enabled = true;
      }
      else if (rdbExclude.Checked)
      {
        // nothing needs to be enabled here
      }
    }

    /// <summary>
    /// Verify that a proper solution for these Parts has been found.
    /// </summary>
    /// <returns>A value indicating whether the selected configuration is ok.</returns>
    public bool CheckSolution()
    {
      if (_iCurrentItem == -1)
        return false;

      bool bState = false;
      if (this.rdbReplacePart.Checked)
      {
        bState = (this.comCurReplace.SelectedIndex > -1);
      }
      else if (this.rdbAddNew.Checked)
      {
        bState = xReelSettingsPicker.VerifyInput();
      }
      else if (this.rdbExclude.Checked)
      {
        bState = true;
      }
      else
      {
        bState = false;
      }
      return bState;
    }


  }
}
