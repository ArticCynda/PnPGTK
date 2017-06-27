using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace PnPConverter
{
  public partial class ProjectWizard
  {
    /// <summary>
    /// Current panel shown in the wizard.
    /// </summary>
    private WizardState _xState = WizardState.Intro;

    /// <summary>
    /// A list with BOM data.
    /// </summary>
    private List<BomInfo> _xBomData = new List<BomInfo>();

    /// <summary>
    /// The file with raw pick and place data.
    /// </summary>
    private FileInfo _xPnPFile;

    /// <summary>
    /// Indicates whether the user has imported valid Pick and Place data.
    /// </summary>
    private DataFileStatus _xPnPStatus = DataFileStatus.Undefined;

    /// <summary>
    /// The Bill of Materials file.
    /// </summary>
    private FileInfo _xBomFile;

    /// <summary>
    /// Indicates whether the user has imported valid BOM data.
    /// </summary>
    private DataFileStatus _xBomStatus = DataFileStatus.Undefined;

    /// <summary>
    /// List of parts described in the Altium Designer export file.
    /// </summary>
    private List<PnPPart> _xPnPFilePartsList = new List<PnPPart>();

    /// <summary>
    /// List of surface mount footprints to identify SMD parts
    /// </summary>
    private List<PnPFootprint> _xSmdFootprints = new List<PnPFootprint>();

    /// <summary>
    /// List of included parts in the current project.
    /// </summary>
    private List<PnPPart> _xPartsList = new List<PnPPart>();

    /// <summary>
    /// List of parts in the Altium Designer export file that are excluded from the current project.
    /// </summary>
    private List<PnPPart> _xExcludedPartsList = new List<PnPPart>();

    /// <summary>
    /// Library of available Reels.
    /// </summary>
    private List<PnPReel> _xReelLibrary = new List<PnPReel>();

    /// <summary>
    /// Parts that do not have a matching Reel in the Library.
    /// </summary>
    //private List<PnPPart> _xMissingReelParts = new List<PnPPart>();

    /// <summary>
    /// List of active Reels in the project, with Parts assigned to them.
    /// </summary>
    private List<PnPPartType> _xPartTypeList = new List<PnPPartType>();

    /// <summary>
    /// Name of the project.
    /// </summary>
    private string _sProjectName = string.Empty;

    /// <summary>
    /// Estimated board count for this project.
    /// </summary>
    private int _PCBCount = -1;

    /// <summary>
    /// List of Reels for all Parts in this project.
    /// </summary>
    private List<PnPReel> _xActiveReels = new List<PnPReel>();

    /// <summary>
    /// List of Stacks for all Phases in this project.
    /// </summary>
    private List<PnPStack> _xActiveStacks = new List<PnPStack>();

    /// <summary>
    /// The Pick and Place Machine for which the current project will generate files.
    /// </summary>
    private PnPMachine _xMachine = new TM220A();

    /// <summary>
    /// The project configuration.
    /// </summary>
    private PnPProject _xProject;

    /// <summary>
    /// The current phase in the execution of the project flow.
    /// </summary>
    private int _iCurrentPhase = -1;

    /// <summary>
    /// The configuration of the current panel.
    /// </summary>
    public PnPPanel _xPanelConfiguration;


    public static Image GetOKImage()
    {
      const string OKFILE = "ok.png";
      if (System.IO.File.Exists(OKFILE))
        return Image.FromFile(OKFILE);
      else
        return null;
    }

    public static Image GetNOKImage()
    {
      const string NOKFILE = "nok.png";
      if (System.IO.File.Exists(NOKFILE))
        return Image.FromFile(NOKFILE);
      else
        return null;
    }

    public static Image GetPendingImage()
    {
      const string PENDINGFILE = "pending.png";
      if (System.IO.File.Exists(PENDINGFILE))
        return Image.FromFile(PENDINGFILE);
      else
        return null;
    }
  }

  /// <summary>
  /// Floating point equivalent of System.Drawing.Point.
  /// </summary>
  public class FPoint
  {
    public float X;
    public float Y;

    public FPoint(float x, float y)
    {
      X = x;
      Y = y;
    }
  }

  /// <summary>
  /// Floating point equivalent of System.Drawing.Size.
  /// </summary>
  public class FSize
  {
    public float X;
    public float Y;

    public FSize(float x, float y)
    {
      X = x;
      Y = y;
    }

    public FSize(FPoint point)
    {
      X = point.X;
      Y = point.Y;
    }
  }
}
