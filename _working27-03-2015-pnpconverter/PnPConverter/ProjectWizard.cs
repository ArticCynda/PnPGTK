using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PnPConverter
{
  public partial class ProjectWizard : Form
  {
    public ProjectWizard()
    {
      InitializeComponent();
    }

    private enum WizardState
    {
      Intro,
      ImportPnPFile,
      ImportLibs,
      ReviewParts,
      AssignReels,
      Project,
      Panel,
      Phases,
      Finish
    }

    private void NewPCBWizard_Load(object sender, EventArgs e)
    {
      btnNext.Select();
    }

    private void NextStep(object sender, EventArgs e)
    {
      switch (_xState)
      { 
        case WizardState.Intro:
          _xState = WizardState.ImportPnPFile;
          break;
        case WizardState.ImportPnPFile:
          _xState = WizardState.ImportLibs;
          break;
        case WizardState.ImportLibs:
          _xState = WizardState.ReviewParts;
          break;
        case WizardState.ReviewParts:
          _xState = WizardState.AssignReels;
          break;
        case WizardState.AssignReels:
          _xState = WizardState.Project;
          break;
        case WizardState.Project:
          _xState = WizardState.Panel;
          break;
        case WizardState.Panel:
          _xState = WizardState.Phases;
          break;
        case WizardState.Phases:
          _xState = WizardState.Finish;
          break;
      }
      UpdatePanelLayout();
    }

    private void PreviousStep(object sender, EventArgs e)
    {
      switch (_xState)
      {
        case WizardState.Intro:
          _xState = WizardState.Intro;
          break;
        case WizardState.ImportPnPFile:
          _xState = WizardState.Intro;
          break;
        case WizardState.ImportLibs:
          _xState = WizardState.ImportPnPFile;
          break;
        case WizardState.ReviewParts:
          _xState = WizardState.ImportLibs;
          break;
        case WizardState.AssignReels:
          _xState = WizardState.ReviewParts;
          break;
        case WizardState.Project:
          _xState = WizardState.AssignReels;
          break;
        case WizardState.Panel:
          _xState = WizardState.Project;
          break;
        case WizardState.Phases:
          _xState = WizardState.Panel;
          break;
      }
      UpdatePanelLayout();
    }

    private void UpdatePanelLayout()
    {
      // update the user controls based on the state the form is in:
      switch (_xState)
      {
        case WizardState.Intro:
          this.btnBack.Enabled = false;
          this.pnlSelectPnP.Visible = false;
          this.pnlIntro.Visible = true;
          break;
        case WizardState.ImportPnPFile:
          this.btnBack.Enabled = true;
          this.pnlIntro.Visible = false;
          this.pnlSelectLib.Visible = false;
          this.pnlSelectPnP.Visible = true;
          break;
        case WizardState.ImportLibs:
          this.pnlReviewParts.Visible = false;
          this.pnlSelectPnP.Visible = false;
          this.pnlSelectLib.Visible = true;
          this.ResumeLayout(true);
          LoadPcbLibs();
          break;
        case WizardState.ReviewParts:
          this.pnlSelectLib.Visible = false;
          this.pnlAddReels.Visible = false;
          this.pnlReviewParts.Visible = true;
          this.btnNext.Enabled = true;
          this.ResumeLayout(true);
          this.Cursor = Cursors.WaitCursor;
          SavePcbLibs();
          CollectFootprints();
          AssembleSummary();
          UpdatePartSummary();
          this.Cursor = Cursors.Default;
          break;
        case WizardState.AssignReels:
          //this.btnNext.Text = "&Next";
          this.pnlProjectProperties.Visible = false;
          this.pnlReviewParts.Visible = false;
          this.pnlAddReels.Visible = true;
          this.Cursor = Cursors.WaitCursor;
          LoadReelCollection();
          IdentifyMissingReels();
          LoadReelLibraryList();
          LoadReelList();
          this.Cursor = Cursors.Default;
          break;
        case WizardState.Project:
          this.pnlAddReels.Visible = false;
          this.pnlPanel.Visible = false;
          this.pnlProjectProperties.Visible = true;
          this.Cursor = Cursors.WaitCursor;
          SaveNewReels();
          LoadProjectName();
          this.Cursor = Cursors.Default;
          break;
        case WizardState.Panel:
          this.btnNext.Text = "&Next";
          this.pnlProjectProperties.Visible = false;
          this.pnlPhases.Visible = false;
          this.pnlPanel.Visible = true;
          InitPanel();
          SavePanelConfiguration();
          break;
        case WizardState.Phases:
          this.btnNext.Text = "E&xport";
          this.pnlProjectProperties.Visible = false;
          this.pnlPhases.Visible = true;
          this.Cursor = Cursors.WaitCursor;
          ProjectInit();
          GeneratePhases();
          DisplayPhases();
          this.Cursor = Cursors.Default;
          break;
        case WizardState.Finish:
          UpdateStackConfiguration();
          ExportFiles();
          MessageBox.Show("wizard finished!");
          break;
      }

    }

    private void LoadPcbLibs()
    {
      // first load the existing libraries into the list view
      //ConfigManager.ConfigValues xSmdFootprints = ConfigManager.ConfigReader();
      this.listLibraries.Clear();

      List<FileInfo> xSmdLibs = ConfigManager.GetSmdLibraries();
      foreach (FileInfo xSmdLib in xSmdLibs)
        this.listLibraries.Items.Add(xSmdLib.FullName);

      UpdatePcbLibs();
    }

    private void SavePcbLibs()
    {
      // save the selected libraries to disk
      //ConfigManager.ConfigValues xSmdFootprints;
      //xSmdFootprints.SmdLibs = new List<FileInfo>();
      List<FileInfo> xSmdLibs = new List<FileInfo>();
      foreach (ListViewItem xLib in this.listLibraries.Items)
      {
        FileInfo xCurLib = new FileInfo(xLib.Text);
        xSmdLibs.Add(xCurLib);
      }

      
      //ConfigManager.ConfigWriter(xSmdFootprints);
      ConfigManager.SetSmdLibraries(xSmdLibs);
      //MessageBox.Show("configuration successfully stored on disk!");

    }

    private void UpdatePcbLibs()
    {
      //MessageBox.Show("updating pcblibs!");
      this.lblFootprintCount.Text = "Searching for footprints...";
      this.Cursor = Cursors.WaitCursor;
      int iFootprintCount = 0;
      foreach (ListViewItem xPcbLibrary in this.listLibraries.Items)
      {
        if (xPcbLibrary.Tag == null)
        {
          List<PnPFootprint> xCurFootprints = ADLibReader.GetFootprints(new FileInfo(xPcbLibrary.Text));
          xPcbLibrary.Tag = xCurFootprints.Count;
          iFootprintCount += xCurFootprints.Count;
        }
        else
        {
          int iFootprints = (int)(xPcbLibrary.Tag);
          iFootprintCount += iFootprints;
        }
      }
      if (iFootprintCount == 0)
      {
        this.lblFootprintCount.Text = "Add library with SMD footprints used in this project.";
      }
      else
      {
        this.lblFootprintCount.Text = "A total of " + iFootprintCount.ToString();
        if (iFootprintCount == 1)
          this.lblFootprintCount.Text += " valid footprint was found in " + this.listLibraries.Items.Count.ToString();
        else
          this.lblFootprintCount.Text += " valid footprints were found in " + this.listLibraries.Items.Count.ToString();
        
        if (this.listLibraries.Items.Count == 1)
          this.lblFootprintCount.Text += " library.";
        else
          this.lblFootprintCount.Text += " libraries.";
      }
      //this.lblFootprintCount.Text = "A total of " + iFootprintCount.ToString() + " valid footprints was detected in " + this.listLibraries.Items.Count.ToString() + " libraries.";
      this.Cursor = Cursors.Default;
    }

    private void AddPcbLib(object sender, EventArgs e)
    {
      OpenFileDialog xBrowseLib = new OpenFileDialog();
      xBrowseLib.Filter = "Altium Designer PCB Library|*.PcbLib";
      xBrowseLib.Title = "Select Altium Designer PCB Library...";
      xBrowseLib.FileName = "D:\\Dropbox\\PCBtech\\libs\\mylibrary.PcbLib";
      if (xBrowseLib.ShowDialog() == DialogResult.OK)
      {
        this.lblFootprintCount.Text = "Checking file integrity...";
        FileInfo xLibFile = new FileInfo(xBrowseLib.FileName);
        if (ADLibReader.IsADLib(xLibFile))
        {
          this.picLibOK.Visible = true;
          this.picLibNOK.Visible = false;
          this.lblADLib.Text = "Valid Altium library selected.";
          bool bExists = false;
          foreach (ListViewItem xListItem in listLibraries.Items)
          {
            if (xListItem.Text.Equals(xLibFile.FullName))
              bExists = true;
          }
          
          if (bExists)
          {
            // TODO: messagebox customization
            MessageBox.Show("The selected library is already included.");
          }
          else
          {
            this.listLibraries.Items.Add(xLibFile.FullName);
            UpdatePcbLibs();
          }
        }
        else
        {
          this.picLibOK.Visible = false;
          this.picLibNOK.Visible = true;
          this.lblADLib.Text = "Not a valid Altium library.";
          this.lblFootprintCount.Text = "Integrity check completed.";
        }
        //this.lblFootprintCount.Text = "Integrity check completed.";
      }
    }

    private void PurgePcbLibs(object sender, EventArgs e)
    {
      bool bPurged = false;
      foreach (ListViewItem xCurLib in this.listLibraries.Items)
        if (!File.Exists(xCurLib.Text))
        {
          this.listLibraries.Items.Remove(xCurLib);
          bPurged = true;
        }
      if (!bPurged)
        MessageBox.Show("all libraries valid!");
      else
        UpdatePcbLibs();
    }

    private void RemovePcbLib(object sender, EventArgs e)
    {
      foreach (ListViewItem xCurLib in this.listLibraries.SelectedItems)
        this.listLibraries.Items.Remove(xCurLib);

      UpdatePcbLibs();
    }


    private void CollectFootprints()
    {
      _xSmdFootprints.Clear();
      foreach (ListViewItem xCurLib in this.listLibraries.Items)
      {
        FileInfo xCurLibFile = new FileInfo(xCurLib.Text);
        if (ADLibReader.IsADLib(xCurLibFile))
        {
          List<PnPFootprint> xFootprints = ADLibReader.GetFootprints(xCurLibFile);
          foreach (PnPFootprint xCurFootprint in xFootprints)
            _xSmdFootprints.Add(xCurFootprint);
        }
      }
    }


    private void AssembleSummary()
    {
      _xPartsList.Clear();
      _xExcludedPartsList.Clear();

      // sort out any parts that do not have known SMD footprints
      foreach (PnPPart xCurPart in _xPnPFilePartsList)
      {
        //PnPPart xNewPart = new PnPPart(
        //  xCurPart.Designator,
        //  xCurPart.Footprint,
        //  xCurPart.Comment,
        //  xCurPart.Layer,
        //  xCurPart.Coordinates,
        //  xCurPart.Rotation,
        //  xCurPart.SkipPlacement
        //);

        bool bFootprintFound = false;
        foreach (PnPFootprint xCurFootprint in _xSmdFootprints)
        {
          if (xCurFootprint.Name.ToLower() == xCurPart.Footprint.Name.ToLower())
          {
            bFootprintFound = true;
            // copy relevant parameters and add the part to the updated list
            xCurPart.Footprint = xCurFootprint;

            // skip footprints with height zero, which are logos, mounting holes, fiducials etc.
            if (xCurFootprint.Height > 0)
            {
              if (!xCurFootprint.Name.ToLower().StartsWith("mech"))
              {
                xCurPart.ExclusionReason = ExclusionReason.None;
                _xPartsList.Add(xCurPart);
              }
              else
              {
                xCurPart.ExclusionReason = ExclusionReason.Mechanical;
                _xExcludedPartsList.Add(xCurPart);
              }
            }
            else
            {
              xCurPart.ExclusionReason = ExclusionReason.NotPhysical;
              _xExcludedPartsList.Add(xCurPart);
            }
          }
        }
        if (!bFootprintFound)
        {
          if (xCurPart.Footprint.Name.ToLower().StartsWith("mech"))
            xCurPart.ExclusionReason = ExclusionReason.Mechanical;
          else
            xCurPart.ExclusionReason = ExclusionReason.NotSmd;
          _xExcludedPartsList.Add(xCurPart);
        }
      }
    }

    private void UpdatePartSummary()
    {
      this.listPartSummary.Items.Clear();

      foreach (PnPPart xCurPart in _xPartsList)
      {
        string[] sProperties = new string[5];
        sProperties[0] = xCurPart.Designator;
        sProperties[1] = xCurPart.Footprint.Name;
        sProperties[2] = xCurPart.Footprint.Description;
        sProperties[3] = xCurPart.PartNumberOrValue;
        sProperties[4] = xCurPart.Footprint.Height.ToString() + " mm";
        ListViewItem xCurPartEntry = new ListViewItem(sProperties);
        this.listPartSummary.Items.Add(xCurPartEntry);
      }

      lnkOmittedParts.Text = "Review " + (_xPnPFilePartsList.Count - _xPartsList.Count).ToString() + " omitted parts";
    }

    private void ReviewOmittedParts(object sender, LinkLabelLinkClickedEventArgs e)
    {
      ExcludedPartsDialog xOmittedPartsDialog = new ExcludedPartsDialog();
      xOmittedPartsDialog.IncludedParts = _xPartsList;
      xOmittedPartsDialog.ExcludedParts = _xExcludedPartsList;
      if (xOmittedPartsDialog.ShowDialog() == DialogResult.OK)
      {
        // if the user clicked OK, update the lists
        _xPartsList = xOmittedPartsDialog.IncludedParts;
        _xExcludedPartsList = xOmittedPartsDialog.ExcludedParts;
        UpdatePartSummary();
      }
    }

    private void OmitSelectedParts(object sender, EventArgs e)
    {
      bool bOmitted = false;
      foreach (ListViewItem xCurPartEntry in this.listPartSummary.Items)
      {
        if (xCurPartEntry.Checked)
        {
          bOmitted = true;
          foreach (PnPPart xCurPart in _xPartsList)
          {
            if (xCurPart.Designator.ToUpper() == xCurPartEntry.SubItems[0].Text.ToUpper())
            {
              _xPartsList.Remove(xCurPart);
              xCurPart.ExclusionReason = ExclusionReason.UserDefined;
              _xExcludedPartsList.Add(xCurPart);
              break;
            }
          }
        }
      }
      if (bOmitted)
        UpdatePartSummary();
    }

    private void EnableOmitSelectedButton(object sender, ItemCheckedEventArgs e)
    {
      bool bCheckedParts = false;
      foreach (ListViewItem xCurPartEntry in this.listPartSummary.Items)
        if (xCurPartEntry.Checked)
          bCheckedParts = true;
 
      this.btnOmitSelected.Enabled = bCheckedParts;
    }

    private void EnableRemoveLibButton(object sender, EventArgs e)
    {
      this.btnRemoveLib.Enabled = (this.listLibraries.SelectedItems.Count > 0);
    }


   

    private string _sProjectName = string.Empty;
    private int _PCBCount = -1;

    private void LoadProjectName()
    {
      if (_sProjectName == string.Empty)
      {
        const string FN_LEAD = "Pick Place for ";
        if (_xPnPFile.Name.StartsWith(FN_LEAD))
        {
          _sProjectName = _xPnPFile.Name.Substring(FN_LEAD.Length);
          _sProjectName = _sProjectName.Substring(0, _sProjectName.Length - _xPnPFile.Extension.Length);
        }
        else
        {
          _sProjectName = _xPnPFile.Name;
        }
      }
      this.txtProjectName.Text = _sProjectName;

      if (_PCBCount == -1)
        _PCBCount = 1;
      this.numPCBCount.Value = _PCBCount;
    }

    private void SaveNewReels()
    {
      // if the user has provided new reel data, then add these reels to the library:
      bool bNewReelsAdded = false;
      foreach (ReelInfo xNewReel in _xReelList)
      {
        if (xNewReel.Solution == MissingReelSolution.NewReel)
        {
          PnPReel xReel = new PnPReel();
          // copy relevant properties
          xReel.GUID = xNewReel.GUID;
          xReel.PartNumberOrValue = xNewReel.PartNameOrValue;
          xReel.Manufacturer = xNewReel.Manufacturer;
          xReel.Supplier = xNewReel.Supplier;
          xReel.OrderCode = xNewReel.OrderCode;
          xReel.SupplyPackage = xNewReel.PackageType;
          xReel.Footprint = xNewReel.Footprint;
          xReel.FeedSpacing = xNewReel.FeedSpacing;
          xReel.Nozzle = xNewReel.Nozzle;
          xReel.XOffset = xNewReel.XOffset;
          xReel.YOffset = xNewReel.YOffset;
          xReel.Speed = xNewReel.Speed;
          xReel.Rotation = xNewReel.Rotation;

          // add this reel to the collection:
          _xReelLibrary.Add(xReel);
          bNewReelsAdded = true;

          // experimental
          // assign the newly created Reel to the associated Parts
          for (int iCur = 0; iCur < _xPartsList.Count; iCur++)
          {
            PnPPart xCurPart = _xPartsList[iCur];
            if (xNewReel.Designators.Contains(xCurPart.Designator))
              xCurPart.AssignedReel = xNewReel.GUID;
          }
        }
        // experimental
        else if (xNewReel.Solution == MissingReelSolution.StandIn)
        {
          // assign the selected Reel to replace the actual one to the associated Parts
          for (int iCur = 0; iCur < _xPartsList.Count; iCur++)
          {
            PnPPart xCurPart = _xPartsList[iCur];
            if (xNewReel.Designators.Contains(xCurPart.Designator))
              xCurPart.AssignedReel = xNewReel.ReplacementReel;
          }
        }
      }

      // save the library to disk:
      if (bNewReelsAdded)
        PnPReel.SaveReels(_xReelLibrary);
    }



    private void ProjectInit()
    {
      FileInfo xLastProjectFile = ConfigManager.GetLastProject();
      _xProject = PnPProject.LoadConfiguration(xLastProjectFile);
    }

    private void GeneratePhases()
    {
      // erase existing data of active reels and stacks:
      _xActiveReels.Clear();
      _xActiveStacks.Clear();
      
      // assemble a list of Reels required in this project:
      foreach (PnPPart xCurPart in _xPartsList)
      {
        // if the user selected a Part to be placed manually, it will not have a Reel assigned to it
        if (xCurPart.AssignedReel != Guid.Empty)
        {
          // search the matching Reel for this Part
          PnPReel xReel = null;
          foreach (PnPReel xMatchReel in _xReelLibrary)
            if (xMatchReel.GUID == xCurPart.AssignedReel)
            {
              xReel = xMatchReel;
              break;
            }

          // check if no such Reel has been added yet to the list of active Reels:
          if (_xActiveReels.IndexOf(xReel) == -1)
          {
            // add Reel to the list of active Reels:
            xReel.StackAssigned = false;
            _xActiveReels.Add(xReel);
          }
        }
      }

      // initialize stack list with last loaded stacks:
      _xActiveStacks = _xProject.Stacks; 

      // first identify the Reels that are currently loaded into the Machine and can be used, and assign them to Phase 1:
      for (int iCur = 0; iCur < _xActiveReels.Count; iCur++)
      {
        PnPReel xCurReel = _xActiveReels[iCur];
        foreach (PnPStack xCurStack in _xMachine.GetStacks(PnPStackType.Undefined))
        {
          if (xCurStack.Reel.Equals(xCurReel))
          {
            PnPStack xNewStack = new PnPStack(xCurStack.Location, xCurStack.MaxHeight);
            xNewStack.Phase = 1;
            _xActiveStacks.Add(xNewStack); 
            xCurReel.StackAssigned = true;
          }
        }
      }

      // also identify the locked Stacks and add them to the Stack list to prevent them from being reused
      foreach (PnPStack xCurStack in _xMachine.GetStacks(PnPStackType.Undefined))
        if (xCurStack.Locked)
        {
          PnPStack xLockedStack = xCurStack;
          xLockedStack.Phase = 1;
          _xActiveStacks.Add(xLockedStack);
        }

      // count the number of Reels that do not yet have a Stack assigned:
      int iStacklessReels = 0;
      foreach (PnPReel xCurReel in _xActiveReels)
        if (!xCurReel.StackAssigned)      
          iStacklessReels++;

      int iPhase = 1; // the current phase to fill up
      // loop through the Reels in the active project until all have a Stack assigned to them:
      while (iStacklessReels > 0)
      {
        
        // start by filling up 8 mm stacks:
        int iStacks8mm = _xMachine.GetStacks(PnPStackType.Reel8mm).Count;
        StackLocation xLoc = new StackLocation();
        xLoc.StackType = PnPStackType.Reel8mm;
        for (int i = 1; i <= iStacks8mm; i++)
        {
          // search in the stack list if the stack with the current Position is still available in the current Phase:
          bool bAvailable = true;
          xLoc.Position = i;
          foreach (PnPStack xCurStack in _xActiveStacks)
          {
            // break out of the search if the Stack on the current Location has been loaded in the current Phase:
            if (xCurStack.Phase == iPhase && xCurStack.Location.Equals(xLoc))
            {
              bAvailable = false;
              break;
            }

            // also break out of the search if the Stack on the current Location is marked as Locked, regardless of Phase:
            if (xCurStack.Phase == 1 && xCurStack.Location.Equals(xLoc) && xCurStack.Locked)
            {
              bAvailable = false;
              break;
            }
          }

          // DEBUG
          // testing pointed out that the first 8 mm stack of the machine does not appear to be automatically advanced.
          // since it is unclear if this is a bug in this software, a firmware bug or an undocumented feature, exclude the first 8 mm reel for now:
          //StackLocation xPrimeLoc = new StackLocation();
          //xPrimeLoc.StackType = PnPStackType.Reel8mm;
          //xPrimeLoc.Position = 1;
          if (i == 1)
          {
            bAvailable = false;
          }

          // if this Stack is available, assign a matching Reel in the Reel list to it:
          if (bAvailable)
          {
            // search a Reel that can be loaded in this Stack:
            //bool bReelFound = false;
            for (int iCur = 0; iCur < _xActiveReels.Count; iCur++)
            {
              PnPReel xCurReel = _xActiveReels[iCur];
              if (xCurReel.SupplyPackage == PnPSupplyPackage.Reel8mm && !xCurReel.StackAssigned && _xMachine.GetStack(xLoc).MaxHeight >= xCurReel.Footprint.Height)
              {
                xCurReel.StackAssigned = true;
                //_xActiveReels[iCur].StackAssigned = true;
                PnPStack xNewStack = new PnPStack(xLoc, _xMachine.GetStack(xLoc).MaxHeight);
                //xNewStack.Location = xLoc;
                xNewStack.LoadReel(xCurReel);
                xNewStack.Phase = iPhase;
                _xActiveStacks.Add(xNewStack);
                _xActiveReels[iCur] = xCurReel;
                //bReelFound = true;
                break;
              }
  
            // if no more reels need to be 
            //if (!bReelFound)
              //break;
            }
          }
        }


        // then fill up 12 mm stacks:
        int iStacks12mm = _xMachine.GetStacks(PnPStackType.Reel12mm).Count;
        xLoc = new StackLocation();
        xLoc.StackType = PnPStackType.Reel12mm;
        for (int i = 1; i <= iStacks12mm; i++)
        {
          // search in the stack list if the stack with the current Position is still available in the current Phase:
          bool bAvailable = true;
          xLoc.Position = i;
          foreach (PnPStack xCurStack in _xActiveStacks)
          {
            if (xCurStack.Phase == iPhase && xCurStack.Location.Equals(xLoc))
            {
              bAvailable = false;
              break;
            }

            // also break out of the search if the Stack on the current Location is marked as Locked, regardless of Phase:
            if (xCurStack.Phase == 1 && xCurStack.Location.Equals(xLoc) && xCurStack.Locked)
            {
              bAvailable = false;
              break;
            }
          }

          // if this Stack is available, assign a matching Reel in the Reel list to it:
          if (bAvailable)
          {
            // search a Reel that can be loaded in this Stack:
            //bool bReelFound = false;
            for (int iCur = 0; iCur < _xActiveReels.Count; iCur++)
            {
              PnPReel xCurReel = _xActiveReels[iCur];
              if (xCurReel.SupplyPackage == PnPSupplyPackage.Reel12mm && !xCurReel.StackAssigned && _xMachine.GetStack(xLoc).MaxHeight >= xCurReel.Footprint.Height)
              {
                xCurReel.StackAssigned = true;
                //_xActiveReels[iCur].StackAssigned = true;
                PnPStack xNewStack = new PnPStack(xLoc, _xMachine.GetStack(xLoc).MaxHeight);
                //xNewStack.Location = xLoc;
                xNewStack.LoadReel(xCurReel);
                xNewStack.Phase = iPhase;
                _xActiveStacks.Add(xNewStack);
                _xActiveReels[iCur] = xCurReel;
                //bReelFound = true;
                break;
              }

              // if no more reels need to be 
              //if (!bReelFound)
              //  break;
            }
          }
        }

        // then fill up 16 mm stacks:
        int iStacks16mm = _xMachine.GetStacks(PnPStackType.Reel16mm).Count;
        xLoc = new StackLocation();
        xLoc.StackType = PnPStackType.Reel16mm;
        for (int i = 1; i <= iStacks16mm; i++)
        {
          // search in the stack list if the stack with the current Position is still available in the current Phase:
          bool bAvailable = true;
          xLoc.Position = i;
          foreach (PnPStack xCurStack in _xActiveStacks)
          {
            if (xCurStack.Phase == iPhase && xCurStack.Location.Equals(xLoc))
            {
              bAvailable = false;
              break;
            }

            // also break out of the search if the Stack on the current Location is marked as Locked, regardless of Phase:
            if (xCurStack.Phase == 1 && xCurStack.Location.Equals(xLoc) && xCurStack.Locked)
            {
              bAvailable = false;
              break;
            }
          }

          // if this Stack is available, assign a matching Reel in the Reel list to it:
          if (bAvailable)
          {
            // search a Reel that can be loaded in this Stack:
            //bool bReelFound = false;
            for (int iCur = 0; iCur < _xActiveReels.Count; iCur++)
            {
              PnPReel xCurReel = _xActiveReels[iCur];
              if (xCurReel.SupplyPackage == PnPSupplyPackage.Reel16mm && !xCurReel.StackAssigned && _xMachine.GetStack(xLoc).MaxHeight >= xCurReel.Footprint.Height)
              {
                xCurReel.StackAssigned = true;
                //_xActiveReels[iCur].StackAssigned = true;
                PnPStack xNewStack = new PnPStack(xLoc, _xMachine.GetStack(xLoc).MaxHeight);
                //xNewStack.Location = xLoc;
                xNewStack.LoadReel(xCurReel);
                xNewStack.Phase = iPhase;
                _xActiveStacks.Add(xNewStack);
                _xActiveReels[iCur] = xCurReel;
                //bReelFound = true;
                break;
              }

              // if no more reels need to be 
              //if (!bReelFound)
              //  break;
            }
          }
        }

        // finally fill up 16 mm trays:
        int iTrays16mm = _xMachine.GetStacks(PnPStackType.Tray16mm).Count;
        xLoc = new StackLocation();
        xLoc.StackType = PnPStackType.Tray16mm;
        for (int i = 1; i <= iTrays16mm; i++)
        {
          // search in the stack list if the stack with the current Position is still available in the current Phase:
          bool bAvailable = true;
          xLoc.Position = i;
          foreach (PnPStack xCurStack in _xActiveStacks)
          {
            if (xCurStack.Phase == iPhase && xCurStack.Location.Equals(xLoc))
            {
              bAvailable = false;
              break;
            }

            // also break out of the search if the Stack on the current Location is marked as Locked, regardless of Phase:
            if (xCurStack.Phase == 1 && xCurStack.Location.Equals(xLoc) && xCurStack.Locked)
            {
              bAvailable = false;
              break;
            }
          }

          // if this Stack is available, assign a matching Reel in the Reel list to it:
          if (bAvailable)
          {
            // search a Reel that can be loaded in this Stack:
            //bool bReelFound = false;
            for (int iCur = 0; iCur < _xActiveReels.Count; iCur++)
            {
              PnPReel xCurReel = _xActiveReels[iCur];
              if ((xCurReel.SupplyPackage == PnPSupplyPackage.Tray || xCurReel.SupplyPackage == PnPSupplyPackage.Tube) && !xCurReel.StackAssigned)
              {
                //xCurReel.StackAssigned = true;
                //_xActiveReels[iCur].StackAssigned = true;
                PnPStack xNewStack = new PnPStack(xLoc, _xMachine.GetStack(xLoc).MaxHeight);
                //xNewStack.Location = xLoc;
                xNewStack.LoadReel(xCurReel);
                xNewStack.Phase = iPhase;
                _xActiveStacks.Add(xNewStack);
                xCurReel.StackAssigned = true;
                _xActiveReels[iCur] = xCurReel;
                //bReelFound = true;
                break;
              }

              // if no more reels need to be 
              //if (!bReelFound)
              //  break;
            }
          }
        }

        // (re)count the number of Reels that do not yet have a Stack assigned:
        iStacklessReels = 0;
        foreach (PnPReel xCurReel in _xActiveReels)
          if (!xCurReel.StackAssigned)
            iStacklessReels++;

        // add a new phase if not all Reels could be placed
        iPhase++;
      }

      // initially start displaying phase 1:
      _iCurrentPhase = 1;
      UpdatePhaseButtons();

      // save this configuration:
      _xProject.SaveConfiguration(new FileInfo(_sProjectName + ".pnp"));
      SaveCurrentPhaseStacks();
      

    }

    /// <summary>
    /// Counts the number of phases in this project.
    /// </summary>
    /// <returns>The current number of phases.</returns>
    private int Phases()
    {
      // first determine how many different Phases this project requires:
      int iPhaseCount = 1;
      foreach (PnPStack xPhaseStack in _xActiveStacks)
        if (xPhaseStack.Phase > iPhaseCount)
          iPhaseCount = xPhaseStack.Phase;

      return iPhaseCount;
    }


    private void SaveCurrentPhaseStacks()
    {
      // first assemble a list of all the active stacks in the current phase:
      List<PnPStack> xStackList = new List<PnPStack>();
      foreach (PnPStack xCurStack in _xActiveStacks)
        if (xCurStack.Phase == _iCurrentPhase)
          xStackList.Add(xCurStack);

      ConfigManager.SetLoadedStacks(xStackList);
    }

    /// <summary>
    /// Configures the Tab control on the Phase panel to match the different Phases, and loads the Reels.
    /// </summary>
    private void DisplayPhases()
    {
      // first determine how many different Phases this project requires:
      int iPhaseCount = Phases();

      // now add the necessary tabs to the Tab control:
      this.tabPhases.TabPages.Clear();
      for (int iPhase = 1; iPhase <= iPhaseCount; iPhase++)
      {
        TabPage xNewPage = new TabPage("Phase " + iPhase.ToString());
        xNewPage.Name = iPhase.ToString();
        xNewPage.AutoScroll = true;
        this.tabPhases.TabPages.Add(xNewPage);
      }

      // select the active phase:
      for (int iPhase = 1; iPhase <= iPhaseCount; iPhase++)
      {
        TabPage xCurPage = this.tabPhases.TabPages[iPhase - 1];
        if (xCurPage.Name.ToString() == iPhase.ToString())
        {
          this.tabPhases.SelectedTab = xCurPage;
          break;
        }
      }

      //StackType[] xStackTypes = new StackType[] {StackType.Reel8mm, StackType.Reel12mm, StackType.Reel16mm, StackType.Tray16mm};
      //foreach (StackType xCurType in _xMachine.GetStackTypes())
      //{
      //  MessageBox.Show(xCurType.ToString());

      //}


      foreach (TabPage xCurPage in this.tabPhases.TabPages)
      {
        // clean up all controls on this tab page and clear the list:
        foreach (Control xControl in xCurPage.Controls)
          xControl.Dispose();
        xCurPage.Controls.Clear();

        int iRowCounter = 0;
        foreach (PnPStackType xCurType in _xMachine.GetStackTypes())
        {
          int iRows = _xMachine.GetStacks(xCurType).Count;
          
          for (int i = 1; i <= iRows; i++)
          {
            // create a checkbox to lock or unlock the Stack:
            StackLoader xStackLoader = new StackLoader();
            xStackLoader.Name = "stack" + i.ToString();
            xStackLoader.StackPosition = i;
            xStackLoader.Type = xCurType;
            xStackLoader.Location = new Point(10, 5 + iRowCounter * 28);
            xStackLoader.LockedStateChanged += new StackLoader.LockedStateChangedEventHandler(LockedReel);
            xStackLoader.SelectedReelChanged += new StackLoader.SelectedReelChangedEventHandler(ReelChanged);
            xStackLoader.Machine = _xMachine;     // first assign a machine before adding reels!       
            xStackLoader.Reels = _xReelLibrary;
            xStackLoader.ActiveReels = _xActiveReels;
            xStackLoader.Clear(); //removes any selection
            xCurPage.Controls.Add(xStackLoader);
            
            iRowCounter++;
          }
        }
      }

      //MessageBox.Show("pause");

      // fill every tab with the necessary controls:
      //foreach (TabPage xCurPage in this.tabPhases.TabPages)
      //{
      //  //PnPMachine xMachine = new TM220A();
      //  int iRows = _xMachine.GetStacks(StackType.Undefined).Count; // retrieve the number of stacks for this machine
        
      //  // clean up all controls on this tab page and clear the list:
      //  foreach (Control xControl in xCurPage.Controls)
      //    xControl.Dispose();
      //  xCurPage.Controls.Clear();

      //  for (int i = 1; i <= iRows; i++)
      //  {
      //    // create a checkbox to lock or unlock the Stack:
      //    StackLoader xStackLoader = new StackLoader();
      //    xStackLoader.Name = "stack" + i.ToString();
      //    //xStackLoader.Tag = i;

      //    xStackLoader.Location = new Point(10, 5 + i * 28);
      //    xStackLoader.LockedStateChanged += new StackLoader.LockedStateChangedEventHandler(LockedReel);
      //    xStackLoader.Reels = _xReelLibrary;
      //    xStackLoader.ActiveReels = _xActiveReels;
      //    xStackLoader.Clear(); //removes any selection
      //    xCurPage.Controls.Add(xStackLoader);
      //  }
      //}

      // load Reels in the Stacks
      foreach (PnPStack xActiveStack in _xActiveStacks)
      {
        // search the tab page for this phase:
        TabPage xPage = new TabPage();
        foreach (TabPage xCurPage in this.tabPhases.TabPages)
          if (xCurPage.Name == xActiveStack.Phase.ToString())
          {
            xPage = xCurPage;
            break;
          }

        // configure the Stack to the correct StackLoader on this page:
        foreach (Control xLoader in xPage.Controls)
        {
          StackLoader xStackLoader = (StackLoader)xLoader;

          if (xStackLoader.StackPosition == xActiveStack.Location.Position && xStackLoader.Type == xActiveStack.Location.StackType)
          {
            xStackLoader.Stack = xActiveStack;
          }
          
          
          // calculate the index of this Stack in the machine's Stack list:
          //int iLoaderIndex = xActiveStack.Location.Position;
          //switch (xActiveStack.Location.StackType)
          //{
          //  case StackType.Reel8mm:
          //    if ((int)xStackLoader.Tag == iLoaderIndex)
          //      xStackLoader.Stack = xActiveStack;
          //    else
          //    {
          //      StackLocation xLoc = new StackLocation();
          //      xLoc.StackType = StackType.Reel8mm;
          //      xLoc.Position = (int)xStackLoader.Tag;
          //      xStackLoader.Stack = new PnPStack(xLoc, 5); // verify 5  
          //      xStackLoader.Clear();
          //    }
          //    break;               
          //  //  break;
          //  case StackType.Reel12mm:
          //    if ((int)xStackLoader.Tag + _xMachine.GetStacks(StackType.Reel8mm).Count == iLoaderIndex)
          //      xStackLoader.Stack = xActiveStack;
          //    //iLoaderIndex += _xMachine.GetStacks(StackType.Reel8mm);
          //    break;
          //  case StackType.Reel16mm:
          //    if ((int)xStackLoader.Tag + _xMachine.GetStacks(StackType.Reel8mm).Count + _xMachine.GetStacks(StackType.Reel12mm).Count == iLoaderIndex)
          //      xStackLoader.Stack = xActiveStack;
          //    //iLoaderIndex += _xMachine.GetStacks(StackType.Reel8mm) + _xMachine.GetStacks(StackType.Reel12mm);
          //    break;
          //  case StackType.Tray16mm:
          //    if ((int)xStackLoader.Tag + _xMachine.GetStacks(StackType.Reel8mm).Count + _xMachine.GetStacks(StackType.Reel12mm).Count + _xMachine.GetStacks(StackType.Reel16mm).Count == iLoaderIndex)
          //      xStackLoader.Stack = xActiveStack;
          //    //iLoaderIndex += _xMachine.GetStacks(StackType.Reel8mm) + _xMachine.GetStacks(StackType.Reel12mm) + _xMachine.GetStacks(StackType.Reel16mm);
          //    break;
          //}
          
        }

      }

    }

    private void LockedReel(object sender, EventArgs e)
    {
      //MessageBox.Show("locked state changed!");
      
      // lock or unlock this row in all other tabs, too
      StackLoader xLoader = (StackLoader)sender;
      foreach (TabPage xCurTab in this.tabPhases.TabPages)
      {
        foreach (Control xCurCtrl in xCurTab.Controls)
        {
          StackLoader xCurLoader = (StackLoader)xCurCtrl;
          // lock or unlock only stacks on other tab pages:
          if (!xCurLoader.Equals(xLoader))
          {
            if (xCurLoader.StackPosition == xLoader.StackPosition && xCurLoader.Type == xLoader.Type)
              xCurLoader.Locked = xLoader.Locked;
          }
        }
      }
    }

    private void ReelChanged(object sender, EventArgs e)
    {
      CheckReelConfiguration();
      //MessageBox.Show("fired!");
      this.lblUnassignedReels.Refresh();
    }

    private int CheckReelConfiguration()
    {
      // to verify the configuration, check that every reel has been placed in a slot:
      int iReelsFound = 0;

      foreach (TabPage xCurPage in this.tabPhases.TabPages)
      {
        foreach (PnPReel xReel in _xActiveReels)
        {
          bool bReelFound = false;
          foreach (Control xCurControl in xCurPage.Controls)
          {
            StackLoader xCurLoader = (StackLoader)xCurControl;
            if (!xCurLoader.IsEmpty)
            {
              if (xReel.Equals(xCurLoader.SelectedReel))
              {
                bReelFound = true;
                break;
              }
            }
          }
          if (bReelFound)
            iReelsFound++;
        }
      }

      int iUnassigned = _xActiveReels.Count - iReelsFound;
      this.lblUnassignedReels.Text = "Unassigned reels: " + iUnassigned.ToString();
      return iUnassigned;
    }

    private void CheckReelConfigurationButton(object sender, EventArgs e)
    {
      CheckReelConfiguration();      
    }

    /// <summary>
    /// Updates _xActiveStacks with the configuration that the user selected in the GUI
    /// </summary>
    private void UpdateStackConfiguration()
    {
      //List<PnPReel> xTempReels = new List<PnPReel>();
      foreach (TabPage xCurPage in this.tabPhases.TabPages)
      { 
        foreach (Control xCurLoader in xCurPage.Controls)
        {
          StackLoader xLoader = (StackLoader)xCurLoader;
          
          if (xLoader.IsEmpty)
          {
            PnPStack xMatchStack = null;
            foreach (PnPStack xCurStack in _xActiveStacks)
              if (xCurStack.Location.Position == xLoader.StackPosition && xCurStack.Location.StackType == xLoader.Type && xCurPage.Name == xCurStack.Phase.ToString())
              {
                xMatchStack = xCurStack;
                break;
              }
            if (xMatchStack != null)
              _xActiveStacks.Remove(xMatchStack);
          }
          else
          {
            // search any currently matching stack:
            bool bStackFound = false;
            for (int iCur = 0; iCur < _xActiveStacks.Count; iCur++)
            {
              PnPStack xCurStack = _xActiveStacks[iCur];

              // check if the current Stack matches the one in this StackLoader
              if (xCurStack.Location.Position == xLoader.StackPosition && xCurStack.Location.StackType == xLoader.Type && xCurPage.Name == xCurStack.Phase.ToString())
              {
                xCurStack.LoadReel(xLoader.SelectedReel);
                //if (_xActiveReels.IndexOf(xLoader.SelectedReel) == -1)
                //  _xActiveReels.Add(xLoader.SelectedReel);
                bStackFound = true;
                break;
              }
            }

            // if selected Reel is not yet in the list of active Stacks, add it
            if (!bStackFound)
            {
              StackLocation xLoc = new StackLocation();
              xLoc.Position = xLoader.StackPosition;
              xLoc.StackType = xLoader.Type;
              PnPStack xNewStack = new PnPStack(xLoc, _xMachine.GetStack(xLoc).MaxHeight);
              xNewStack.LoadReel(xLoader.SelectedReel);
              int iNewPhase;
              int.TryParse(xCurPage.Name, out iNewPhase); // retrieve the phase from the tab page
              xNewStack.Phase = iNewPhase;
              _xActiveStacks.Add(xNewStack);
              xLoader.SelectedReel.StackAssigned = true;
              _xActiveReels.Add(xLoader.SelectedReel);
            }

            // add this reel to the list of active reels if it's not part of it yet
            //if (xTempReels.IndexOf(xLoader.SelectedReel) != -1)
            //  xTempReels.Add(xLoader.SelectedReel);
          }
        }
      }
      //_xActiveReels = xTempReels;

     
    }

    private void ExportFiles()
    {
      // count the phases:
      int iPhases = 0;
      foreach (PnPStack xCurStack in _xActiveStacks)
        if (xCurStack.Phase > iPhases)
          iPhases = xCurStack.Phase;

      int iMissingReels = CheckReelConfiguration();
      if (iMissingReels > 0) // reels missing
      {
        string sMsg = string.Empty;
        string sTitle = string.Empty;
        if (iMissingReels == 1)
        {
          sMsg = "There is 1 reel"; 
          sTitle = "Missing reel";
        }
        else
        {
          sMsg = "There are " + iMissingReels.ToString() + " reels";
          sTitle = "Missing reels";
        }
        DialogResult xAns = MessageBox.Show(sMsg + " missing from the configuration to populate the board. Would you like to exclude the associated part(s) from automatic placement and continue exporting production files?", sTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
        if (xAns != DialogResult.Yes)
          return;
      }
      else
      {
        DialogResult xResponse = MessageBox.Show("Export " + iPhases.ToString() + " phase(s) to production files?", "Export", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        if (xResponse != DialogResult.Yes)
          return;
      }

      _xMachine.ExportPnPData(_xPnPFile.Directory, _sProjectName, _xActiveStacks, _xPartsList);
      //_xMachine.ExportPnPData(new FileInfo("file"), _xActiveStacks);



    }

    /// <summary>
    /// Checks the part list for parts that do not have a reel assigned to them, and exclude them from the active aprts list.
    /// </summary>
    private void RemoveReellessParts()
    {
      List<PnPPart>xReellessParts = new List<PnPPart>();
      foreach (PnPPart xCurPart in _xPartsList)
      {
        bool bReelfound = false;
        foreach (PnPReel xCurReel in _xActiveReels)
          if (xCurReel.GUID == xCurPart.AssignedReel)
            bReelfound = true;

        if (!bReelfound)
          xReellessParts.Add(xCurPart);
      }

      for (int iCur = 0; iCur < xReellessParts.Count; iCur++)
      {
        PnPPart xCurPart = xReellessParts[iCur];
        _xPartsList.Remove(xCurPart);
        xCurPart.AssignedReel = Guid.Empty;
        xCurPart.ExclusionReason = ExclusionReason.NoReelAssigned;
        _xExcludedPartsList.Add(xCurPart);
      }
    }
    


    /// <summary>
    /// Stores the current phase configuration to disk
    /// </summary>
    private void SaveStackConfiguration()
    {
      List<PnPStack> xCurPhaseStacks = new List<PnPStack>();
      
      foreach (PnPStack xCurStack in _xActiveStacks)
        if (xCurStack.Phase == _iCurrentPhase)
          xCurPhaseStacks.Add(xCurStack);
      
      ConfigManager.SetLoadedStacks(xCurPhaseStacks);
    }

    
    private void NextPhase(object sender, EventArgs e)
    {
      _iCurrentPhase++;
      // store current stack configuration to disk as a machine setting (project independent)
      //ConfigManager.SetLoadedStacks(_xActiveStacks);
      SaveCurrentPhaseStacks();
      //_xProject.SaveConfiguration(new FileInfo(_sProjectName + ".pnp"));
      UpdatePhaseButtons();      
    }

    private void PreviousPhase(object sender, EventArgs e)
    {
      _iCurrentPhase--;
      UpdatePhaseButtons();
    }

    private void LoadProject(object sender, EventArgs e)
    {
      OpenFileDialog xOpenProject = new OpenFileDialog();
      xOpenProject.Filter = "Pick and Place project (*.pnp)|*.pnp";
      if (xOpenProject.ShowDialog() == DialogResult.OK)
      {
        _xProject = PnPProject.LoadConfiguration(new FileInfo(xOpenProject.FileName));
        _xActiveStacks = _xProject.Stacks;
        _sProjectName = _xProject.Name;
        _PCBCount = _xProject.BoardCount;
        _iCurrentPhase = _xProject.ActivePhase;
        DisplayPhases();
      }
      xOpenProject.Dispose();
    }

    private void SaveProject(object sender, EventArgs e)
    {
      SaveFileDialog xSaveProject = new SaveFileDialog();
      xSaveProject.FileName = _sProjectName + ".pnp";
      if (xSaveProject.ShowDialog() == DialogResult.OK)
      {
        _xProject.SaveConfiguration(new FileInfo(xSaveProject.FileName));
      }
      xSaveProject.Dispose();
    }

    private void UpdatePhaseButtons()
    {
      int iPhases = Phases();
      this.btnNextPhase.Enabled = true;
      this.btnPrevPhase.Enabled = true;

      if (_iCurrentPhase == iPhases)
        this.btnNextPhase.Enabled = false;

      if (_iCurrentPhase == 1)
        this.btnPrevPhase.Enabled = false;

    }

    private void EditReelSettings(object sender, EventArgs e)
    {

    }

    private void PackageTypeChanged(object sender, EventArgs e)
    {

    }








    
  }
}
