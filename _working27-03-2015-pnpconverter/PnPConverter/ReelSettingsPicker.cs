using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PnPConverter
{
  public partial class ReelSettingsPicker : UserControl
  {
    private string _sManufacturer, _sSupplier, _sOrderCode;
    private PnPSupplyPackage _xPackage;
    private float _fOffsetX, _fOffsetY, _fSizeX, _fSizeY, _fHeight, _fFeedSpacing;
    private PnPMachineNozzle _xNozzle;
    private int _iRotation, _iSpeed;

    private const string REEL8MM = "Reel, 8 mm", REEL12MM = "Reel, 12 mm", REEL16MM = "Reel, 16 mm", TUBE = "Tube", TRAY = "Tray";
    private string[] PACKAGES = new string[] {REEL8MM, REEL12MM, REEL16MM, TUBE, TRAY};
    private const string VERY_FAST = "Very Fast", FAST = "Fast", NORMAL = "Default", SLOW = "Slow", VERY_SLOW = "Very Slow";
    private string[] SPEEDS = new string[] {VERY_FAST, FAST, NORMAL, SLOW, VERY_SLOW};
    private const string LARGE = "Large", MEDIUM = "Medium", SMALL = "Small", TINY = "Tiny";
    private string[] NOZZLES = new string[] {LARGE, MEDIUM, SMALL, TINY};

    public ReelSettingsPicker()
    {
      InitializeComponent();

      // load values for comboboxes:
      this.comPackageType.Items.Clear();
      foreach (string sItem in PACKAGES)
        this.comPackageType.Items.Add(sItem);

      this.comSpeed.Items.Clear();
      foreach (string sItem in SPEEDS)
        this.comSpeed.Items.Add(sItem);

      this.comNozzle.Items.Clear();
      foreach (string sItem in NOZZLES)
        this.comNozzle.Items.Add(sItem);
      
    }

    /// <summary>
    /// Performs a verification of the input supplied by the user.
    /// </summary>
    /// <returns></returns>
    public bool VerifyInput()
    {
      bool bState = true;

      if (this.txtManufacturer.Text.Trim() == string.Empty)
        bState = false;

      if (this.txtSupplier.Text.Trim() == string.Empty)
        bState = false;

      if (this.comPackageType.SelectedIndex == -1)
        bState = false;

      if (this.comNozzle.SelectedIndex == -1)
        bState = false;

      if (this.comSpeed.SelectedIndex == -1)
        bState = false;

      //int iFS = -1;
      //if (!int.TryParse(this.txtFeedSpacing.Text, out iFS))
      //  bState = false;
      //else
      //  if (iFS <= 0)
      //    bState = false;
      int iFS = (int)this.numFeedSpacing.Value;

      return bState;
    }

    /// <summary>
    /// The manufacturer of this Part.
    /// </summary>
    public string Manufacturer
    {
      get
      {
        return _sManufacturer = this.txtManufacturer.Text;
      }
      set
      {
        this.txtManufacturer.Text = _sManufacturer = value;
      }
    }

    /// <summary>
    /// The supplier of this Part.
    /// </summary>
    public string Supplier
    {
      get
      {        
        return _sSupplier = this.txtSupplier.Text;
      }
      set
      {
        this.txtSupplier.Text = _sSupplier = value;
      }
    }

    /// <summary>
    /// The code identifying a Part to from its Supplier.
    /// </summary>
    public string OrderCode
    {
      get
      {
        return _sOrderCode = this.txtOrderCode.Text;
      }
      set
      {
        this.txtOrderCode.Text = _sOrderCode = value;
      }
    }

    /// <summary>
    /// The container in which the Part is supplied by its manufacturer.
    /// </summary>
    public PnPSupplyPackage Package
    {
      get
      {
        if (this.comPackageType.SelectedIndex > -1)
        {
          switch (this.comPackageType.Items[this.comPackageType.SelectedIndex].ToString())
          {
            case REEL8MM:
              _xPackage = PnPSupplyPackage.Reel8mm;
              break;
            case REEL12MM:
              _xPackage = PnPSupplyPackage.Reel12mm;
              break;
            case REEL16MM:
              _xPackage = PnPSupplyPackage.Reel16mm;
              break;
            case TRAY:
              _xPackage = PnPSupplyPackage.Tray;
              break;
            case TUBE:
              _xPackage = PnPSupplyPackage.Tube;
              break;
          }
        }
        else
        {
          _xPackage = PnPSupplyPackage.Tube; // if no reel info is present, assume a tray or tube
        }

        return _xPackage;
      }
      set
      {
        _xPackage = value;

        switch (_xPackage)
        {
          case PnPSupplyPackage.Reel8mm:
            this.comPackageType.SelectedIndex = this.comPackageType.Items.IndexOf(REEL8MM);
            break;
          case PnPSupplyPackage.Reel12mm:
            this.comPackageType.SelectedIndex = this.comPackageType.Items.IndexOf(REEL12MM);
            break;
          case PnPSupplyPackage.Reel16mm:
            this.comPackageType.SelectedIndex = this.comPackageType.Items.IndexOf(REEL16MM);
            break;
          case PnPSupplyPackage.Tray:
            this.comPackageType.SelectedIndex = this.comPackageType.Items.IndexOf(TRAY);
            break;
          case PnPSupplyPackage.Tube:
            this.comPackageType.SelectedIndex = this.comPackageType.Items.IndexOf(TUBE);
            break;
          case PnPSupplyPackage.Undefined:
            this.comPackageType.SelectedIndex = -1;
            break;
        }
      }
    }

    /// <summary>
    /// Offset in x direction of a Part in its Package container.
    /// </summary>
    public float OffsetX
    {
      get
      {
        //ConfigManager.StringToFloat(this.numOffsetX.Value, out _fOffsetX);
        _fOffsetX = (float)this.numOffsetX.Value;

        return _fOffsetX;
      }
      set
      {
        _fOffsetX = value;
        
        this.numOffsetX.Value = (decimal)_fOffsetX;
      }
    }

    /// <summary>
    /// Offset in y direction of a Part in its Package container.
    /// </summary>
    public float OffsetY
    {
      get
      {
        //ConfigManager.StringToFloat(this.txtOffsetY.Text, out _fOffsetY);
        _fOffsetY = (float)this.numOffsetY.Value;

        return _fOffsetY;
      }
      set
      {
        _fOffsetY = value;

        this.numOffsetY.Value = (decimal)_fOffsetY;
        //this.txtOffsetY.Text = _fOffsetY.ToString();
      }
    }

    /// <summary>
    /// Width of a Part.
    /// </summary>
    public float SizeX
    {
      get
      {
        //ConfigManager.StringToFloat(this.txtSizeX.Text, out _fSizeX);
        _fSizeX = (float)this.numSizeX.Value;

        return _fSizeX;
      }
      set
      {
        _fSizeX = value;

        //this.txtSizeX.Text = _fSizeX.ToString();
        this.numSizeX.Value = (decimal)_fSizeX;
      }
    }

    /// <summary>
    /// Depth of a Part.
    /// </summary>
    public float SizeY
    {
      get
      {
        //ConfigManager.StringToFloat(this.txtSizeY.Text, out _fSizeY);
        _fSizeY = (float)this.numSizeY.Value;

        return _fSizeY;
      }
      set
      {
        _fSizeY = value;

        //this.txtSizeY.Text = _fSizeY.ToString();
        this.numSizeY.Value = (decimal)_fSizeY;
      }
    }

    /// <summary>
    /// Height of a Part.
    /// </summary>
    public float PartHeight
    {
      get
      {
        //ConfigManager.StringToFloat(this.txtHeight.Text, out _fHeight);
        _fHeight = (float)this.numHeight.Value;

        return _fHeight;
      }
      set
      {
        _fHeight = value;

        //this.txtHeight.Text = _fHeight.ToString();
        this.numHeight.Value = (decimal)_fHeight;
      }
    }

    // Center to center distance of two adjecent Parts in their Package container.
    public float FeedSpacing
    {
      get
      {
        //ConfigManager.StringToFloat(this.txtFeedSpacing.Text, out _fFeedSpacing);
        _fFeedSpacing = (float)this.numFeedSpacing.Value;

        return _fFeedSpacing;
      }
      set
      {
        _fFeedSpacing = value;
  
        //this.txtFeedSpacing.Text = _fFeedSpacing.ToString();
        this.numFeedSpacing.Value = (decimal)_fFeedSpacing;
      }
    }

    /// <summary>
    /// The preferred Nozzle to pick up the Part.
    /// </summary>
    public PnPMachineNozzle Nozzle
    {
      get
      {
        if (this.comNozzle.SelectedIndex > -1)
        {
          switch (this.comNozzle.Items[this.comNozzle.SelectedIndex].ToString())
          {
            case LARGE:
              _xNozzle = PnPMachineNozzle.L;
              break;
            case MEDIUM:
              _xNozzle = PnPMachineNozzle.M;
              break;
            case SMALL:
              _xNozzle = PnPMachineNozzle.S;
              break;
            case TINY:
              _xNozzle = PnPMachineNozzle.XS;
              break;
          }
        }
        else
        {
          _xNozzle = PnPMachineNozzle.Undefined;
        }

        return _xNozzle;
      }
      set
      {
        _xNozzle = value;

        switch (_xNozzle)
        {
          case PnPMachineNozzle.L:
            this.comNozzle.SelectedIndex = this.comNozzle.Items.IndexOf(LARGE);
            break;
          case PnPMachineNozzle.M:
            this.comNozzle.SelectedIndex = this.comNozzle.Items.IndexOf(MEDIUM);
            break;
          case PnPMachineNozzle.S:
            this.comNozzle.SelectedIndex = this.comNozzle.Items.IndexOf(SMALL);
            break;
          case PnPMachineNozzle.XS:
            this.comNozzle.SelectedIndex = this.comNozzle.Items.IndexOf(TINY);
            break;
          case PnPMachineNozzle.Undefined:
            this.comNozzle.SelectedIndex = -1;
            break;
        }
      }
    }

    /// <summary>
    /// A Part will be rotated over this angle in degrees before being placed.
    /// </summary>
    public int Rotation
    {
      get
      {
        return _iRotation = (int)(this.numRotation.Value % 360);
      }
      set
      {
        this.numRotation.Value = _iRotation = value;
      }
    }

    /// <summary>
    /// The speed with which to place a Part. Must be a positive non zero percentage (max speed = 100).
    /// </summary>
    public int Speed
    {
      get
      {
        if (this.comSpeed.SelectedIndex > -1)
        {
          switch (this.comSpeed.Items[this.comSpeed.SelectedIndex].ToString())
          {
            case VERY_FAST:
              _iSpeed = 100;
              break;
            case FAST:
              _iSpeed = 80;
              break;
            case NORMAL:
              _iSpeed = 50;
              break;
            case SLOW:
              _iSpeed = 20;
              break;
            case VERY_SLOW:
              _iSpeed = 10;
              break;
          }
        }
        else
        {
          _iSpeed = 100; // set full speed if undefined
        }

        return _iSpeed;
      }
      set
      {
        if (value <= 0 || value > 100)
          throw new ArgumentOutOfRangeException();

        _iSpeed = value;

        if (_iSpeed > 90)
          this.comSpeed.SelectedIndex = this.comSpeed.Items.IndexOf(VERY_FAST);
        else if (_iSpeed < 90 && _iSpeed >= 70)
          this.comSpeed.SelectedIndex = this.comSpeed.Items.IndexOf(FAST);
        else if (_iSpeed < 70 && _iSpeed >= 40)
          this.comSpeed.SelectedIndex = this.comSpeed.Items.IndexOf(MEDIUM);
        else if (_iSpeed < 40 && _iSpeed > 10)
          this.comSpeed.SelectedIndex = this.comSpeed.Items.IndexOf(SLOW);
        else if (_iSpeed <= 10)
          this.comSpeed.SelectedIndex = this.comSpeed.Items.IndexOf(VERY_SLOW);
        else
          this.comSpeed.SelectedIndex = -1;
      }
    }

    private void PackageTypeChanged(object sender, EventArgs e)
    {
      if (this.comPackageType.SelectedItem != null)
        if (this.comPackageType.SelectedItem.ToString() == TUBE || this.comPackageType.SelectedItem.ToString() == TRAY)
        {
          pnlPartSize.Visible = true;
          //pnlOffsets.Location = new Point(0, 140);
        }
        else
        {
          pnlPartSize.Visible = false;
          //pnlOffsets.Location = new Point(0, 100);
        }
    }

  }
}
