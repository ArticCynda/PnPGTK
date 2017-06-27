using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace PnPConverter
{
  /// <summary>
  /// Provides an interface for reading and writing configuration settings from a config file.
  /// </summary>
  class ConfigManager
  {
    const string CONFIG_FILE = "config.ini";
    const string CONFIG_LAST_PROJECT = "default.pnp";

    const string CONFIG_LIBS = "[SMD Libraries]";
    const string CONFIG_STACKS = "[Loaded Stacks]";
    const string CONFIG_NOZZLES = "[Installed Nozzles]";
    const string CONFIG_LAST = "[Last Project]";
  
    const string STACK_UNDEF = "Undefined";
    const string STACK_8MM = "Reel8mm";
    const string STACK_12MM = "Reel12mm";
    const string STACK_16MM = "Reel16mm";
    const string STACK_TRAY16MM = "Tray16mm";

    const string NOZZLE_XS = "XS";
    const string NOZZLE_S = "S";
    const string NOZZLE_M = "M";
    const string NOZZLE_L = "L";

    /// <summary>
    /// The configuration file used to store settings. This field is read only.
    /// </summary>
    public static string ConfigFile
    {
      get
      {
        string dir = System.Environment.CurrentDirectory;
        string filename = System.IO.Path.Combine(dir, CONFIG_FILE);
        return filename;
      }
    }

    /// <summary>
    /// Reads previously saved SMD libraries from disk.
    /// </summary>
    /// <returns>A list of library files.</returns>
    public static List<FileInfo> GetSmdLibraries()
    {
      List<FileInfo> xSmdLibs = new List<FileInfo>();
      if (!File.Exists(CONFIG_FILE))
        return xSmdLibs;
      
      FileStream xStream = new FileStream(CONFIG_FILE, FileMode.Open);
      StreamReader xReader = new StreamReader(xStream);
      
      bool bReadingLibs = false;
      while (!xReader.EndOfStream)
      {
        string sConfigLine = xReader.ReadLine();

        if (bReadingLibs)
        {
          if (!sConfigLine.Trim().Equals(string.Empty))
          {
            if (sConfigLine.StartsWith("["))
              bReadingLibs = false;
            else
            {
              FileInfo xSmdLibrary = new FileInfo(sConfigLine);
              if (xSmdLibrary.Exists)
                xSmdLibs.Add(xSmdLibrary);
            }
          }
        }

        if (sConfigLine.ToLower().StartsWith(CONFIG_LIBS.ToLower()))
          bReadingLibs = true;
      }

      xReader.Close();
      xReader.Dispose();
      xStream.Close();
      xStream.Dispose();

      return xSmdLibs;
    }

    /// <summary>
    /// Reads the last saved stack configuration from disk.
    /// </summary>
    /// <returns>A list of stored stacks for this machine.</returns>
    public static List<PnPStack> GetLoadedStacks()
    {
      List<PnPStack> xStacks = new List<PnPStack>();
      if (!File.Exists(CONFIG_FILE))
        return xStacks;

      FileStream xStream = new FileStream(CONFIG_FILE, FileMode.Open);
      StreamReader xReader = new StreamReader(xStream);
      List<PnPReel> xReelLib = PnPReel.LoadReels();

      bool bReadingStacks = false;
      while (!xReader.EndOfStream)
      {
        string sConfigLine = xReader.ReadLine();

        if (bReadingStacks)
        {
          if (!sConfigLine.Trim().Equals(string.Empty))
          {
            if (sConfigLine.StartsWith("["))
              bReadingStacks = false;
            else
            {
              //xWriter.Write(xStack.Location.StackType.ToString());
              //xWriter.Write("|");

              //xWriter.Write(xStack.Location.Position.ToString());
              //xWriter.Write("|");

              //xWriter.Write(xStack.MaxHeight.ToString());
              //xWriter.Write("|");

              //if (xStack.Locked)
              //  xWriter.Write('1');
              //else
              //  xWriter.Write('0');
              ////xWriter.Write(((int)xStack.Locked).ToString());
              //xWriter.Write('|');

              ////xWriter.WriteLine(PnPReel.SerializeReel(xStack.Reel));
              //xWriter.Write(xStack.Reel.GUID.ToString());


              bool bErrors = false;
              string[] sParams = sConfigLine.Trim().Split(new char[] { '|' });
              // at least 4 parts must be found:
              if (sParams.Length >= 4)
              {
                string sStackType = sParams[0];
                PnPStackType xType = PnPStackType.Undefined;
                switch (sStackType)
                {
                  case STACK_UNDEF:
                    xType = PnPStackType.Undefined;
                    break;
                  case STACK_8MM:
                    xType = PnPStackType.Reel8mm;
                    break;
                  case STACK_12MM:
                    xType = PnPStackType.Reel12mm;
                    break;
                  case STACK_16MM:
                    xType = PnPStackType.Reel16mm;
                    break;
                  case STACK_TRAY16MM:
                    xType = PnPStackType.Tray16mm;
                    break;
                }

                string sPosition = sParams[1];
                int iPosition = -1;
                bErrors = (bErrors || int.TryParse(sPosition, out iPosition));
                StackLocation xStackType = new StackLocation();
                xStackType.StackType = xType;
                xStackType.Position = iPosition;

                string sMaxHeight = sParams[2];
                float fMaxHeight = 0;
                bErrors = (bErrors || !StringToFloat(sMaxHeight, out fMaxHeight));

                string sLocked = sParams[3];
                bool bLocked = false;
                if (sLocked == "1")
                  bLocked = true;
                else if (sLocked == "0")
                  bLocked = false;
                else
                {
                  bLocked = false;
                  bErrors = true;
                }

                Guid xReelID = Guid.Empty;
                try 
	            {	        
                  xReelID = new Guid(sParams[4]);
	            }
	            catch (Exception)
	            {
                  bErrors = true;
	            }
                // extract the reel data:
                //int iReelDataOffset = sStackType.Length + sPosition.Length + sMaxHeight.Length + 3;
                //bool bReelErrors;
                //PnPReel xReelData = PnPReel.DeSerializeReel(sConfigLine.Substring(iReelDataOffset), out bReelErrors);

                // reconstruct the stack:
                //if (!bErrors && !bReelErrors)
                if (!bErrors)
                {
                  PnPStack xNewStack = new PnPStack(xStackType, fMaxHeight);
                  xNewStack.Locked = bLocked;
                  
                  // search the fitted reel for this Stack:
                  foreach (PnPReel xCurReel in xReelLib)
                  {
                    if (xCurReel.GUID == xReelID)
                    {
                      xNewStack.LoadReel(xCurReel);
                      xStacks.Add(xNewStack);
                      break;
                    }
                  }

                  //xNewStack.LoadReel(xReelData);
                  //xStacks.Add(xNewStack);
                }
              }
            }
          }
        }

        if (sConfigLine.ToLower().StartsWith(CONFIG_STACKS.ToLower()))
          bReadingStacks = true;
      }

      xReader.Close();
      xReader.Dispose();
      xStream.Close();
      xStream.Dispose();

      return xStacks;

    }

    /// <summary>
    /// Reads the nozzle configuration from disk.
    /// </summary>
    /// <returns>A series of installed nozzles. An empty array will be returned if no nozzles are found. </returns>
    public static PnPMachineNozzle[] GetInstalledNozzles()
    {
      // if config file doesn't exist, return Undefined nozzles
      if (!File.Exists(CONFIG_FILE))
        return new PnPMachineNozzle[] {};
      PnPMachineNozzle[] xNozzles = new PnPMachineNozzle[] {};

      FileStream xStream = new FileStream(CONFIG_FILE, FileMode.Open);
      StreamReader xReader = new StreamReader(xStream);

      bool bReadingNozzles = false;
      while (!xReader.EndOfStream)
      {
        string sConfigLine = xReader.ReadLine();

        if (bReadingNozzles)
        {
          if (!sConfigLine.Trim().Equals(string.Empty))
          {
            if (sConfigLine.StartsWith("["))
              bReadingNozzles = false;
            else
            {
              string[] sParams = sConfigLine.Trim().Split(new char[] { '|' });
              xNozzles = new PnPMachineNozzle[sParams.Length];
              for (int iCur = 0; iCur < xNozzles.Length; iCur++)
              {
                string sNozzle = sParams[iCur];
                switch (sNozzle.Trim().ToUpper())
                {
                  case NOZZLE_XS:
                    xNozzles[iCur] = PnPMachineNozzle.XS;
                    break;
                  case NOZZLE_S:
                    xNozzles[iCur] = PnPMachineNozzle.S;
                    break;
                  case NOZZLE_M:
                    xNozzles[iCur] = PnPMachineNozzle.M;
                    break;
                  case NOZZLE_L:
                    xNozzles[iCur] = PnPMachineNozzle.L;
                    break;
                  default:
                    xNozzles[iCur] = PnPMachineNozzle.Unsupported;
                    break;
                }
              }
            }
          }
        }

        if (sConfigLine.ToLower().StartsWith(CONFIG_NOZZLES.ToLower()))
          bReadingNozzles = true;
      }

      xReader.Close();
      xReader.Dispose();
      xStream.Close();
      xStream.Dispose();

      return xNozzles;
    }

    /// <summary>
    /// Reads the last saved project file from disk.
    /// </summary>
    /// <returns></returns>
    public static FileInfo GetLastProject()
    {
      // if config file doesn't exist, return Undefined nozzles
      if (!File.Exists(CONFIG_LAST))
        return new FileInfo(CONFIG_LAST_PROJECT);
      //PnPMachineNozzle[] xNozzles = new PnPMachineNozzle[] { };
      FileInfo xProjectFile = new FileInfo(CONFIG_LAST_PROJECT);

      FileStream xStream = new FileStream(CONFIG_FILE, FileMode.Open);
      StreamReader xReader = new StreamReader(xStream);

      bool bReadingFile = false;
      while (!xReader.EndOfStream)
      {
        string sConfigLine = xReader.ReadLine();

        if (bReadingFile)
        {
          if (!sConfigLine.Trim().Equals(string.Empty))
          {
            if (sConfigLine.StartsWith("["))
              bReadingFile = false;
            else
            {
              // try to biuld a new FileInfo object from the read file name. If it is not in a valid format, use a default file.
              try
              {
                xProjectFile = new FileInfo(sConfigLine);
              }
              catch (Exception)
              {
                xProjectFile = new FileInfo(CONFIG_LAST_PROJECT);
              }
            }
          }
        }

        if (sConfigLine.ToLower().StartsWith(CONFIG_LAST.ToLower()))
          bReadingFile = true;
      }

      xReader.Close();
      xReader.Dispose();
      xStream.Close();
      xStream.Dispose();

      return xProjectFile;
    }

    /// <summary>
    /// Converts a string with commas into a floating point value.
    /// </summary>
    /// <param name="text">The input string containing the value.</param>
    /// <returns>The converted floating point value. If the string cannot be converted, 0 will be returned.</returns>
    public static bool StringToFloat(string text, out float value)
    {
      // replace all points by commas
      text = text.Replace(',', '.');

      //float fValue;
      if (!float.TryParse(text, out value))
        return false;
      CultureInfo xDecPoint = new CultureInfo("en-US");
      value = Convert.ToSingle(text, xDecPoint);
      
      return true;
    }

    /// <summary>
    /// Saves a list of known SMD libraries to disk.
    /// </summary>
    /// <param name="libraries">The list of libraries to save.</param>
    public static void SetSmdLibraries(List<FileInfo> libraries)
    {
      foreach (FileInfo xFile in libraries)
        if (xFile.Name == string.Empty)
          throw new FileNotFoundException("The library name cannot be empty.");

      // to write the smd libraries, the stack configuration first have to be retrieved or it will be overwritten!
      List<PnPStack> xStackBackup = GetLoadedStacks();
      PnPMachineNozzle[] xNozzles = GetInstalledNozzles();
      FileInfo xSaveFileBackup = GetLastProject();

      WriteConfigFile(libraries, xStackBackup, xNozzles, xSaveFileBackup);
    }
    
    /// <summary>
    /// Saves the stack configuration of a machine to disk.
    /// </summary>
    /// <param name="stacks">The stack configuration to save.</param>
    public static void SetLoadedStacks(List<PnPStack> stacks)
    {
      // to write the stacks, the smd library list first have to be retrieved or it will be overwritten!
      List<FileInfo> xSmdLibBackup = GetSmdLibraries();
      PnPMachineNozzle[] xNozzleBackup = GetInstalledNozzles();
      FileInfo xSaveFileBackup = GetLastProject();

      WriteConfigFile(xSmdLibBackup, stacks, xNozzleBackup, xSaveFileBackup); 
    }

    /// <summary>
    /// Saves the nozzles configuration of a machine to disk.
    /// </summary>
    /// <param name="nozzles"></param>
    public static void SetNozzles(PnPMachineNozzle[] nozzles)
    {
      // to write the smd libraries, the stack configuration first have to be retrieved or it will be overwritten!
      List<PnPStack> xStackBackup = GetLoadedStacks();
      List<FileInfo> xLibraryBackup = GetSmdLibraries();
      FileInfo xSaveFileBackup = GetLastProject();

      WriteConfigFile(xLibraryBackup, xStackBackup, nozzles, xSaveFileBackup);
    }

    /// <summary>
    /// Saves the last project file to disk
    /// </summary>
    /// <param name="saveFile"></param>
    public static void SetLastProject(FileInfo saveFile)
    {
      List<PnPStack> xStackBackup = GetLoadedStacks();
      List<FileInfo> xLibraryBackup = GetSmdLibraries();
      PnPMachineNozzle[] xNozzlesBackup = GetInstalledNozzles();

      WriteConfigFile(xLibraryBackup, xStackBackup, xNozzlesBackup, saveFile);
    }

    /// <summary>
    /// Writes configuration settings to a config file.
    /// Do NOT call this routine externally, instead use Set and Get functions to retrieve settings.
    /// </summary>
    /// <param name="smdLibraries">SMD libraries to write to the config file.</param>
    /// <param name="stacks">Stack configuration to write to the config file.</param>
    /// <param name="nozzles">Nozzle configuration to write to the config file.</param>
    /// <returns></returns>
    private static bool WriteConfigFile(List<FileInfo> smdLibraries, List<PnPStack> stacks, PnPMachineNozzle[] nozzles, FileInfo saveFile)
    {
      try
      {
        FileStream xStream = new FileStream(CONFIG_FILE, FileMode.Create);
        StreamWriter xWriter = new StreamWriter(xStream);

        xWriter.WriteLine(CONFIG_LIBS);
        foreach (FileInfo xLibrary in smdLibraries)
        {
          if (xLibrary.Exists)
            xWriter.WriteLine(xLibrary.FullName);
        }

        xWriter.WriteLine();
        xWriter.WriteLine(CONFIG_STACKS);
        foreach (PnPStack xStack in stacks)
        {
          switch (xStack.Location.StackType)
          {
            case PnPStackType.Reel8mm:
              xWriter.Write(STACK_8MM);
              break;
            case PnPStackType.Reel12mm:
              xWriter.Write(STACK_12MM);
              break;
            case PnPStackType.Reel16mm:
              xWriter.Write(STACK_16MM);
              break;
            case PnPStackType.Tray16mm:
              xWriter.Write(STACK_TRAY16MM);
              break;
            default:
              xWriter.Write(STACK_UNDEF);
              break; 
          }
          //xWriter.Write(xStack.Location.StackType.ToString());
          xWriter.Write('|');

          xWriter.Write(xStack.Location.Position.ToString());
          xWriter.Write('|');

          xWriter.Write(xStack.MaxHeight.ToString());
          xWriter.Write('|');
  
          if (xStack.Locked)
            xWriter.Write('1');
          else
            xWriter.Write('0');
          //xWriter.Write(((int)xStack.Locked).ToString());
          xWriter.Write('|');

          //xWriter.WriteLine(PnPReel.SerializeReel(xStack.Reel));
          xWriter.Write(xStack.Reel.GUID.ToString());
        }

        xWriter.WriteLine();
        xWriter.WriteLine(CONFIG_NOZZLES);
        for (int iCurNozzle = 0; iCurNozzle < nozzles.Length; iCurNozzle++)
        {
          switch (nozzles[iCurNozzle])
          {
            case PnPMachineNozzle.XS:
              xWriter.Write(NOZZLE_XS);
              break;
            case PnPMachineNozzle.S:
              xWriter.Write(NOZZLE_S);
              break;
            case PnPMachineNozzle.M:
              xWriter.Write(NOZZLE_M);
              break;
            case PnPMachineNozzle.L:
              xWriter.Write(NOZZLE_M);
              break;
            default:
              throw new NozzleNotSupportedException();
          }
          if (iCurNozzle < nozzles.Length -1)
            xWriter.Write('|');
        }
        
        xWriter.WriteLine();
        xWriter.WriteLine(CONFIG_LAST);
        xWriter.WriteLine(saveFile.FullName);

        xWriter.Flush();
        xWriter.Close();
        xWriter.Dispose();
        xStream.Close();
        xStream.Dispose();
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }
  }
}
