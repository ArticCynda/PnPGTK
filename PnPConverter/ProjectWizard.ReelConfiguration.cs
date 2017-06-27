using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PnPConverter
{
  public partial class ProjectWizard
  {

    /// <summary>
    /// Loads the library of known Reels from disk.
    /// </summary>
    private void LoadReelCollection()
    {
      // load existing reels from disk
      _xReelLibrary = PnPReel.LoadReels();
    }

    

    //private void IdentifyMissingReels()
    //{
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
    //}

    private void FillReelList()
    {
      // TODO: check for duplicate part designators!!

      // clear the listview
      this.listReelConfiguration.Items.Clear();

      // clear the list of reels that are required for the current project
      _xPartTypeList.Clear();

      // loop through the parts and assign them to their respective part types
      foreach (PnPPart xCurPart in _xPartsList)
      {
        // search the list of part types for a match with the current part
        bool bFound = false;
        for (int iCur = 0; iCur < _xPartTypeList.Count; iCur++)
        {
          PnPPartType xPartType = _xPartTypeList[iCur];
          if (xPartType.Footprint.Name == xCurPart.Footprint.Name && xPartType.PartNameOrValue == xCurPart.PartNameOrValue)
          {
            bFound = true;
            xPartType.Designators.Add(xCurPart.Designator); // add this part to the reel list
            break;
          }
        }

        // if the part type doesn't exist yet in the project, then add it to the list:
        if (!bFound)
        {
          PnPPartType xCurPartType = new PnPPartType();
          xCurPartType.Designators = new List<string>();
          xCurPartType.GUID = Guid.Empty;
          xCurPartType.Designators.Add(xCurPart.Designator);
          xCurPartType.Footprint = xCurPart.Footprint;
          xCurPartType.PartNameOrValue = xCurPart.PartNameOrValue;
          xCurPartType.Index = _xPartTypeList.Count;
          xCurPartType.Solution = PartPlaceSolution.Undefined;
          
          _xPartTypeList.Add(xCurPartType);
        }  
      }

      // try to find a matching reel for every part type
      for (int iCur = 0; iCur < _xPartTypeList.Count; iCur++)
      {
        PnPPartType xCurType = _xPartTypeList[iCur];
        foreach (PnPReel xCurReel in _xReelLibrary)
          if (xCurReel.Footprint.Name == xCurType.Footprint.Name && xCurReel.PartNameOrValue == xCurType.PartNameOrValue)
          {
            xCurType.GUID = xCurReel.GUID;
            xCurType.Solution = PartPlaceSolution.Assigned;
            _xPartTypeList[iCur] = xCurType;
            break;
          }
      }

      // configure the icon
      ImageList xImages = new ImageList();
      xImages.Images.Add(GetOKImage());
      xImages.Images.Add(GetPendingImage());
      this.listReelConfiguration.SmallImageList = xImages;

      // load the part types in the listview
      foreach (PnPPartType xCurPartType in _xPartTypeList)
      {
        StringBuilder sEntry = new StringBuilder("   "); // some space between text and icon

        // add all designators that are associated with this part type
        for (int i = 0; i < xCurPartType.Designators.Count - 1; i++)
        {
          sEntry.Append(xCurPartType.Designators[i]);
          sEntry.Append(", ");
        }
        sEntry.Append(xCurPartType.Designators[xCurPartType.Designators.Count - 1]);

        // create a new list view item
        string[] sNewEntry = new string[] { sEntry.ToString(), xCurPartType.Footprint.Name, xCurPartType.PartNameOrValue };
        ListViewItem xNewListEntry = new ListViewItem(sNewEntry);

        // mark part types with existing reels as "ok":
        if (xCurPartType.GUID != Guid.Empty)
          xNewListEntry.ImageIndex = 0; // ok image
        else
          xNewListEntry.ImageIndex = -1; // ok image

        this.listReelConfiguration.Items.Add(xNewListEntry);
      }

      // search the first unassigned item and select it, if none can be found then enable Next button
      int iUnassigned = -1;
      _iCurrentItem = -1;
      foreach (PnPPartType xCurPartType in _xPartTypeList)
      {
        if (xCurPartType.GUID == Guid.Empty)
        {
          iUnassigned = xCurPartType.Index;
          this.listReelConfiguration.Items[iUnassigned].Selected = true;
          LoadPartTypeSettings(xCurPartType);
          break;
        }
      }
      if (iUnassigned == -1)
      {
        this.btnNext.Enabled = true;
        this.btnNext.Focus();
        if (_xPartTypeList.Count > 1)
        {
          this.listReelConfiguration.Items[0].Selected = true;
          LoadPartTypeSettings(_xPartTypeList[0]);
        }
      }

      
      


      //this.listReelConfiguration.Items.Clear();
      //_xReelList.Clear();

      ////foreach (PnPPart xCurPart in _xMissingReelParts)
      //foreach (PnPPart xCurPart in _xPartsList)
      //{
      //  // search the list of missing reels for a match with the current part
      //  int iRegisteredIndex = -1;
      //  foreach (PartInfo xRegisteredPart in _xReelList)
      //  {
      //    if (xRegisteredPart.Footprint.Equals(xCurPart.Footprint) && xRegisteredPart.PartNameOrValue == xCurPart.PartNumberOrValue)
      //    {
      //      iRegisteredIndex = xRegisteredPart.Index;
      //      break;
      //    }
      //  }

      //  // if the index was found (= reel was already registered) then add it, otherwise create a new one
      //  if (iRegisteredIndex > -1)
      //  {
      //    foreach (PartInfo xPart in _xReelList)
      //      if (xPart.Index == iRegisteredIndex)
      //        xPart.Designators.Add(xCurPart.Designator);
      //  }
      //  else
      //  {
      //    PartInfo xNewReel = new PartInfo();
      //    xNewReel.Designators = new List<string>();
      //    xNewReel.Designators.Add(xCurPart.Designator);
      //    xNewReel.Footprint = xCurPart.Footprint;
      //    xNewReel.PartNameOrValue = xCurPart.PartNumberOrValue;
      //    xNewReel.Index = _xReelList.Count;
      //    xNewReel.AutomaticConfig = true;
      //    xNewReel.Solution = PartPlaceSolution.Undefined;

      //    // see if a Reel with this footprint and value already exists in the library, if it does then mark the "replace" option and save the GUID:
      //    foreach (PnPReel xCurReel in _xReelLibrary)
      //    {
      //      if (xCurReel.Footprint.Name.ToLower() == xCurPart.Footprint.Name.ToLower() && xCurReel.PartNumberOrValue.ToLower() == xCurPart.PartNumberOrValue.ToLower())
      //      {
      //        xNewReel.Solution = PartPlaceSolution.StandIn;
      //        xNewReel.ReplacementReel = xCurReel.GUID;
      //        //xNewReel.AutomaticConfig = false;
      //        xNewReel.DataOK = true;
      //      }
      //    }

      //    _xReelList.Add(xNewReel);
      //  }
      //}

      //// configure the icon
      //ImageList xImages = new ImageList();
      //xImages.Images.Add(GetOKImage());
      //xImages.Images.Add(GetPendingImage());
      //this.listReelConfiguration.SmallImageList = xImages;

      //// now fill the ListView with parts
      //for (int iCur = 0; iCur < _xReelList.Count; iCur++)
      //{
      //  PartInfo xReelInfo = _xReelList[iCur];
      //  StringBuilder sEntry = new StringBuilder("   "); // some space between text and icon
      //  for (int i = 0; i < xReelInfo.Designators.Count - 1; i++)
      //  {
      //    sEntry.Append(xReelInfo.Designators[i]);
      //    sEntry.Append(", ");
      //  }
      //  sEntry.Append(xReelInfo.Designators[xReelInfo.Designators.Count - 1]);

      //  string[] sNewEntry = new string[] { sEntry.ToString(), xReelInfo.Footprint.Name, xReelInfo.PartNameOrValue };

      //  ListViewItem xNewListEntry = new ListViewItem(sNewEntry);
      //  xNewListEntry.Tag = xReelInfo.Index;
      //  xNewListEntry.Tag = xReelInfo.Index;

      //  // mark existing reels as "ok":
      //  if (xReelInfo.Solution == PartPlaceSolution.StandIn)
      //  {
      //    xNewListEntry.ImageIndex = 0; // ok image
      //  } 
      //  else
      //  {
      //    xNewListEntry.ImageIndex = -1; // ok image
      //  }

      //  this.listReelConfiguration.Items.Add(xNewListEntry);
      //}

      //// check if there are Parts without Reel assigned to them. If there are, highlight the first one, if not highlight "Next" button
      //int iIndex = -1;
      //foreach (PartInfo xListEntry in _xReelList)
      //  if (xListEntry.Solution == PartPlaceSolution.Undefined)
      //    if (iIndex == -1)
      //      iIndex = xListEntry.Index;
      //    else
      //      if (xListEntry.Index < iIndex)
      //        iIndex = xListEntry.Index;

      //if (iIndex != -1)
      //{
      //  // at least one Part with missing data, highlight that row:
      //  this.listReelConfiguration.Select();
      //  this.listReelConfiguration.Items[iIndex].Selected = true;
      //}

      //// check if there are überhaupt any unassigned items:
      //bool bAllOK = true;
      //foreach (PartInfo xCurReel in _xReelList)
      //  if (!xCurReel.DataOK)
      //    bAllOK = false;

      //if (bAllOK)
      //{
      //  // no parts with missing reels!
      //  this.btnNext.Enabled = true;
      //  this.lblMissingReels.Text = "Packaging information for all parts is available, click Next to proceed.";
      //}
      //else
      //{
      //  // parts with missing data!
      //  this.btnNext.Enabled = false;
      //  this.lblMissingReels.Text = "Packaging information is missing for the following part(s):";
      //}

    }

    /// <summary>
    /// Attempts to configure the settings for a new reel automatically based on known configurations or values
    /// </summary>
    //private void AutomaticReelConfig()
    //{
    //  if (_iCurrentItem == -1)
    //    return;

    //  PnPPartType newReel = _xPartTypeList[_iCurrentItem];
    //  if (!newReel.AutomaticConfig)
    //    return;

    //  // if this footprint already exists in the library, copy its settings
    //  foreach (PnPReel xCurReel in _xReelLibrary)
    //  {
    //    if (xCurReel.Footprint.Name.Trim().ToLower() == newReel.Footprint.Name.Trim().ToLower())
    //    {
    //      // footprint match
    //      newReel.FeedSpacing = xCurReel.FeedSpacing;
    //      newReel.Nozzle = xCurReel.Nozzle;
    //      newReel.PackageType = xCurReel.SupplyPackage;
    //      newReel.Speed = xCurReel.Speed;
    //      newReel.XOffset = xCurReel.XOffset;
    //      newReel.YOffset = xCurReel.YOffset;
    //      break;
    //    }
    //  }

    //  // try to deduct suitable values for jellybean parts based on their footprint
    //  List<string> sJellyBeans = new List<string>() { "CAP", "RES", "IND", "FUSE", "LED" };
    //  bool bIsJellyPart = false;
    //  foreach (string sCur in sJellyBeans)
    //    if (newReel.Footprint.Name.ToUpper().Trim().StartsWith(sCur))
    //      bIsJellyPart = true;

    //  if (bIsJellyPart)
    //  {
    //    string sFootprintSize = newReel.Footprint.Name.Trim().Substring(newReel.Footprint.Name.Trim().Length - 4);
    //    switch (sFootprintSize)
    //    {
    //      case "0402":
    //        newReel.XOffset = 0;
    //        newReel.YOffset = 0.05F;
    //        newReel.Nozzle = PnPMachineNozzle.XS;
    //        newReel.Footprint.Height = 0.5F;
    //        newReel.FeedSpacing = 2;
    //        newReel.Speed = 100;
    //        newReel.PackageType = PnPSupplyPackage.Reel8mm;
    //        break;
    //      case "0603":
    //        newReel.XOffset = -0.04F;
    //        newReel.YOffset = -0.1F;
    //        newReel.Nozzle = PnPMachineNozzle.S;
    //        newReel.Footprint.Height = 0.95F;
    //        newReel.FeedSpacing = 4;
    //        newReel.Speed = 100;
    //        newReel.PackageType = PnPSupplyPackage.Reel8mm;
    //        break;
    //      case "0805":
    //        newReel.XOffset = 0.05F;
    //        newReel.YOffset = 0;
    //        newReel.Nozzle = PnPMachineNozzle.S;
    //        newReel.Footprint.Height = 1.10F;
    //        newReel.FeedSpacing = 4;
    //        newReel.Speed = 90;
    //        newReel.PackageType = PnPSupplyPackage.Reel8mm;
    //        break;
    //      case "1206":
    //        newReel.XOffset = 0.23F;
    //        newReel.YOffset = 0;
    //        newReel.Nozzle = PnPMachineNozzle.M;
    //        newReel.Footprint.Height = 1.30F;
    //        newReel.FeedSpacing = 4;
    //        newReel.Speed = 80;
    //        newReel.PackageType = PnPSupplyPackage.Reel8mm;
    //        break;
    //      case "1210":
    //        newReel.XOffset = 0.24F;
    //        newReel.YOffset = 0.02F;
    //        newReel.Nozzle = PnPMachineNozzle.L;
    //        newReel.Footprint.Height = 1.40F;
    //        newReel.FeedSpacing = 4;
    //        newReel.Speed = 80;
    //        newReel.PackageType = PnPSupplyPackage.Reel8mm;
    //        break;
    //    }

    //    // DEBUG:
    //    // jelly parts rotated by 90° in Altium!
    //    newReel.Rotation = 90;
    //  }

    //  // try to deduct values for common IC types
    //  string sICFootprint = newReel.Footprint.Name.Trim().ToUpper();
    //  if (
    //    sICFootprint.StartsWith("IC-SOIC-") ||
    //    sICFootprint.StartsWith("IC-TSSOP-") ||
    //    sICFootprint.StartsWith("IC-MSOP-") ||
    //    sICFootprint.StartsWith("IC-POWERSO-") ||
    //    sICFootprint.StartsWith("IC-SSOP-") ||
    //    sICFootprint.StartsWith("IC-VQFN-")
    //  )
    //  {
    //    newReel.Nozzle = PnPMachineNozzle.L;
    //    newReel.Footprint.Height = 2F;
    //    newReel.FeedSpacing = 18;
    //    newReel.Speed = 10;
    //    newReel.PackageType = PnPSupplyPackage.Tray;
    //  }

    //  // search the parts list for matching (footprint, value) combinations to load manufacturer properties automatically
    //  foreach (PnPPart xCurPart in _xPartsList)
    //  {
    //    if (xCurPart.PartNameOrValue == newReel.PartNameOrValue && xCurPart.Footprint.Name == newReel.Footprint.Name)
    //    {
    //      newReel.Manufacturer = xCurPart.Manufacturer;
    //      newReel.Supplier = xCurPart.Supplier;
    //      newReel.OrderCode = xCurPart.OrderCode;
    //      break;
    //    }
    //  }

    //  newReel.Speed = 50; // default

    //  newReel.GUID = Guid.NewGuid();
    //  newReel.AutomaticConfig = false;

    //  _xReelList[_iCurrentItem] = newReel;
    //}

    /// <summary>
    /// Loads a list with Reels into the ComboBox.
    /// </summary>
    private void LoadReelLibraryList()
    {
      this.comReels.Items.Clear();
      foreach (PnPReel xCurReel in _xReelLibrary)
        this.comReels.Items.Add(xCurReel);
    }

    //private void SaveNewReels()
    //{
    //  // if the user has provided new reel data, then add these reels to the library:
    //  bool bNewReelsAdded = false;
    //  foreach (PartInfo xNewReel in _xReelList)
    //  {
    //    if (xNewReel.Solution == PartPlaceSolution.NewReel)
    //    {
    //      PnPReel xReel = new PnPReel();
    //      // copy relevant properties
    //      xReel.GUID = xNewReel.GUID;
    //      xReel.PartNameOrValue = xNewReel.PartNameOrValue;
    //      xReel.Manufacturer = xNewReel.Manufacturer;
    //      xReel.Supplier = xNewReel.Supplier;
    //      xReel.OrderCode = xNewReel.OrderCode;
    //      xReel.SupplyPackage = xNewReel.PackageType;
    //      xReel.Footprint = xNewReel.Footprint;
    //      xReel.FeedSpacing = xNewReel.FeedSpacing;
    //      xReel.Nozzle = xNewReel.Nozzle;
    //      xReel.XOffset = xNewReel.XOffset;
    //      xReel.YOffset = xNewReel.YOffset;
    //      xReel.Speed = xNewReel.Speed;
    //      xReel.Rotation = xNewReel.Rotation;

    //      // add this reel to the collection:
    //      _xReelLibrary.Add(xReel);
    //      bNewReelsAdded = true;

    //      // experimental
    //      // assign the newly created Reel to the associated Parts
    //      for (int iCur = 0; iCur < _xPartsList.Count; iCur++)
    //      {
    //        PnPPart xCurPart = _xPartsList[iCur];
    //        if (xNewReel.Designators.Contains(xCurPart.Designator))
    //          xCurPart.AssignedReel = xNewReel.GUID;
    //      }
    //    }
    //    // experimental
    //    else if (xNewReel.Solution == PartPlaceSolution.StandIn)
    //    {
    //      // assign the selected Reel to replace the actual one to the associated Parts
    //      for (int iCur = 0; iCur < _xPartsList.Count; iCur++)
    //      {
    //        PnPPart xCurPart = _xPartsList[iCur];
    //        if (xNewReel.Designators.Contains(xCurPart.Designator))
    //          xCurPart.AssignedReel = xNewReel.ReplacementReel;
    //      }
    //    }
    //  }

    //  // save the library to disk:
    //  if (bNewReelsAdded)
    //    PnPReel.SaveReels(_xReelLibrary);
    //}

    /// <summary>
    /// The item that is currently selected in the ListView.
    /// It is important to store this to be able to store data in the correct ReelInfo when the user picks a new item from the ListView!
    /// </summary>
    private int _iCurrentItem = -1;

    /// <summary>
    /// Stores the configuration that is selected for the current item into its ReelInfo container before switching to another item.
    /// </summary>
    private PnPPartType StorePartTypeSettings(PnPPartType partType)
    {
      //if (_iCurrentItem == -1)
        //return;

      // user has selected another part, so store the existing values into the reel info struct
      //PnPPartType xCurPartType = new PnPPartType();
      //foreach (PartInfo xReel in _xReelList)
      //  if (xReel.Index == _iCurrentItem)
      //    xCurReel = xReel;

      // store the selected solution:
      if (this.rdbUseExisting.Checked)
        partType.Solution = PartPlaceSolution.Assigned;
      else if (this.rdbExclude.Checked)
        partType.Solution = PartPlaceSolution.Excluded;
      else
        partType.Solution = PartPlaceSolution.Undefined;

      // if an existing reel was selected, store this info
      if (this.comReels.SelectedIndex > -1)
      {
        partType.GUID = ((PnPReel)this.comReels.SelectedItem).GUID;      


      //  //string sSelectedReel = this.comCurReplace.Items[this.comCurReplace.SelectedIndex].ToString();
      //  //string sGUID = sSelectedReel.Substring(sSelectedReel.Length - 37, 36);
      //  //xCurReel.ReplacementReel = new Guid(sGUID);
      //  PnPReel xReel = (PnPReel)this.comReels.Items[this.comReels.SelectedIndex];
      //  xCurReel.ReplacementReel = xReel.GUID;        
      }

      // settings for new reels:
      //xCurReel.Manufacturer = this.xReelSettingsPicker.Manufacturer;
      //xCurReel.OrderCode = this.xReelSettingsPicker.OrderCode;
      //xCurReel.Supplier = this.xReelSettingsPicker.Supplier;
      //xCurReel.Rotation = this.xReelSettingsPicker.Rotation;
      //xCurReel.Speed = this.xReelSettingsPicker.Speed;
      //xCurReel.PackageType = this.xReelSettingsPicker.Package;
      //xCurReel.Nozzle = this.xReelSettingsPicker.Nozzle;
      //xCurReel.Footprint.Height = this.xReelSettingsPicker.PartHeight;
      //xCurReel.XOffset = this.xReelSettingsPicker.OffsetX;
      //xCurReel.YOffset = this.xReelSettingsPicker.OffsetY;
      //xCurReel.FeedSpacing = this.xReelSettingsPicker.FeedSpacing;
      //xCurReel.XSize = this.xReelSettingsPicker.SizeX;
      //xCurReel.YSize = this.xReelSettingsPicker.SizeY;

      //_xPartTypeList[_iCurrentItem] = xCurReel;
      return partType;
    }

    /// <summary>
    /// Loads the corresponding configuration for the item that the user selected from the ListView.
    /// </summary>
    private void LoadPartTypeSettings(PnPPartType partType)
    {
      //if (_iCurrentItem == -1)
        //return;

      // retrieve the reel info object that holds the data for this entry:
      //int iCur = (int)this.listMissingReels.SelectedItems[0].Tag;
      //AutomaticReelConfig();
      //PnPPartType xCurEntry = _xPartTypeList[_iCurrentItem];

      // enable appropriate controls:
      //this.rdbReplacePart.Checked = false;
      this.rdbUseExisting.Checked = false;
      this.rdbExclude.Checked = false;
      this.comReels.Enabled = false;
      this.comReels.SelectedIndex = -1;
      //this.xReelSettingsPicker.Enabled = false;

      switch (partType.Solution)
      {
        case PartPlaceSolution.Assigned:
          this.rdbUseExisting.Checked = true;
          this.comReels.Enabled = true;
          break;

        case PartPlaceSolution.Excluded:
          this.rdbExclude.Checked = true;
          break;
      }

      if (partType.GUID != Guid.Empty)
      {
        // search reel that matches this item
        PnPReel xReel = null;
        foreach (PnPReel xCurReel in _xReelLibrary)
          if (xCurReel.GUID == partType.GUID)
          {
            xReel = xCurReel;
            break;
          }

        this.comReels.SelectedItem = xReel;
      }

      //this.lblCurGUID.Text = "Unique reel ID: " + xCurEntry.GUID.ToString();
      //this.xReelSettingsPicker.Manufacturer = xCurEntry.Manufacturer;
      //this.xReelSettingsPicker.Supplier = xCurEntry.Supplier;
      //this.xReelSettingsPicker.OrderCode = xCurEntry.OrderCode;
      //this.xReelSettingsPicker.OffsetX = xCurEntry.XOffset;
      //this.xReelSettingsPicker.OffsetY = xCurEntry.YOffset;
      //this.xReelSettingsPicker.SizeX = xCurEntry.XSize;
      //this.xReelSettingsPicker.SizeY = xCurEntry.YSize;
      //this.xReelSettingsPicker.Nozzle = xCurEntry.Nozzle;
      //this.xReelSettingsPicker.Package = xCurEntry.PackageType;
      //this.xReelSettingsPicker.Speed = xCurEntry.Speed;
      //this.xReelSettingsPicker.FeedSpacing = xCurEntry.FeedSpacing;
      //this.xReelSettingsPicker.Rotation = xCurEntry.Rotation;
      //this.xReelSettingsPicker.PartHeight = xCurEntry.Footprint.Height;

      //if (this.rdbReplacePart.Checked)
      //  this.comReels.SelectedIndex = -1;

      // search the entry in the combobox that matches the replacment GUID and select it
      //if (!xCurEntry.ReplacementReel.Equals(Guid.Empty))
      //{
      //  int iReelIndex = -1;
      //  for (int iCur = 0; iCur < this.comReels.Items.Count; iCur++)
      //  {
      //    object xItem = this.comReels.Items[iCur];
      //    if (xItem.ToString().EndsWith("[" + xCurEntry.ReplacementReel.ToString() + "]"))
      //      iReelIndex = iCur;
      //  }
      //  this.comReels.SelectedIndex = iReelIndex;
      //}
    }

    
    /// <summary>
    /// User picks another item from the ListView.
    /// </summary>
    private void SwitchReel(object sender, EventArgs e)
    {
      if (this.listReelConfiguration.SelectedItems.Count == 1 && this.listReelConfiguration.SelectedItems[0].Index != _iCurrentItem)
      {
        // store settings at old index:
        if (_iCurrentItem != -1)
        {
          PnPPartType xPrevPartType = _xPartTypeList[_iCurrentItem];
          _xPartTypeList[_iCurrentItem] = StorePartTypeSettings(xPrevPartType);

          if ((this.rdbUseExisting.Checked && this.comReels.SelectedIndex > -1) || this.rdbExclude.Checked)
          {
            this.listReelConfiguration.Items[_iCurrentItem].ImageIndex = 0;
          }
          else
            this.listReelConfiguration.Items[_iCurrentItem].ImageIndex = -1;
        }

        // change index to newly selected entry and load its settings
        _iCurrentItem = this.listReelConfiguration.SelectedItems[0].Index;
        PnPPartType xCurPartType = _xPartTypeList[_iCurrentItem];
        LoadPartTypeSettings(xCurPartType);
        //if (_iCurrentItem != -1)
        //{
          //bool bValidity = CheckSolution();
          //if (bValidity)
          //  this.listReelConfiguration.Items[_iCurrentItem].ImageIndex = 0;
          //else
          //  this.listReelConfiguration.Items[_iCurrentItem].ImageIndex = 1;

          //PnPPartType xReel = _xPartTypeList[_iCurrentItem];
          //xReel.DataOK = bValidity;
          //_xPartTypeList[_iCurrentItem] = xReel;
        //}

        // switch index to new item:
        //_iCurrentItem = (int)this.listReelConfiguration.SelectedItems[0].Tag;
        //this.lblMissingReels.Text = "Selected index: " + this.listReelConfiguration.SelectedItems[0].ToString();

        // load settings at new index:
        //LoadReelSettings();

        // check if the Next button can be enabled if all data is correct:
        bool bDataValid = true;
        foreach (PnPPartType xPartType in _xPartTypeList)
          if ((xPartType.Solution == PartPlaceSolution.Assigned && xPartType.GUID == Guid.Empty) || xPartType.Solution == PartPlaceSolution.Undefined)
            bDataValid = false;
        this.btnNext.Enabled = bDataValid;
      }
    }

    /// <summary>
    /// User ticked another Radio Button to change the configuration of this Reel.
    /// </summary>
    private void ChangeSolution(object sender, EventArgs e)
    {
      //this.xReelSettingsPicker.Enabled = false;
      //this.comReels.Enabled = false;
      this.comReels.Enabled = this.rdbUseExisting.Checked;

      //if (this.rdbUseExisting.Checked)
      //{
      //  this.comReels.Enabled = true;
      //}
      //else if (this.rdbExclude.Checked)
      //{
        // nothing needs to be enabled here
      //}
    }

    /// <summary>
    /// Verify that a proper solution for these Parts has been found.
    /// </summary>
    /// <returns>A value indicating whether the selected configuration is ok.</returns>
    //public bool CheckSolution()
    //{
    //  if (_iCurrentItem == -1)
    //    return false;

    //  bool bState = false;
    //  if (this.rdbReplacePart.Checked)
    //  {
    //    bState = (this.comReels.SelectedIndex > -1);
    //  }
    //  else if (this.rdbUseExisting.Checked)
    //  {
    //    bState = xReelSettingsPicker.VerifyInput();
    //  }
    //  else if (this.rdbExclude.Checked)
    //  {
    //    bState = true;
    //  }
    //  else
    //  {
    //    bState = false;
    //  }
    //  return bState;
    //}

    private void EditReelSettings(object sender, EventArgs e)
    {
      EditReelSettings xEditDialog = new EditReelSettings();
      //foreach (PnPReel xReel in _xReelLibrary)
        //if (xReel.GUID == _xPartTypeList[_iCurrentItem].GUID)
          //xEditDialog.Reel = xReel;
      if (this.comReels.SelectedIndex > -1)
      {
        xEditDialog.Reel = (PnPReel)this.comReels.SelectedItem;
        if (xEditDialog.ShowDialog() == DialogResult.OK)
        {
          for (int iCur = 0; iCur < _xReelLibrary.Count; iCur++)
          {
            if (_xReelLibrary[iCur].GUID == _xPartTypeList[_iCurrentItem].GUID)
            {
              _xReelLibrary[iCur] = xEditDialog.Reel;
              break;
            }
          }
        }
      }

    }

    private void AddNewReel(object sender, EventArgs e)
    {
      NewReelDialog xNewReelDialog = new NewReelDialog();
      xNewReelDialog.ReelCollection = _xReelLibrary;
      xNewReelDialog.PartCollection = _xPartsList;

      // automatically assign settings based on the specified data from BOM and PnP file
      PnPReel xNewReel = xNewReelDialog.AutoConfigure(_xPartTypeList[_iCurrentItem]);
      xNewReel.Footprint = _xPartTypeList[_iCurrentItem].Footprint;
      xNewReel.PartNameOrValue = _xPartTypeList[_iCurrentItem].PartNameOrValue;
      xNewReelDialog.Reel = xNewReel;

      if (xNewReelDialog.ShowDialog() == DialogResult.OK)
      {
        // add newly created reel to the library
        _xReelLibrary.Add(xNewReelDialog.Reel);

        // save reel library to disk
        PnPReel.SaveReels(_xReelLibrary);

        // add reel also to the combobox and focus it:
        this.comReels.Items.Add(xNewReelDialog.Reel);
        this.comReels.SelectedItem = xNewReelDialog.Reel;
        this.rdbUseExisting.Checked = true;
      }

      xNewReelDialog.Dispose();      
    }

    private void ReelSelectionChanged(object sender, EventArgs e)
    {
      // enable Edit button when a reel is selected
      this.btnEditReel.Enabled = (this.comReels.SelectedIndex != -1);
    }


  }

  /// <summary>
  /// Represents a specific Part type, the Parts in the project that match this type, and the Reel that supplies this Part.
  /// </summary>
  public struct PnPPartType
  {
    /// <summary>
    /// The designators of all parts that are of this type.
    /// </summary>
    public List<string> Designators;

    /// <summary>
    /// The footprint of this part type.
    /// </summary>
    public PnPFootprint Footprint;

    /// <summary>
    /// The part name, number, or its value.
    /// </summary>
    public string PartNameOrValue;

    /// <summary>
    /// An index for numeric enumeration.
    /// </summary>
    public int Index;

    /// <summary>
    /// A value indicating if the part will be placed as part of the project.
    /// </summary>
    public PartPlaceSolution Solution;

    /// <summary>
    /// The GUID of the Reel that is assigned to this PartInfo
    /// </summary>
    public Guid GUID;

    //public bool AutomaticConfig;

    //public Guid ReplacementReel;
    //public string ReplacementReelDescription;
    //public string FootprintString;
    //public string PartNumber;
    //public PnPSupplyPackage PackageType;
    //public float Height;
    //public float XOffset, YOffset;
    //public PnPMachineNozzle Nozzle;
    //public float FeedSpacing;
    //public int Quantity;
    //public string Manufacturer;
    //public string Supplier;
    //public string OrderCode;
    //public int Speed;
    //public int Rotation;

    //public float XSize, YSize; // experimental for automatic centring of nozzles for tray and tube parts   
  }

  public enum PartPlaceSolution
  {
    Assigned,
    Excluded,
    Undefined
  }

}
