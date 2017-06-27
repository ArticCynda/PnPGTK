using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PnPConverter
{
  /// <summary>
  /// Represents the configuration of an embedded board array or panel.
  /// </summary>
  public class PnPPanel
  {
    private float _fRouterWidth, _fBorderX, _fBorderY, _fDistanceX, _fDistanceY, _fWidth, _fHeight;
    private int _iRepeatX, _iRepeatY;
    private bool _bIsPanel = false;

    /// <summary>
    /// A value indicating whether the board is panelized or not.
    /// </summary>
    public bool IsPanel
    {
      get { return _bIsPanel; }
      set { _bIsPanel = value; }
    }

    /// <summary>
    /// The diameter of the tool, typically a contour mill or router, used to cut individual PCBs from the panel.
    /// </summary>
    public float RouterWidth
    {
      get { return _fRouterWidth; }
      set
      {
        if (value <= 0)
          throw new ArgumentException("Tool width must be greater than zero.");

        _fRouterWidth = value;
      }
    }

    /// <summary>
    /// The width of the horizontal panel border in mm, minus the router diameter.
    /// </summary>
    public float BorderX
    {
      get { return _fBorderX; }
      set
      {
        if (value < 0)
          throw new ArgumentException("Border width cannot be a negative value.");

        _fBorderX = value;
      }
    }

    /// <summary>
    /// The width of the vertical panel border in mm, minus the router diameter.
    /// </summary>
    public float BorderY
    {
      get { return _fBorderY; }
      set
      {
        if (value < 0)
          throw new ArgumentException("Border width cannot be a negative value.");

        _fBorderY = value;
      }
    }

    /// <summary>
    /// The number of identical PCBs held by the panel in horizontal direction. Must be equal to or greater than 1.
    /// </summary>
    public int ArrayWidth
    {
      get { return _iRepeatX; }
      set
      {
        if (value < 1)
          throw new ArgumentOutOfRangeException("The panel must contain at least 1 PCB.");

        _iRepeatX = value;
      }
    }

    /// <summary>
    /// The number of identical PCBs held by the panel in vertical direction. Must be equal to or greater than 1.
    /// </summary>
    public int ArrayHeight
    {
      get { return _iRepeatY; }
      set
      {
        if (value < 1)
          throw new ArgumentOutOfRangeException("The panel must contain at least 1 PCB.");

        _iRepeatY = value;
      }
    }

    /// <summary>
    /// The distance between 2 adjecent horizontal router cuts in mm.
    /// </summary>
    public float DistanceX
    {
      get { return _fDistanceX; }
      set
      {
        if (value < 0)
          throw new ArgumentException("Border width cannot be a negative value.");

        _fDistanceX = value;
      }
    }

    /// <summary>
    /// The distance between 2 adjecent vertical router cuts in mm.
    /// </summary>
    public float DistanceY
    {
      get { return _fDistanceY; }
      set
      {
        if (value < 0)
          throw new ArgumentException("Border width cannot be a negative value.");

        _fDistanceY = value;
      }
    }

    /// <summary>
    /// The width of a single PCB.
    /// </summary>
    public float Width
    {
      get { return _fWidth; }
      set
      {
        if (value < 0)
          throw new ArgumentException("PCB must have positive dimensions.");

        _fWidth = value;
      }
    }

    /// <summary>
    /// The height of a single PCB.
    /// </summary>
    public float Height
    {
      get { return _fHeight; }
      set
      {
        if (value < 0)
          throw new ArgumentException("PCB must have positive dimensions.");

        _fHeight = value;
      }
    }

  }
}
