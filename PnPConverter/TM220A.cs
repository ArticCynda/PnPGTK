using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace PnPConverter
{
  class TM220A : PnPMachine
  {
    private List<PnPStack> _xStacks = new List<PnPStack>();
    private PnPMachineNozzle[] _xNozzles = new PnPMachineNozzle[2];
    //private PCBOffset _xPCBOffset = new PCBOffset();
    //private PnPPanel _xPanel = new PnPPanel();

    const int REEL_HEIGHT = 5;
    const int TRAY_HEIGHT = 8;

    public TM220A()
    {
      // set configuration for 8 mm stacks:
      for (int i = 1; i <= 12; i++)
      {
        StackLocation xLoc = new StackLocation();
        xLoc.StackType = PnPStackType.Reel8mm;
        xLoc.Position = i;
        // set max height of parts on this machine to 5 mm:
        PnPStack xNewStack = new PnPStack(xLoc, REEL_HEIGHT);
        _xStacks.Add(xNewStack);
      }

      // set configuration for 12 mm stacks:
      for (int j = 1; j <= 2; j++)
      {
        StackLocation xLoc = new StackLocation();
        xLoc.StackType = PnPStackType.Reel12mm;
        xLoc.Position = j;
        // set max height of parts on this machine to 5 mm:
        PnPStack xNewStack = new PnPStack(xLoc, REEL_HEIGHT);
        _xStacks.Add(xNewStack);
      }

      // set configuration for 16 mm stack:
      StackLocation x16mmLoc = new StackLocation();
      x16mmLoc.StackType = PnPStackType.Reel16mm;
      x16mmLoc.Position = 1;
      PnPStack x16mmStack = new PnPStack(x16mmLoc, REEL_HEIGHT);
      _xStacks.Add(x16mmStack);

      // set configuration for 16 mm tray:
      x16mmLoc = new StackLocation();
      x16mmLoc.StackType = PnPStackType.Tray16mm;
      x16mmLoc.Position = 1;
      x16mmStack = new PnPStack(x16mmLoc, TRAY_HEIGHT);
      _xStacks.Add(x16mmStack);

      // set configuration for nozzles:
      PnPMachineNozzle[] xReadNozzles = ConfigManager.GetInstalledNozzles();
      if (xReadNozzles.Length == 0)
        _xNozzles = new PnPMachineNozzle[] {PnPMachineNozzle.Undefined, PnPMachineNozzle.Undefined};
      else if (xReadNozzles.Length == 1)
        _xNozzles = new PnPMachineNozzle[] {xReadNozzles[0], PnPMachineNozzle.Undefined};
      else
        _xNozzles = new PnPMachineNozzle[] {xReadNozzles[0], xReadNozzles[1]};
        
    }

    public override List<PnPStack> GetStacks(PnPStackType type)
    {
      // if no stack type is specified, return all of them:
      if (type == PnPStackType.Undefined)
        return _xStacks;

      // search stacks of the given type:
      List<PnPStack> xStackList = new List<PnPStack>();
      foreach (PnPStack xStack in _xStacks)
        if (xStack.Location.StackType == type)
          xStackList.Add(xStack);

      return xStackList;
    }

    public override PnPStackType[] GetStackTypes()
    {
      PnPStackType[] xStackTypes = new PnPStackType[] { PnPStackType.Reel8mm, PnPStackType.Reel12mm, PnPStackType.Reel16mm, PnPStackType.Tray16mm };
      return xStackTypes;
    }

    public override PnPSupplyPackage[] GetSupportedPackages(PnPStackType stackType)
    {
      switch (stackType)
      {
        case PnPStackType.Reel8mm:  return new PnPSupplyPackage[] {PnPSupplyPackage.Reel8mm};
        case PnPStackType.Reel12mm: return new PnPSupplyPackage[] {PnPSupplyPackage.Reel12mm};
        case PnPStackType.Reel16mm: return new PnPSupplyPackage[] {PnPSupplyPackage.Reel16mm};
        case PnPStackType.Tray16mm: return new PnPSupplyPackage[] {PnPSupplyPackage.Reel8mm, PnPSupplyPackage.Reel12mm, PnPSupplyPackage.Reel16mm, PnPSupplyPackage.Tray, PnPSupplyPackage.Tube};
      }
      return new PnPSupplyPackage[] {PnPSupplyPackage.Reel8mm, PnPSupplyPackage.Reel12mm, PnPSupplyPackage.Reel16mm, PnPSupplyPackage.Tray, PnPSupplyPackage.Tube};
    }

    public override PnPStack GetStack(StackLocation location)
    {
      PnPStack xFoundStack = null;
      foreach (PnPStack xCurStack in _xStacks)
        if (xCurStack.Location.Position == location.Position && xCurStack.Location.StackType == location.StackType)
          xFoundStack = xCurStack;
      
      return xFoundStack;
    }

    /// <summary>
    /// Counts the number of phases in the specified Stack list.
    /// </summary>
    /// <param name="stacks">The Stacks loaded onto the Machine.</param>
    /// <returns>The number of Phases for this project.</returns>
    public static int PhaseCount(List<PnPStack> stacks)
    {
      int iPhaseCount = 0;
      foreach (PnPStack xCurStack in stacks)
        if (xCurStack.Phase > iPhaseCount)
          iPhaseCount = xCurStack.Phase;

      return iPhaseCount;
    }

    public override void ExportPnPData(DirectoryInfo directory, string fileNamePrefix, List<PnPStack> stacks, List<PnPPart> parts, PnPPanel panel)
    {
      // search the phase count for this project:
      int iPhaseCount = PhaseCount(stacks);

      for (int iPhase = 1; iPhase <= iPhaseCount; iPhase++)
      {
        // generate a file for every phase:
        string sFileName = directory.FullName + Path.DirectorySeparatorChar + fileNamePrefix + "-Phase" + iPhase.ToString() + ".csv";

        FileStream xStream = new FileStream(sFileName, FileMode.Create);
        StreamWriter xWriter = new StreamWriter(xStream, Encoding.ASCII);

        // Write the board setup
        xWriter.WriteLine("%,OriginOffsetCommand,X,Y,,");
        xWriter.Write("65535,0,");
        if (panel.IsPanel)
        {
          float fOffsetX = panel.BorderX; // + _xPanel.RouterWidth;
          xWriter.Write(fOffsetX.ToString().Replace(',', '.'));
          xWriter.Write(",");
          float fOffsetY = panel.BorderY; // + _xPanel.RouterWidth;
          xWriter.Write(fOffsetY.ToString().Replace(',', '.'));
        }
        else
        {
          // not a panel, offsets are 0
          xWriter.Write('0');
          xWriter.Write(",");
          xWriter.Write('0');
        }
        //xWriter.Write(_xPCBOffset.XOffset.ToString());
        //xWriter.Write(",");
        //xWriter.Write(_xPCBOffset.YOffset.ToString());
        xWriter.WriteLine(",,");
        xWriter.WriteLine();

        // Write the stack setup
        xWriter.WriteLine("%,StackOffsetCommand,Stack,X,Y,Comment");
        PnPStackType[] xStackOrder = {PnPStackType.Tray16mm, PnPStackType.Reel8mm, PnPStackType.Reel12mm, PnPStackType.Reel16mm}; // order of stacks on this machine
        int iRowCounter = 0;
        foreach (PnPStackType xCurStackType in xStackOrder)
        {
          // retrieve the number of stacks of this type:
          int iStackCount = this.GetStacks(xCurStackType).Count;
          
          // configure all stacks of this type:
          for (int iCur = 1; iCur <= iStackCount; iCur++)
          {
            // write the lead:
            xWriter.Write("65535,1,");
            xWriter.Write(iRowCounter.ToString());
            xWriter.Write(",");
            
            // search the stack on this location:
            PnPStack xStack = null;
            foreach (PnPStack xCurStack in stacks)
              if (xCurStack.Phase == iPhase && xCurStack.Location.StackType == xCurStackType && xCurStack.Location.Position == iCur)
              {
                xStack = xCurStack;
                break;
              } 

            if (xStack == null)
            {
              // no Reel is loaded in this Stack
              xWriter.WriteLine("0,0,"); //questionable
            }
            else
            {
              // DEBUG: if the location is the front IC tray, then adjust the coordinates accordingly
              // values determined experimentally!
              if (xCurStackType == PnPStackType.Tray16mm)
              {
                float fTrayX = (float)(-9 + xStack.Reel.PartSize.X / 2);
                float fTrayY = (float)(8.25 - xStack.Reel.PartSize.Y / 2);
                xWriter.Write(fTrayX.ToString().Replace(',', '.'));
                xWriter.Write(',');
                xWriter.Write(fTrayY.ToString().Replace(',', '.'));
                xWriter.Write(',');
              }
              else
              {
                // write the Reel parameters for this Stack:
                xWriter.Write(xStack.Reel.Offset.X.ToString().Replace(',', '.'));
                xWriter.Write(',');
                xWriter.Write(xStack.Reel.Offset.Y.ToString().Replace(',', '.'));
                xWriter.Write(',');
              }
              xWriter.WriteLine(xStack.Reel.Footprint.Name);
            }

            // increase row counter:
            iRowCounter++;
          }
        }
        xWriter.WriteLine();

        // Write the feed spacing
        xWriter.WriteLine("%,FeederSpacingCommand,Stack,FeedSpacing,");
        iRowCounter = 0;
        foreach (PnPStackType xCurStackType in xStackOrder)
        {
          if (xCurStackType == PnPStackType.Tray16mm)
          {
            xWriter.WriteLine("65535,2,0,18,"); // set feed spacing to 18mm for the front tray
            iRowCounter++;
          }
          else
          {
            // retrieve the number of stacks of this type:
            int iStackCount = this.GetStacks(xCurStackType).Count;

            for (int iCur = 1; iCur <= iStackCount; iCur++)
            {
              // write the lead
              xWriter.Write("65535,2,");
              xWriter.Write(iRowCounter.ToString());
              xWriter.Write(",");

              // search the stack on this location:
              PnPStack xStack = null;
              foreach (PnPStack xCurStack in stacks)
                if (xCurStack.Phase == iPhase && xCurStack.Location.StackType == xCurStackType && xCurStack.Location.Position == iCur)
                {
                  xStack = xCurStack;
                  break;
                }

              if (xStack == null)
              {
                // no Reel is loaded in this Stack
                xWriter.Write("0");
              }
              else
              {
                // write the Reel offset for this Stack
                xWriter.Write(xStack.Reel.FeedSpacing.ToString());
              }
              xWriter.WriteLine(",");

              // debug
              iRowCounter++;
            }
          }
        }
        xWriter.WriteLine();

        // Write the panel setup
        xWriter.WriteLine("%,JointedBoardCommand,X,Y,");
        if (panel.IsPanel)
        {
          // calculate the position for each board on the panel
          float fOffsetX = 0; //_xPanel.BorderX + _xPanel.RouterWidth;
          float fOffsetY = 0; // _xPanel.BorderY + _xPanel.RouterWidth;

          for (int i = 1; i <= panel.ArrayWidth; i++)
            for (int j = 1; j <= panel.ArrayHeight; j++)
              if (!(i == 1 && j == 1)) // skip (1,1) board
              {
                fOffsetX = (i - 1) * (panel.Width + panel.DistanceX);
                fOffsetY = (j - 1) * (panel.Height + panel.DistanceY);
                xWriter.Write("65535,3,");
                xWriter.Write(fOffsetX.ToString().Replace(',', '.'));
                xWriter.Write(',');
                xWriter.Write(fOffsetY.ToString().Replace(',', '.'));
                xWriter.WriteLine(",0,0,0,0");
              }
        }
        //xWriter.WriteLine("%65535,3,0,0,0,0,0,0");
        xWriter.WriteLine();


        // Write the parts to be placed
        xWriter.WriteLine("%,Head,Stack,X,Y,R,H,Skip,Ref,Comment,");
        iRowCounter = 1;  // part numbers start at 1 instead of 0
        int iCurSpeed = -1;   // current speed setting, a value between 1 and 10
        foreach (PnPPart xCurPart in parts)
        {
          // search the Stack in which this Part is loaded
          PnPStack xStack = null;
          foreach (PnPStack xCurStack in stacks)
            if (xCurStack.Phase == iPhase && xCurStack.Reel.GUID == xCurPart.AssignedReel)
            {
              xStack = xCurStack;
              break;
            }

          
          if (xStack == null)
          {
            // not for this phase, don't do anything
            //throw new ArgumentException("No matching stack for part " + xCurPart.Designator + " (" + xCurPart.PartNumberOrValue + ") could be found.");
          }
          else
          {
            // adjust speed if necessary
            int iPartSpeed = xStack.Reel.Speed;
            if (iPartSpeed < 1 || iPartSpeed > 100)
              throw new ArgumentOutOfRangeException("The specified speed setting " + iPartSpeed.ToString() + " is unsupported by this machine.");
            else
            {
              int iDivSpeed = Convert.ToInt32(iPartSpeed / 10); // machine accepts values between 1 and 10
              if (iDivSpeed < 1)
                iDivSpeed = 1;
              if (iDivSpeed > 10)
                iDivSpeed = 10;

              if (iDivSpeed != iCurSpeed)
              {
                // write a line to adjust speed:
                xWriter.Write("0,");

                xWriter.Write(iDivSpeed.ToString());
                xWriter.WriteLine(",0,0,0,0,0,0,");

                iCurSpeed = iDivSpeed;
              }
            }

            // write part number
            xWriter.Write(iRowCounter.ToString());
            xWriter.Write(',');

            // select nozzle
            PnPMachineNozzle xNozzle = SelectNozzle(xStack.Reel.Nozzle);
            if (_xNozzles[0].Equals(xNozzle))
              xWriter.Write("1,");
            else
              xWriter.Write("2,");

            // determine the absolute position of this Stack on the Machine and write it to the file:
            switch (xStack.Location.StackType)
            {
              case PnPStackType.Tray16mm:
                xWriter.Write('0'); // tray is stack 0 on the TM220A
                break;
              case PnPStackType.Reel8mm:
                xWriter.Write(xStack.Location.Position.ToString()); 
                break;
              case PnPStackType.Reel12mm:
                int iIndex1 = this.GetStacks(PnPStackType.Reel8mm).Count + xStack.Location.Position;
                xWriter.Write(iIndex1.ToString());
                break;
              case PnPStackType.Reel16mm:
                int iIndex2 = this.GetStacks(PnPStackType.Reel8mm).Count + this.GetStacks(PnPStackType.Reel12mm).Count + xStack.Location.Position;
                xWriter.Write(iIndex2.ToString());
                break;
              case PnPStackType.Undefined:
                // this should not normally occur!
                throw new ArgumentOutOfRangeException("Stack type undefined for stack with reel " + xStack.Reel.ToString());
            }
            xWriter.Write(',');

            // write coordinates
            // coordinates should be in mm and be accurate to maximum 3 decimals (1 µm)
            float fMidX, fMidY;
            fMidX = (float)Math.Round(xCurPart.Coordinates.MidX, 3);
            fMidY = (float)Math.Round(xCurPart.Coordinates.MidY, 3);
            string sMidX, sMidY;
            sMidX = string.Format("{0:0.000}", fMidX.ToString().Replace(',', '.'));
            sMidY = string.Format("{0:0.000}", fMidY.ToString().Replace(',', '.'));
            xWriter.Write(sMidX);
            xWriter.Write(",");
            xWriter.Write(sMidY);
            xWriter.Write(",");

            // write rotation
            // rotation must be in range -180° to 180° AND be an integer value!
            // DEBUG: add rotation of the reel to the part's rotation:
            PnPReel xPartReel = new PnPReel();
            bool bReelFound = false;
            foreach (PnPStack xCurStack in stacks)
              if (xCurStack.Reel.GUID == xCurPart.AssignedReel)
              {
                xPartReel = xCurStack.Reel;
                bReelFound = true;
                break;
              }
            float fRot = xCurPart.Rotation;
            if (bReelFound)
              fRot += xPartReel.Rotation;
            fRot = fRot % 360;

            //float fRot = xCurPart.Rotation % 360;
            if (fRot > 180)
              fRot = fRot - 360;
            int iRot = (int)Math.Round(fRot, 0);
            xWriter.Write(iRot.ToString().Replace(',', '.'));
            xWriter.Write(",");

            // write height
            xWriter.Write(xStack.Reel.Footprint.Height.ToString().Replace(',', '.'));
            xWriter.Write(",");

            // write skip
            xWriter.Write("0,");

            // write designator
            xWriter.Write(xCurPart.Designator);
            xWriter.Write(",");

            // write comment
            xWriter.WriteLine(xCurPart.PartNameOrValue + " (" + xCurPart.AssignedReel.ToString() + ")");

            // increase row counter
            iRowCounter++;
          }
        }

        xWriter.Flush();
        xWriter.Close();
      }
          
   
    }

    public PnPMachineNozzle SelectNozzle(PnPMachineNozzle desiredNozzle)
    {
      PnPMachineNozzle xProposedNozzle;
      if (desiredNozzle.Equals(PnPMachineNozzle.Undefined))
        xProposedNozzle = PnPMachineNozzle.M;
      else
        xProposedNozzle = desiredNozzle;

      // principle: if the desired nozzle is not installed, select the direct smaller one that is available
      while (!xProposedNozzle.Equals(PnPMachineNozzle.XS))
      {
        for (int i = 0; i < 2; i++)
          if (_xNozzles[i].Equals(xProposedNozzle))
            return xProposedNozzle;

        switch (xProposedNozzle)
        {
          case PnPMachineNozzle.L:
            xProposedNozzle = PnPMachineNozzle.M;
            break;
          case PnPMachineNozzle.M:
            xProposedNozzle = PnPMachineNozzle.S;
            break;
          case PnPMachineNozzle.S:
            xProposedNozzle = PnPMachineNozzle.XS;
            break;
          case PnPMachineNozzle.XS:
            return PnPMachineNozzle.XS;
            break;
          default:
            throw new NozzleNotSupportedException();
        }
      }
      return PnPMachineNozzle.XS;
    }

    public override PnPStackType GetStackType(StackLocation location)
    {
      return location.StackType;
    }

    public void LoadStackConfiguration(FileInfo configFile)
    {
      for (int i = 0; i < _xStacks.Count; i++)
        _xStacks[i].UnloadReel();

      if (!configFile.Exists)
        return;
    
      FileStream xStream = new FileStream(configFile.FullName, FileMode.Open);
      StreamReader xReader = new StreamReader(xStream);
      
      // remove the header line
      if (!xReader.EndOfStream)
        xReader.ReadLine();

      while (!xReader.EndOfStream)
      {
        string sStackLine = xReader.ReadLine();
        string[] sParams = sStackLine.Split(new char[] {','});
        
        // search the position in the stack set:
        int iPos = -1;
        //for (int i = 0; i < _sPosition.Length; i++)
        //  if (_sPosition[i].Trim().ToLower().Equals(sParams[0].Trim().ToLower()))
        //  {
        //    iPos = i;
        //    break;
        //  }
        
        // if the position doesn't exist, skip this stack:
        if (iPos != -1 || sParams.Length != 9)
        {
          //Nozzle xNozzle = Nozzle.Undefined;
          //switch (sParams[3].Trim().ToLower())
          //{
          //  case "XS":
          //    xNozzle = Nozzle.XS;
          //    break;
          //  case "S":
          //    xNozzle = Nozzle.S;
          //    break;
          //  case "M":
          //    xNozzle = Nozzle.M;
          //    break;
          //  case "L":
          //    xNozzle = Nozzle.L;
          //    break;
          //}
          float fX, fY, fSpeed, fHeight, fFeedSpacing;
          float.TryParse(sParams[4], out fX);
          float.TryParse(sParams[5], out fY);
          float.TryParse(sParams[6], out fSpeed);
          float.TryParse(sParams[7], out fHeight);
          float.TryParse(sParams[8], out fFeedSpacing);
          PnPFootprint xFootprint = new PnPFootprint();
          xFootprint.Name = sParams[1];
            
          //PnPStack xCurStack = new PnPStack(
            //_xStackTypes[iPos],
            
             

//new PnPStack(_xStackTypes[iPos], xFootprint, sParams[2], fSpeed, fX, fY, fFeedSpacing, sParams[0], xNozzle, fHeight);
          //_xStacks[iPos] = xCurStack;
        }
    

        //PnPStack xCurStack = new PnPStack


      }

    }

    public void SaveStackConfiguration(FileInfo configFile)
    {
      FileStream xStream = new FileStream(configFile.FullName, FileMode.Create);
      StreamWriter xWriter = new StreamWriter(xStream);

      xWriter.WriteLine("Stack Config File for TM220A");
      for (int i = 0; i < _xStacks.Count; i++)
      {
        if (_xStacks[i].IsLoaded)
        {
          //xWriter.Write(_sPosition[i]);
          xWriter.Write(",");
          xWriter.Write(_xStacks[i].Reel.Footprint);
          xWriter.Write(",");
          xWriter.Write(_xStacks[i].Reel.PartNameOrValue);
          xWriter.Write(",");
          xWriter.Write(_xStacks[i].Reel.Nozzle.ToString());
          xWriter.Write(",");
          //xWriter.Write(_xStacks[i].OffsetX.ToString());
          xWriter.Write(",");
          //xWriter.Write(_xStacks[i].OffsetY.ToString());
          xWriter.Write(",");
          //xWriter.Write(_xStacks[i].Speed.ToString());
          xWriter.Write(",");
          xWriter.Write(_xStacks[i].Reel.Footprint.Height.ToString());
          xWriter.Write(",");
          //xWriter.Write(_xStacks[i].FeedSpacing.ToString());
          xWriter.WriteLine();
        }
      }

      xWriter.Flush();
      xWriter.Close();
      xWriter.Dispose();
      xStream.Close();
      xStream.Dispose();  
    }
  }

/*
  public struct PCBOffset
  {
    public float XOffset;
    public float YOffset;
  }
 * */

}
