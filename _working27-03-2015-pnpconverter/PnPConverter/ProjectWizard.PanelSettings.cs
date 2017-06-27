using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PnPConverter
{
  public partial class ProjectWizard : Form
  {

    /// <summary>
    /// Updates visibility of user control elements based on the selected panelization option.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PanelConfigurationChanged(object sender, EventArgs e)
    {
      this.numContourWidth.Enabled = this.rdbPanel.Checked;
      this.numPanelBorderX.Enabled = this.rdbPanel.Checked;
      this.numPanelBorderY.Enabled = this.rdbPanel.Checked;
      this.numPanelXCount.Enabled = this.rdbPanel.Checked;
      this.numPanelYCount.Enabled = this.rdbPanel.Checked;
      this.numPanelDistanceX.Enabled = this.rdbPanel.Checked;
      this.numPanelDistanceY.Enabled = this.rdbPanel.Checked;
      this.numPCBWidth.Enabled = this.rdbPanel.Checked;
      this.numPCBHeight.Enabled = this.rdbPanel.Checked;
    }

    /// <summary>
    /// Saves the panelization configuration.
    /// </summary>
    private void SavePanelConfiguration()
    {
      _xPanelConfiguration.ArrayHeight = (int)this.numPanelYCount.Value;
      _xPanelConfiguration.ArrayWidth = (int)this.numPanelXCount.Value;
      _xPanelConfiguration.BorderX = (float)this.numPanelBorderX.Value;
      _xPanelConfiguration.BorderY = (float)this.numPanelBorderY.Value;
      _xPanelConfiguration.DistanceX = (float)this.numPanelDistanceX.Value;
      _xPanelConfiguration.DistanceY = (float)this.numPanelDistanceY.Value;
      _xPanelConfiguration.Width = (float)this.numPCBWidth.Value;
      _xPanelConfiguration.Height = (float)this.numPCBHeight.Value;
      _xPanelConfiguration.IsPanel = this.rdbPanel.Checked;
    }

    /// <summary>
    /// Initialize the panel configuration
    /// </summary>
    private void InitPanel()
    {
      _xPanelConfiguration = new PnPPanel();
      PanelConfigurationChanged(null, null);
    }
  }
}
