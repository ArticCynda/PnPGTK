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
          //IdentifyMissingReels();
          LoadReelLibraryList();
          FillReelList();
          this.Cursor = Cursors.Default;
          break;
        case WizardState.Project:
          this.pnlAddReels.Visible = false;
          this.pnlPanel.Visible = false;
          this.pnlProjectProperties.Visible = true;
          this.Cursor = Cursors.WaitCursor;
          //SaveNewReels();
          LoadProjectName();
          this.Cursor = Cursors.Default;
          break;
        case WizardState.Panel:
          this.btnNext.Text = "&Next";
          this.pnlProjectProperties.Visible = false;
          this.pnlPhases.Visible = false;
          this.pnlPanel.Visible = true;
          InitPanel();
          break;
        case WizardState.Phases:
          this.btnNext.Text = "E&xport";
          this.pnlPanel.Visible = false;
          this.pnlPhases.Visible = true;
          this.Cursor = Cursors.WaitCursor;
          SavePanelConfiguration();
          ProjectInit();
          AssignReels();
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
        sProperties[3] = xCurPart.PartNameOrValue;
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


   















    
  }
}
