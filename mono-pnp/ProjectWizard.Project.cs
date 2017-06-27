using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PnPConverter
{
  public partial class ProjectWizard
  {
    /// <summary>
    /// Attempts to deduct a project name from the PnP file name when Altium's default naming convention is used.
    /// </summary>
    private void LoadProjectName()
    {
      if (_sProjectName == string.Empty)
      {
        const string FN_LEAD = "Pick Place for ";
        if (_xPnPFile.Name.StartsWith(FN_LEAD))
        {
          _sProjectName = _xPnPFile.Name.Substring(FN_LEAD.Length);
          _sProjectName = _sProjectName.Substring(0, _sProjectName.Length - _xPnPFile.Extension.Length);
        }
        else
        {
          _sProjectName = _xPnPFile.Name;
        }
      }
      this.txtProjectName.Text = _sProjectName;

      if (_PCBCount == -1)
        _PCBCount = 1;
      this.numPCBCount.Value = _PCBCount;
    }

  }
}
