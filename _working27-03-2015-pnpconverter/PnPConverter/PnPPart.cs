using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PnPConverter
{
  /// <summary>
  /// Represents the Footprint of a Part.
  /// </summary>
  public struct PnPFootprint
  {
    public float Height;
    public string Name;
    public string Description;
    public string ItemGUID;
    public string RevisionGUID;
  }

  /// <summary>
  /// Represents the Coordinates at which a Part is placed on the PCB.
  /// </summary>
  public struct PnPPartCoordinates
  {
    public float MidX;
    public float MidY;
    public float RefX;
    public float RefY;
    public float PadX;
    public float PadY;
  }

  /// <summary>
  /// Represents a Part to be placed on the PCB.
  /// </summary>
  public class PnPPart
  {
    private string _sDesignator, _sPartNumber;
    private PCBLayer _xLayer;
    private float _fRotation;
    private bool _bExcluded = false, _bHasReel = false;
    private ExclusionReason _xExclusion = ExclusionReason.None;
    private PnPFootprint _xFootprint = new PnPFootprint();
    private PnPPartCoordinates _xCoordinates = new PnPPartCoordinates();

    // data merged from BOM
    private string _sManufacturer;
    private string _sOrderCode;
    private string _sSupplier;
    private string _sDescription;

    // experimental
    private Guid _xAssignedReel = Guid.Empty;

    /// <summary>
    /// Creates a new Part.
    /// </summary>
    /// <param name="designator">The designator of the Part, i.e. IC15, K6, C12 etc.</param>
    /// <param name="footprint">The footprint of the Part.</param>
    /// <param name="partNumberOrValue">The ID of the Part as provided by the manufacturer, or the value for capacitors, resistors, fuses etc. Must provide enough information to uniquely identify a Part in combination with its Footprint.</param>
    /// <param name="layer">The side of the PCB this Part must be placed on.</param>
    /// <param name="coordinates">The location on the PCB where the Part must be placed.</param>
    /// <param name="rotation">The orientation of the Part in degrees.</param>
    public PnPPart(
      string designator,
      PnPFootprint footprint,
      string partNumberOrValue,
      PCBLayer layer,
      PnPPartCoordinates coordinates,
      float rotation)
    {
      if (designator.Trim().Equals(string.Empty))
        throw new ArgumentNullException("A unique designator must be assigned to the Component.");

      if (footprint.Name.Trim().Equals(string.Empty))
        throw new ArgumentNullException("Every Part must have a Footprint.");

      if (footprint.Height < 0)
        throw new ArgumentOutOfRangeException("The height of a component should be a positive value.");

      if (partNumberOrValue.Trim().Equals(string.Empty))
        throw new ArgumentNullException("The Part must have a part number or value assigned.");

      _sDesignator = designator;
      _xFootprint = footprint;
      _xFootprint.Height = (float)Math.Round(_xFootprint.Height, 2);
      _sPartNumber = partNumberOrValue;
      _xLayer = layer;
      _xCoordinates = coordinates;
      _fRotation = rotation % (float)360;
     }

    /// <summary>
    /// The GUID of the Reel this Part that is sourcing this Part.
    /// </summary>
    public Guid AssignedReel
    {
      get{ return _xAssignedReel; }
      set{ _xAssignedReel = value; }
    }

    /// <summary>
    /// Gets or sets the reason why a Part will not be placed on the PCB.
    /// </summary>
    public ExclusionReason ExclusionReason
    {
      get{ return _xExclusion; }
      set{ _xExclusion = value; }
    }

    /// <summary>
    /// Gets or sets the Footprint of the Part.
    /// </summary>
    public PnPFootprint Footprint
    {
      get{ return _xFootprint; }
      set
      {
        if (value.Name.Trim().Equals(string.Empty))
          throw new ArgumentNullException("Every Component must have a Footprint.");

        if (value.Height < 0)
          throw new ArgumentOutOfRangeException("The height of a component should be a positive value.");

        _xFootprint = value;
      }
    }

    /// <summary>
    /// Indicates if a Reel is available for this part.
    /// </summary>
    public bool HasReel
    {
      get{ return _bHasReel; }
      set{ _bHasReel = value; }
    }

    /// <summary>
    /// The designator of a part, i.e. IC15, K6, C12 etc.
    /// </summary>
    public string Designator
    {
      get{ return _sDesignator; }
      set
      {
        if (Designator.Trim().Equals(string.Empty))
         throw new ArgumentNullException("A unique designator must be assigned to the Component.");
        else
          _sDesignator = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the Part must be placed on the PCB. Setting this value will automatically set Exclusion to UserDefined, resetting it will set Exclusion to None.
    /// </summary>
    public bool Excluded
    {
      get{ return _bExcluded; }
      set
      {
        if (value)
          _xExclusion = ExclusionReason.UserDefined;
        else
          _xExclusion = ExclusionReason.None;
        _bExcluded = value;
      }
    }

    /// <summary>
    /// Gets or sets the ID of the Part as provided by the manufacturer, or the value for capacitors, resistors, fuses etc. Must provide enough information to uniquely identify a Part in combination with its Footprint.
    /// </summary>
    public string PartNumberOrValue
    {
      get{ return _sPartNumber; }
      set
      {
        if (value.Trim().Equals(string.Empty))
          throw new ArgumentNullException("The Part must have a part number or value assigned.");
        _sPartNumber = value; }
    }

    /// <summary>
    /// Gets or sets the side of the PCB this Part must be placed on.
    /// </summary>
    public PCBLayer Layer
    {
      get{ return _xLayer; }
      set{ _xLayer = value; }
    }

    /// <summary>
    /// Gets or sets the location on the PCB where the Part must be placed.
    /// </summary>
    public PnPPartCoordinates Coordinates
    {
      get{ return _xCoordinates; }
      set{ _xCoordinates = value; }
    }

    /// <summary>
    /// Gets or sets the orientation of the Part in degrees.
    /// </summary>
    public float Rotation
    {
      get{ return _fRotation; }
      set{ _fRotation = value % (float)360; }
    }

    /// <summary>
    /// Gets or sets a description for this Part.
    /// </summary>
    public string Description
    {
      get{ return _sDescription; }
      set{ _sDescription = value; }
    }

    /// <summary>
    /// Gets or sets the manufacturer of this Part.
    /// </summary>
    public string Manufacturer
    {
      get{ return _sManufacturer; }
      set{ _sManufacturer = value; }
    }
    
    /// <summary>
    /// Gets or sets the order code for this Part. Only useful if Supplier is set correctly.
    /// </summary>
    public string OrderCode
    {
      get{ return _sOrderCode; }
      set{ _sOrderCode = value; }  
    }

    /// <summary>
    /// Gets or sets the supplier of the Part.
    /// </summary>
    public string Supplier
    {
      get{ return _sSupplier; }
      set{ _sSupplier = value;}
    }
  }

  /// <summary>
  /// The side of the PCB on which a Part must be placed.
  /// </summary>
  public enum PCBLayer
  {
    Undefined,
    TopLayer,
    BottomLayer
  };

  /// <summary>
  /// The reason why a Part will not be placed on the PCB.
  /// </summary>
  public enum ExclusionReason
  {
    None,
    NotPhysical,
    NotSmd,
    Mechanical,
    UserDefined,
    TooHigh,
    NoReelAssigned
  };
}
