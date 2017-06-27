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
  /// <summary>
  /// Represents a Stack on a PnP Machine.
  /// </summary>
  public partial class StackLoader : UserControl
  {
    private bool _bLocked = false;
    //private PnPReel _xSelectedReel;
    private List<PnPReel> _xReelLibrary = new List<PnPReel>();
    //private StackLocation _xLocation;
    private PnPStack _xStack;
    private List<PnPReel> _xActiveReels = new List<PnPReel>();  // to set the "active" symbol automatically
    private PnPStackType _xStackType = PnPStackType.Undefined;
    private int _iLocation;
    private PnPMachine _xMachine;
    private bool _bEmpty; // indicates no reel has been selected in the control
    private const string _EMPTY_STACK = "[none]";

    public StackLoader()
    {
      InitializeComponent();
      
    }

    /// <summary>
    /// A value indicating whether a Reel is currently selected. This field is read only, to clear the current selection use the Clear() method.
    /// </summary>
    public bool IsEmpty
    {
      get{ return _bEmpty; }
    }

    /// <summary>
    /// Gets or sets the Machine on which the selected Stack will be loaded.
    /// </summary>
    public PnPMachine Machine
    {
      get{ return _xMachine; }
      set{ _xMachine = value; }
    }

    /// <summary>
    /// The type of Stack respresented by this control. Setting this value will automatically update the StackType of the Location of any Stack object associated with this control.
    /// </summary>
    public PnPStackType Type
    {
      get{ return _xStackType; }
      set
      {
        _xStackType = value;
        this.lblReelType.Text = AttributeHandler.GetDescription(value);

        if (_xStack != null)
        {
          StackLocation xLoc = new StackLocation();
          xLoc.StackType = value;
          xLoc.Position = _xStack.Location.Position;
          _xStack.Location = xLoc;
        }
      }
    }

    /// <summary>
    /// The position of the Stack represented by this control on the Machine. Setting this value will automatically update the Position of the Location of any Stack object associated with this control.
    /// </summary>
    public int StackPosition
    {
      get{ return _iLocation; }
      set
      {
        if (value <= 0)
          throw new ArgumentOutOfRangeException("The position of this Stack on the Machine must be a positive integer.");

        _iLocation = value;
        this.lblNo.Text = value.ToString();

        if (_xStack != null)
        {
          StackLocation xLoc = new StackLocation();
          xLoc.StackType = _xStack.Location.StackType;
          xLoc.Position = value;
          _xStack.Location = xLoc;
        }        
      }
    }

    /// <summary>
    /// Gets or sets the Reels that are required for the current project. This will automatically control the check icon at the right side of the control.
    /// </summary>
    public List<PnPReel> ActiveReels
    {
      get{ return _xActiveReels; }
      set{ _xActiveReels = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the Reel configuration of this Stack may be changed automatically.
    /// </summary>
    public bool Locked
    {
      get{ return _bLocked; }
      set
      {
        _bLocked = value;
        this.chkLocked.Checked = _bLocked;
        this.comReel.Enabled = !_bLocked;
      }
    }

    /// <summary>
    /// Clears any selection from the control.
    /// </summary>
    public void Clear()
    {
      //_xSelectedReel = new PnPReel();
      this.comReel.SelectedIndex = 0;
      this.chkLocked.Checked = false;
      this.picActive.Visible = false;
      _bLocked = false;
      _bEmpty = true;
    }

    /// <summary>
    /// Gets or sets the Reel that is loaded in the Stack. If the Reel is not yet in the list of available Reels, it will be added.
    /// </summary>
    public PnPReel SelectedReel
    {
      get
      {
        if (_xStack == null || _bEmpty)
          return null;
        else
          return _xStack.Reel;
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException("The Reel to load into the control cannot be null.");

        if (value.GUID.Equals(Guid.Empty))
          throw new ArgumentOutOfRangeException("The Reel to load into the control cannot be undefined.");

        _xStack.LoadReel(value);
        _bEmpty = false;

        // search index of this Reel in the ComboBox list:
        int iSelectedIndex = -1;
        for (int iIndex = 0; iIndex < this.comReel.Items.Count; iIndex++)
        {
          object xCurItem = this.comReel.Items[iIndex];
          if (xCurItem.ToString() == value.ToString())
          {
            iSelectedIndex = iIndex;
            break;
          }
        }
        
        if (iSelectedIndex == -1) // reel not yet in the collection
        {
          this.comReel.Items.Add(value.ToString());
          this.comReel.SelectedIndex = this.comReel.Items.Count - 1; // select last item added
        }
        else
        {
          this.comReel.SelectedIndex = iSelectedIndex;
        }
      }
    }

    /// <summary>
    /// Gets or sets the Reels the user can choose to load.
    /// </summary>
    public List<PnPReel> Reels
    {
      get{ return _xReelLibrary; }
      set
      {
        _xReelLibrary = value;

        // load all matching Reels into the ComboBox:
        this.comReel.Items.Clear();
        this.comReel.Items.Add(_EMPTY_STACK);
        foreach (PnPReel xCurReel in _xReelLibrary)
        {
          if (xCurReel == null)
            throw new ArgumentNullException("Reels cannot be null.");

          if (!xCurReel.GUID.Equals(Guid.Empty))
          {
            bool bSupported = false;
            if (_xMachine == null)
              throw new ArgumentNullException("A PnP Machine must be specified before matching Reels can be displayed.");

            // verify that the supply package of this Reel can be fitted in the Stack:
            foreach (PnPSupplyPackage xCurPackage in _xMachine.GetSupportedPackages(_xStackType))
              if (xCurPackage == xCurReel.SupplyPackage)
              {
                bSupported = true;
                break;
              }

            if (bSupported)
              this.comReel.Items.Add(xCurReel.ToString());
          }
        }
      }
    }

    /// <summary>
    /// Gets or sets the Stack represented by this control.
    /// </summary>
    public PnPStack Stack
    {
      get{ return _xStack; }
      set
      {
        _xStack = value;
        Locked = value.Locked;
        if (!value.Reel.GUID.Equals(Guid.Empty))
          SelectedReel = value.Reel;
        this.lblNo.Text = value.Location.Position.ToString();
        this.lblReelType.Text = AttributeHandler.GetDescription(value.Location.StackType);
      }
    }

    public delegate void SelectedReelChangedEventHandler(object sender, EventArgs e);
    public event SelectedReelChangedEventHandler SelectedReelChanged;
    private void SelectedItemChanged(object sender, EventArgs e)
    {
      if (SelectedReelChanged != null)
        SelectedReelChanged(this, e);

      // search the reel in the list:
      //PnPReel xSelectedReel;
      if (this.comReel.Items[this.comReel.SelectedIndex].ToString() == _EMPTY_STACK)
        //_bEmpty = true;
        Clear();
      else
      {
        _bEmpty = false;
        foreach (PnPReel xCurReel in _xReelLibrary)
          if (xCurReel.ToString() == this.comReel.Items[this.comReel.SelectedIndex].ToString())
          {
            if (_xStack == null)
            {
              StackLocation xLoc = new StackLocation();
              xLoc.Position = _iLocation;
              xLoc.StackType = _xStackType;
              _xStack = new PnPStack(xLoc, 5); // to verify
            }
            _xStack.LoadReel(xCurReel);
            break;
          }

        // display the "active" symbol if the selected Reel is required for the current project:
        bool bActive = false;
        foreach (PnPReel xCurReel in _xActiveReels)
          if (xCurReel.ToString() == this.comReel.Items[this.comReel.SelectedIndex].ToString())
          {
            bActive = true;
            break;
          }          
        this.picActive.Visible = bActive;
      }
    }

    public delegate void LockedStateChangedEventHandler(object sender, EventArgs e);
    public event LockedStateChangedEventHandler LockedStateChanged;
    private void LockedChanged(object sender, EventArgs e)
    {
      if (LockedStateChanged != null) 
        LockedStateChanged(this, e);
      
      Locked = this.chkLocked.Checked;
    }

  }

}
