using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PnPConverter
{
  public partial class ProjectWizard
  {
    /// <summary>
    /// Allows the user to select a file with Pick and Place export data.
    /// </summary>
    private void BrowsePnPFile(object sender, EventArgs e)
    {
      OpenFileDialog xBrowsePnPFile = new OpenFileDialog();
      xBrowsePnPFile.Title = "Select Pick and Place file...";
      xBrowsePnPFile.Filter = "Altium Pick and Place File|*.csv|All Files|*.*";
      if (xBrowsePnPFile.ShowDialog() == DialogResult.OK)
      {
        // when OK is clicked, it is already checked that the file does exist.
        this.Cursor = Cursors.WaitCursor;

        this.txtPnPFile.Text = xBrowsePnPFile.FileName;
        _xPnPFile = new FileInfo(xBrowsePnPFile.FileName);
        if (_xPnPFile.Exists)
        {
          _xPnPFilePartsList = PnPFileReader.ReadPnPFile(_xPnPFile, out _xPnPStatus);
        }
        else
        {
          _xPnPStatus = DataFileStatus.FileNotFound;
        }

        CheckPnpBomValidity();
        UpdatePartList();

        this.Cursor = Cursors.Default;
      }
      xBrowsePnPFile.Dispose();
    }

    /// <summary>
    /// Allows the user to select a file with BOM data.
    /// </summary>
    private void BrowseBomFile(object sender, EventArgs e)
    {
      OpenFileDialog xOpenBomFileDialog = new OpenFileDialog();
      xOpenBomFileDialog.Title = "Select Bill of Materials...";
      xOpenBomFileDialog.Filter = "Bill of Materials (*.csv)|*.csv";
      if (xOpenBomFileDialog.ShowDialog() == DialogResult.OK)
      {
        this.Cursor = Cursors.WaitCursor;
        
        this.txtBOMFile.Text = xOpenBomFileDialog.FileName;
        _xBomFile = new FileInfo(xOpenBomFileDialog.FileName);
        if (_xBomFile.Exists)
        {
          _xBomData = BomReader.ReadBomFile(_xBomFile, out _xBomStatus);
        }
        else
        {
          _xBomStatus = DataFileStatus.FileNotFound;
        }

        CheckPnpBomValidity();
        UpdatePartList();

        this.Cursor = Cursors.Default;
      }
      xOpenBomFileDialog.Dispose();
    }

    private void CheckPnpBomValidity()
    {
      // keep the Next button in the wizard disabled as long as the user didn't supply correct data
      this.btnNext.Enabled = (_xPnPStatus == DataFileStatus.Valid);

      // graphically indicate if the PnP data is ok
      if (_xPnPStatus != DataFileStatus.Undefined)
      {
        this.picPnpStatus.Visible = false;
        this.picPnpStatus.Image = GetNOKImage();
        switch (_xPnPStatus)
        {
          case DataFileStatus.EmptyFile:
            this.lblPnpFileMessage.Text = "The specified file is empty.";
            break;
          case DataFileStatus.FileNotFound:
            this.lblPnpFileMessage.Text = "The specified file does not exist.";
            break;
          case DataFileStatus.IoError:
            this.lblPnpFileMessage.Text = "Unable to parse specified file.";
            break;
          case DataFileStatus.MissingColumns:
            this.lblPnpFileMessage.Text = "Unable to parse parts list because required data is missing from the specified file.";
            break;
          case DataFileStatus.MissingData:
            this.lblPnpFileMessage.Text = "Unable to parse parts list because it is incomplete or in the wrong format.";
            break;
          case DataFileStatus.Valid:
            this.lblPnpFileMessage.Text = "Successfully imported data for " + _xPnPFilePartsList.Count.ToString() + " parts.";
            this.picPnpStatus.Image = GetOKImage();
            break;
        }
        this.picPnpStatus.Visible = true;
      }

      // graphically indicate if the BOM data is ok
      if (_xBomStatus != DataFileStatus.Undefined)
      {
        this.picBomStatus.Visible = false;
        this.picBomStatus.Image = GetNOKImage();
        switch (_xBomStatus)
        {
          case DataFileStatus.EmptyFile:
            this.lblBomFileMessage.Text = "The specified file is empty.";
            break;
          case DataFileStatus.FileNotFound:
            this.lblBomFileMessage.Text = "The specified file does not exist.";
            break;
          case DataFileStatus.IoError:
            this.lblBomFileMessage.Text = "Unable to parse specified file.";
            break;
          case DataFileStatus.MissingColumns:
            this.lblBomFileMessage.Text = "Unable to parse BOM because required data is missing from the specified file.";
            break;
          case DataFileStatus.MissingData:
            this.lblBomFileMessage.Text = "Unable to parse BOM because it is incomplete or in the wrong format.";
            break;
          case DataFileStatus.Valid:
            this.lblBomFileMessage.Text = "Successfully imported data for " + _xBomData.Count.ToString() + " different components.";
            this.picBomStatus.Image = GetOKImage();
            break;
        }
        this.picBomStatus.Visible = true;
      }
    }

    private void UpdatePartList()
    {
      // if part data is available, update
      if (_xPnPStatus == DataFileStatus.Valid && _xBomStatus == DataFileStatus.Valid)
      {
        // loop through all parts in the PnP file and match data with the BOM file
        for (int iCurPart = 0; iCurPart < _xPnPFilePartsList.Count; iCurPart++)
        {
          PnPPart xCurPart = _xPnPFilePartsList[iCurPart];
          foreach (BomInfo xCurInfo in _xBomData)
          {
            // check if the designator of the current part appears in this BOM entry:
            for (int iIndex = 0; iIndex < xCurInfo.Designators.Length; iIndex++)
            {
              if (xCurInfo.Designators[iIndex].Trim().ToLower() == xCurPart.Designator.Trim().ToLower())
              {
                // BOM entry matches the current part, check if footprint and comment also match:
                if (xCurInfo.Comment.Trim().ToLower() == xCurPart.PartNumberOrValue.Trim().ToLower() &&
                    xCurInfo.Footprint.Trim().ToLower() == xCurPart.Footprint.Name.Trim().ToLower())
                {
                  // designator, footprint and comment match, so update the other available data:
                  xCurPart.Manufacturer = xCurInfo.Manufacturer;
                  xCurPart.OrderCode = xCurInfo.OrderCode;
                  xCurPart.Supplier = xCurInfo.Supplier;
                  xCurPart.Description = xCurInfo.Description;
                  _xPnPFilePartsList[iCurPart] = xCurPart; // update the parts list
                }
              }
            }
          }
        }
      }

      // clear all items from the table:
      this.listPartsPreview.Items.Clear();

      // summarize the parts in this list:
      foreach (PnPPart xCurPart in _xPnPFilePartsList)
      {
        string[] sPartListParams = new string[4];
        sPartListParams[0] = xCurPart.Designator;
        sPartListParams[1] = xCurPart.Description;
        sPartListParams[2] = xCurPart.PartNumberOrValue;
        sPartListParams[3] = xCurPart.Footprint.Name;
        ListViewItem xPartListEntry = new ListViewItem(sPartListParams);
        this.listPartsPreview.Items.Add(xPartListEntry);
      }

    }
  
  }
}
