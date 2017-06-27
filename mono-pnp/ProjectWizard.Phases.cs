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
    private void ProjectInit()
    {
      FileInfo xLastProjectFile = ConfigManager.GetLastProject();
      _xProject = PnPProject.LoadConfiguration(xLastProjectFile);
    }

    private void AssignReels()
    {
      foreach (PnPPartType xCurType in _xPartTypeList)
      {
        // assign this reel to all parts of this type
        foreach (PnPPart xCurPart in _xPartsList)
        {
          if (xCurType.Designators.IndexOf(xCurPart.Designator) > -1)
          {
            xCurPart.AssignedReel = xCurType.GUID;
          }
        }
      }
    }

    private void GeneratePhases()
    {
      // erase existing data of active reels and stacks:
      _xActiveReels.Clear();
      _xActiveStacks.Clear();

      //foreach (PnPPart xCurPart in _xPartsList)
      //{
      //  // if the user selected a Part to be placed manually, it will not have a Reel assigned to it
      //  if (xCurPart.AssignedReel != Guid.Empty)
      //  {
      //    // search the matching Reel for this Part
      //    PnPReel xReel = null;
      //    foreach (PnPReel xMatchReel in _xReelLibrary)
      //      if (xMatchReel.GUID == xCurPart.AssignedReel)
      //      {
      //        xReel = xMatchReel;
      //        break;
      //      }

      //    // check if no such Reel has been added yet to the list of active Reels:
      //    if (_xActiveReels.IndexOf(xReel) == -1)
      //    {
      //      // add Reel to the list of active Reels:
      //      xReel.StackAssigned = false;
      //      _xActiveReels.Add(xReel);
      //    }
      //  }
      //}

      // assemble a list of Reels required in this project:
      foreach (PnPPartType xPartType in _xPartTypeList)
      {
        if (xPartType.Solution == PartPlaceSolution.Assigned)
        {
          PnPReel xActiveReel = null;
          foreach (PnPReel xCurReel in _xReelLibrary)
          {
            if (xCurReel.GUID == xPartType.GUID)
            {
              xActiveReel = xCurReel;
              break;
            }
          }
          if (xActiveReel == null)
            throw new Exception("Reel data missing");
          else
            _xActiveReels.Add(xActiveReel);
        }
      }

      // initialize stack list with last loaded stacks:
      //_xActiveStacks = _xProject.Stacks;
      List<PnPStack> xLoadedStacks = ConfigManager.GetLoadedStacks();

      // first identify the Reels that are currently loaded into the Machine and can be used, and assign them to Phase 1:
      foreach (PnPStack xCurStack in xLoadedStacks)
      {
        if (xCurStack.Reel != null)
        {
          if (xCurStack.Locked)
          {
            xCurStack.Phase = 1;
            _xActiveStacks.Add(xCurStack);

            // if the reel loaded in this stack is part of the project, mark is as assigned:
            foreach (PnPReel xReel in _xActiveReels)
            {
              if (xReel != null)
              {
                if (xReel.GUID == xCurStack.Reel.GUID)
                {
                  xReel.StackAssigned = true;
                  break;
                }
              }
            }
          }
          else
          {
            foreach (PnPReel xCurReel in _xActiveReels)
            {
              if (xCurReel != null)
              {
                if (xCurReel.GUID == xCurStack.Reel.GUID) // match found, this stack can be reused!
                {
                  _xActiveStacks.Add(xCurStack);
                  break;
                }
              }
            }
          }

        }
      }

      //for (int iCur = 0; iCur < _xActiveReels.Count; iCur++)
      //{
      //  PnPReel xCurReel = _xActiveReels[iCur];
      //  foreach (PnPStack xCurStack in _xMachine.GetStacks(PnPStackType.Undefined))
      //  {
      //    if (xCurStack.Reel.Equals(xCurReel))
      //    {
      //      PnPStack xNewStack = new PnPStack(xCurStack.Location, xCurStack.MaxHeight);
      //      xNewStack.Phase = 1;
      //      _xActiveStacks.Add(xNewStack);
      //      xCurReel.StackAssigned = true;
      //    }
      //  }
      //}
      

      // also identify the locked Stacks and add them to the Stack list to prevent them from being reused
      //foreach (PnPStack xCurStack in _xMachine.GetStacks(PnPStackType.Undefined))
      //  if (xCurStack.Locked)
      //  {
      //    PnPStack xLockedStack = xCurStack;
      //    xLockedStack.Phase = 1;
      //    _xActiveStacks.Add(xLockedStack);
      //  }

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

      _xMachine.ExportPnPData(_xPnPFile.Directory, _sProjectName, _xActiveStacks, _xPartsList, _xPanelConfiguration);
      //_xMachine.ExportPnPData(new FileInfo("file"), _xActiveStacks);

      // debug
      SaveCurrentPhaseStacks();

    }

    /// <summary>
    /// Checks the part list for parts that do not have a reel assigned to them, and exclude them from the active aprts list.
    /// </summary>
    private void RemoveReellessParts()
    {
      List<PnPPart> xReellessParts = new List<PnPPart>();
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



  }
}
