using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PnPConverter
{
  /// <summary>
  /// Represents a Pick and Place project.
  /// </summary>
  class PnPProject
  {
    private const string CONF_STACKS = "[Stacks]";
    private const string CONF_STATS = "[Statistics]";
    private const string CONF_TIME = "DateStamp";
    private const string CONF_NAME = "Name";
    private const string CONF_COUNT = "Boards";
    private const string CONF_PHASE = "Phase";
    private const string CONF_DEF_NAME = "default";

    private const string STACK_UNDEF = "Undefined";
    private const string STACK_8MM = "Reel8mm";
    private const string STACK_12MM = "Reel12mm";
    private const string STACK_16MM = "Reel16mm";
    private const string STACK_TRAY16MM = "Tray16mm";

    private const string ERR_EMPTY_NAME = "The project name cannot be empty.";
    private const string ERR_INV_BOARD_COUNT = "The number of boards must be a positive natural value.";
    private const string ERR_INV_PHASE = "The active phase must be a positive natural value.";

    private List<PnPStack> _xStacks;
    private string _sName = string.Empty;
    private int _iBoardCount = 1;
    private int _iActivePhase = 1;
    private DateTime _xLastSave = DateTime.Now;

    /// <summary>
    /// Represents a Pick and Place project.
    /// </summary>
    public PnPProject(string name)
    {
      if (name.Trim() == string.Empty)
        throw new ArgumentException(ERR_EMPTY_NAME);
      _sName = name;
    }

    /// <summary>
    /// The stack list associated with this project.
    /// </summary>
    public List<PnPStack> Stacks
    {
      get{ return _xStacks; }
      set{ _xStacks = value; }
    }

    /// <summary>
    /// The name of this project.
    /// </summary>
    public string Name
    {
      get{ return _sName; }
      set
      {
        if (value.Trim() == string.Empty)
          throw new ArgumentException(ERR_EMPTY_NAME);

        _sName = value;
      }
    }
 
    /// <summary>
    /// The number of boards to be manufactured in this project.
    /// </summary>
    public int BoardCount
    {
      get{ return _iBoardCount; }
      set
      {
        if (value <= 0)
          throw new ArgumentException(ERR_INV_BOARD_COUNT);

        _iBoardCount = value;
      }
    }

    /// <summary>
    /// The active phase when the project was last saved.
    /// </summary>
    public int ActivePhase
    {
      get{ return _iActivePhase; }
      set
      {
        if (value <= 0)
          throw new ArgumentException(ERR_INV_PHASE);

        _iActivePhase = value;
      }
    }

    /// <summary>
    ///  The moment when the loaded project was last saved.
    /// </summary>
    public DateTime TimeStamp
    {
      get{ return _xLastSave; }
      set{ _xLastSave = value; }
    }

    /// <summary>
    /// Saves the project configuration to the specified file.
    /// </summary>
    /// <param name="file">The save file. If the file exists, it will be overwritten.</param>
    public void SaveConfiguration(FileInfo file)
    {
      FileStream xStream = new FileStream(file.FullName, FileMode.Create);
      StreamWriter xWriter = new StreamWriter(xStream);

      // first write the project's statistics:
      xWriter.WriteLine(CONF_STATS);
      
      // write time:
      xWriter.Write(CONF_TIME);
      xWriter.Write('=');
      xWriter.WriteLine(DateTime.Now.ToString());

      // write project name:
      xWriter.Write(CONF_NAME);
      xWriter.Write('=');
      xWriter.WriteLine(_sName);

      // write board count:
      xWriter.Write(CONF_COUNT);
      xWriter.Write('=');
      xWriter.WriteLine(_iBoardCount.ToString());

      // write active phase:
      xWriter.Write(CONF_PHASE);
      xWriter.Write('=');
      xWriter.WriteLine(_iActivePhase.ToString());
      xWriter.WriteLine();

      // then write all the stacks:
      xWriter.WriteLine(CONF_STACKS);
      
      // loop through the stacks and write them to disk:
      foreach (PnPStack xCurStack in _xStacks)
      {
        // write stack type:
        switch (xCurStack.Location.StackType)
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

        xWriter.Write(xCurStack.Location.Position.ToString());
        xWriter.Write('|');

        xWriter.Write(xCurStack.MaxHeight.ToString());
        xWriter.Write('|');

        if (xCurStack.Locked)
          xWriter.Write('1');
        else
          xWriter.Write('0');
        //xWriter.Write(((int)xStack.Locked).ToString());
        xWriter.Write('|');

        //xWriter.WriteLine(PnPReel.SerializeReel(xStack.Reel));
        xWriter.Write(xCurStack.Reel.GUID.ToString());
        xWriter.Write('|');

        xWriter.WriteLine(xCurStack.Phase.ToString());
      }

      xWriter.Flush();
      xStream.Flush();

      xWriter.Close();
      xStream.Close();

      xWriter.Dispose();
      xStream.Dispose();
    }

    /// <summary>
    /// Loads a project configuration from disk. 
    /// </summary>
    /// <param name="file"></param>
    public static PnPProject LoadConfiguration(FileInfo file)
    {
      List<PnPStack> xStacks = new List<PnPStack>();
      string sProjectName = CONF_DEF_NAME;
      int iBoardCount = 1;
      DateTime xTimeStamp = DateTime.Now;
      int iActivePhase = 1;
      PnPProject xNewProject;

      try
      {
        if (file.Exists)
        {
          FileStream xStream = new FileStream(file.FullName, FileMode.Open);
          StreamReader xReader = new StreamReader(xStream);
          List<PnPReel> xReelLib = PnPReel.LoadReels();

          bool bReadingStacks = false;
          bool bReadingSettings = false;
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
                    bErrors = (bErrors || !ConfigManager.StringToFloat(sMaxHeight, out fMaxHeight));

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

            if (bReadingSettings)
            {
              if (!sConfigLine.Trim().Equals(string.Empty))
              {
                if (sConfigLine.StartsWith("["))
                  bReadingSettings = false;
                else
                {
                  // retrieve time stamp if the CONF_TIME key is found:
                  if (sConfigLine.Trim().ToLower().StartsWith(CONF_TIME + '='))
                  {
                    string sTimeString = sConfigLine.Substring(CONF_TIME.Length);
                    try
                    {
                      xTimeStamp = Convert.ToDateTime(sTimeString);
                    }
                    catch (Exception)
                    {

                      xTimeStamp = DateTime.Now;
                    }
                  }
                  // retrieve project name if the CONF_NAME key is found:
                  else if (sConfigLine.Trim().ToLower().StartsWith(CONF_NAME + '='))
                  {
                    sProjectName = sConfigLine.Substring(CONF_NAME.Length);
                    if (sProjectName.Length == 0)
                      sProjectName = CONF_DEF_NAME;
                  }
                  // retrieve board count if the CONF_COUNT key is found:
                  else if (sConfigLine.Trim().ToLower().StartsWith(CONF_COUNT + '='))
                  {
                    string sCountString = sConfigLine.Substring(CONF_COUNT.Length);
                    int.TryParse(sCountString, out iBoardCount);
                  }
                  else if (sConfigLine.Trim().ToLower().StartsWith(CONF_PHASE + '='))
                  {
                    string sPhaseString = sConfigLine.Substring(CONF_PHASE.Length);
                    int.TryParse(sPhaseString, out iActivePhase);
                  }
                }
              }
            }

            // get into stack reading mode if the CONF_STACKS key is found:
            if (sConfigLine.ToLower().StartsWith(CONF_STACKS.ToLower()))
              bReadingStacks = true;

            // get into project property reading mode if the CONF_STATS key is found:
            if (sConfigLine.ToLower().StartsWith(CONF_STATS.ToLower()))
              bReadingSettings = true;
          }

          xReader.Close();
          xReader.Dispose();
          xStream.Close();
          xStream.Dispose();
        }
      }
      finally
      {
        xNewProject = new PnPProject(sProjectName);
        xNewProject.ActivePhase = iActivePhase;
        xNewProject.BoardCount = iBoardCount;
        xNewProject.Stacks = xStacks;
        xNewProject.TimeStamp = xTimeStamp;
        //timeStamp = xTimeStamp;
        //projectName = sProjectName;
        //boardCount = iBoardCount;
        //activePhase = iActivePhase;
        //return xStacks;
      }

      return xNewProject;
    }
  }
}
