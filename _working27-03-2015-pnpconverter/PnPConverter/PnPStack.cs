using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Reflection;

namespace PnPConverter
{
  /// <summary>
  /// Represents a Stack as place holder in a Machine into which Reels can be loaded.
  /// </summary>
  public class PnPStack
  {
    private PnPReel _xReel;           // the Reel currently loaded in this Stack
    private bool _bLoaded;            // indicates whether a Reel has been loaded in this Stack
    private StackLocation _xLocation; // the location of this Stack in the machine
    private float _fMaxHeight;        // maximum height of Parts this Stack can accomodate
    private bool _bLocked;            // if Locked, the Reel loaded in this Stack will not be changed automatically
    private int _iPhase;              // the phase to which a stack belongs

    /// <summary>
    /// Creates a new Stack.
    /// </summary>
    /// <param name="type">The type of the stack, defining the type of Reel that can be loaded into it.</param>
    /// <param name="position">The position of this Stack in the machine.</param>
    /// <param name="maxHeight">The maximum height of Reels that this Stack can accomodate.</param>
    public PnPStack(StackLocation location, float maxHeight)
    {
      if (location.StackType == PnPStackType.Undefined)
        throw new ArgumentOutOfRangeException("Stack type must be defined.");
      if (location.Position <= 0)
        throw new ArgumentOutOfRangeException("Position of this stack on the machine must be a positive value.");
      _xLocation = location;

      if (maxHeight <= 0)
        throw new ArgumentOutOfRangeException("The height of the stack must be a positive value.");
      else
        _fMaxHeight = maxHeight;
    }

    /// <summary>
    /// Load a Reel in this Stack.
    /// </summary>
    /// <param name="Footprint">The footprint of the part to be loaded.</param>
    /// <param name="Value">The value of the part to be loaded.</param>
    public void LoadReel(PnPReel reel)
    {
      const string EMPTY_REEL = "An undefined Reel cannot be loaded into the Stack.";

      if (reel.GUID.Equals(Guid.Empty))
        throw new ArgumentNullException(EMPTY_REEL);

      _xReel = reel;
      _bLoaded = true;
    }

    /// <summary>
    /// Unload this Stack.
    /// </summary>
    public void UnloadReel()
    {
      _bLoaded = false;
      _xReel.GUID = Guid.Empty;
    }
    
    /// <summary>
    /// Gets the Reel that is currently loaded into this Stack. If no Reel is loaded, an empty Reel object will be returned. Use IsLoaded to check whether a Reel is loaded. This field is read only, use LoadReel() to load a new Reel into this Stack.
    /// </summary>
    public PnPReel Reel
    {
      get
      {
        if (!_bLoaded)
          return new PnPReel();
        else
          return _xReel;
      }
    }

    /// <summary>
    /// Gets or sets the Phase this Stack belongs to.
    /// </summary>
    public int Phase
    {
      get{ return _iPhase; }
      set
      {
        if (value < -1)
          throw new ArgumentOutOfRangeException("Phase must be an integer greater or equal to zero, or -1 to indicate an undefined phase.");
        else
          _iPhase = value;
      }
    }

    /// <summary>
    /// A value indicating whether or not a part is loaded in this Stack. This field is read only.
    /// </summary>
    public bool IsLoaded
    {
      get{ return _bLoaded; }
    }

    /// <summary>
    /// A value indicating whether the Reel configuration for this Stack may be changed automatically. If True, the currently loaded Reel will be preserved. To avoid needless changing of Reels, it is recommended to Lock common parts (i.e. 100nF capacitors, LEDs etc.). If an empty Stack is Locked, no Reels will automatically be loaded into it.
    /// </summary>
    public bool Locked
    {
      get{ return _bLocked; }
      set{ _bLocked = value; }
    }

    /// <summary>
    /// The maximum height of Reels that this Stack can accomodate. This field is read only.
    /// </summary>
    public float MaxHeight
    {
      get{ return _fMaxHeight; }
    }

    /// <summary>
    /// The location of this stack in the machine.
    /// </summary>
    public StackLocation Location
    {
      get{ return _xLocation; }
      set
      {
        if (value.StackType == PnPStackType.Undefined)
          throw new ArgumentOutOfRangeException("Stack type must be defined.");

        if (value.Position <= 0)
          throw new ArgumentOutOfRangeException("Position of this stack on the machine must be a positive value.");
        _xLocation = value;
      }
    }
  }

  /// <summary>
  /// The type and size of a Stack.
  /// </summary>
  public enum PnPStackType
  {
    Undefined,
    [Description("Reel 8 mm")]
    Reel8mm,
    [Description("Reel 12 mm")]
    Reel12mm,
    [Description("Reel 16 mm")]
    Reel16mm,
    [Description("Tray 16 mm")]
    Tray16mm
  }

  class Description : Attribute
  {
    public string Text;
    public Description(string text)
    {
      Text = text;
    }
  }

  class AttributeHandler
  {
    public static string GetDescription(Enum en)
    {

      Type type = en.GetType();

      MemberInfo[] memInfo = type.GetMember(en.ToString());

      if (memInfo != null && memInfo.Length > 0)
      {

        object[] attrs = memInfo[0].GetCustomAttributes(typeof(Description),
        false);

        if (attrs != null && attrs.Length > 0)

          return ((Description)attrs[0]).Text;

      }

      return en.ToString();

    }
  }

  /// <summary>
  /// Specifies the location of a Stack on a Machine using its type and relative position.
  /// </summary>
  public struct StackLocation
  {
    public PnPStackType StackType;
    public int Position;
  }
}
