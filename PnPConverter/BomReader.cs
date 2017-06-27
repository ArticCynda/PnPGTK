using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace PnPConverter
{
  /// <summary>
  /// Handles a Bill Of Materials.
  /// </summary>
  public class BomReader
  {
    private const string IO_ERR_MSG = "Unable to read data from the specified file. Please verify it is not opened in any other application and try again.";

    /// <summary>
    /// Parses the specified Bill Of Materials and returns a list of info objects. If the specified file does not exist or is not a valid BOM, an empty list will be returned.
    /// </summary>
    /// <param name="file">The file containing the BOM data.</param>
    /// <returns>A list of info objects.</returns>
    public static List<BomInfo> ReadBomFile(FileInfo file, out DataFileStatus status)
    {
      // don't throw an error if the file doesn't exist, instead return an empty list:
      if (!file.Exists)
      {
        status = DataFileStatus.FileNotFound;
        return new List<BomInfo>();
      }

      DataFileStatus xStatus = DataFileStatus.Undefined;
      List<BomInfo> xBomInfo = new List<BomInfo>();

      try
      {
        FileStream xStream = new FileStream(file.FullName, FileMode.Open);
        StreamReader xReader = new StreamReader(xStream);

        // first read the line with column descriptors to find the interesting columns:
        int iCommentIndex = -1, iDescriptionIndex = -1, iDesignatorIndex = -1, iFootprintIndex = -1, iManufacturerIndex = -1, iOrderCodeIndex = -1, iPackageIndex = -1, iSupplierIndex = -1;

        if (!xReader.EndOfStream)
        {
          string sDescriptorLine = xReader.ReadLine();
          string[] sColumns = sDescriptorLine.Split(new char[] { ',' });
          for (int iColIndex = 0; iColIndex < sColumns.Length; iColIndex++)
          {
            string sCurItem = sColumns[iColIndex].Trim().ToLower();
            switch (sCurItem)
            {
              case "comment":
                iCommentIndex = iColIndex;
                break;
              case "description":
                iDescriptionIndex = iColIndex;
                break;
              case "designator":
                iDesignatorIndex = iColIndex;
                break;
              case "footprint":
                iFootprintIndex = iColIndex;
                break;
              case "manufacturer":
                iManufacturerIndex = iColIndex;
                break;
              case "ordercode":
                iOrderCodeIndex = iColIndex;
                break;
              case "package":
                iPackageIndex = iColIndex;
                break;
              case "supplier":
                iSupplierIndex = iColIndex;
                break;
            }
          }

          // check if a column for footprint and comment has been identified, without these the data is worthless:
          //if ((iCommentIndex + 1) * (iFootprintIndex + 1) != 0)
          if (iCommentIndex > -1 && iFootprintIndex > -1)
          {
            // comment column and footprint column has been found, assume it's a BOM file

            while (!xReader.EndOfStream)
            {
              BomInfo xCurInfo = new BomInfo();
              string sLine = xReader.ReadLine();
              string[] sParams = ParseBomEntry(sLine);
              bool bAllNull = true;
              foreach (string s in sParams)
                if (s != null)
                  bAllNull = false;
              if (sParams.Length == sColumns.Length && !bAllNull) // skip empty or incomplete rows
              {
                xCurInfo.Comment = sParams[iCommentIndex];
                if (iDescriptionIndex > -1) xCurInfo.Description = sParams[iDescriptionIndex];
                if (iDesignatorIndex > -1)
                {
                  string[] sDesignators = sParams[iDesignatorIndex].Split(new char[] { ',' });
                  for (int iDes = 0; iDes < sDesignators.Length; iDes++)
                    sDesignators[iDes] = sDesignators[iDes].Trim(); // to remove spaces around designators
                  xCurInfo.Designators = sDesignators;
                }
                xCurInfo.Footprint = sParams[iFootprintIndex];
                if (iManufacturerIndex > -1) xCurInfo.Manufacturer = sParams[iManufacturerIndex];
                if (iOrderCodeIndex > -1) xCurInfo.OrderCode = sParams[iOrderCodeIndex];
                if (iPackageIndex > -1) xCurInfo.Package = sParams[iPackageIndex];
                if (iSupplierIndex > -1) xCurInfo.Supplier = sParams[iSupplierIndex];
                xBomInfo.Add(xCurInfo);
              }
              else
              {
                // ignore empty lines in the BOM file
                if (sLine.Trim(new char[] {','}) != string.Empty)
                  xStatus = DataFileStatus.MissingData;
              }
            }
            
            // reading completed!
            if (xStatus != DataFileStatus.MissingData)
              xStatus = DataFileStatus.Valid;
          }
          else
          {
            // no commment or footprint columns declared, data in this file is useless
            xStatus = DataFileStatus.MissingColumns; 
          }
        }
        else
        {
          // this file is empty
          xStatus = DataFileStatus.EmptyFile;
        }

        xReader.Close();
        xStream.Close();
      }
      catch (Exception e)
      {
        MessageBox.Show(IO_ERR_MSG, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        xStatus = DataFileStatus.IoError;
      }
      
      status = xStatus;
      return xBomInfo;
    }

    /// <summary>
    /// Properly parse this line using "" to avoid incorrect splits of the entry for multiple designators!
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    private static string[] ParseBomEntry(string entry)
    {
      string[] sValues = new string[] {};
      string sCurItem = string.Empty;
      ReadStatus status = ReadStatus.Idle;
 
      if (entry.Trim().Length == 0)
        return sValues;

      for (int i = 0; i < entry.Length; i++)
      {
        char c = entry[i];

        if (status == ReadStatus.Idle)
        {
          if (c == '\"') // multi item started
          {
            status = ReadStatus.Multi;
          }
          else if (c != ',') // normal item started
          {
            status = ReadStatus.Single;
            sCurItem += c;
          }
          else
          {
            // idle, but another comma encountered => insert empty item into the array
            string[] sTemp = sValues;
            sValues = new string[sValues.Length + 1];
            sTemp.CopyTo(sValues, 0);
          }
        }
        else if (status == ReadStatus.Multi)
        {
          if (c == '\"') // end of multi item
          {
            status = ReadStatus.MultiFinish;

            // enlarge array and store the new item in the last position:
            string[] sTemp = sValues;
            sValues = new string[sValues.Length + 1];
            sTemp.CopyTo(sValues, 0);
            sValues[sValues.Length - 1] = sCurItem;

            sCurItem = string.Empty;
          }
          else
          {
            sCurItem += c;
          }
        }
        else if (status == ReadStatus.MultiFinish)
        {
          status = ReadStatus.Idle;
        }
        else // reading single item
        {
          if (c == ',') // end of item
          {
            status = ReadStatus.Idle;

            // enlarge array and store the new item in the last position:
            string[] sTemp = sValues;
            sValues = new string[sValues.Length + 1];
            sTemp.CopyTo(sValues, 0);
            sValues[sValues.Length - 1] = sCurItem;

            sCurItem = string.Empty;
          }
          else
          {
            sCurItem += c;
          }

        }
      }

      // enlarge array and store the new item in the last position:
      if (sCurItem != string.Empty)
      {
        string[] sT = sValues;
        sValues = new string[sValues.Length + 1];
        sT.CopyTo(sValues, 0);
        sValues[sValues.Length - 1] = sCurItem;
      }
      else if (entry[entry.Length - 1] == ',') // if last character is a comma, then add another empty entry
      {
        string[] sTemp = sValues;
        sValues = new string[sValues.Length + 1];
        sTemp.CopyTo(sValues, 0);
      }

      return sValues;

    }

    /// <summary>
    /// State machine for reading BOM files.
    /// </summary>
    private enum ReadStatus
    {
      Idle,
      Single,
      Multi,
      MultiFinish
    }
  }

  /// <summary>
  /// The status of the currently loaded BOM data.
  /// </summary>
  public enum DataFileStatus
  {
    Valid,
    Undefined,
    FileNotFound,
    IoError,
    EmptyFile,
    MissingData,
    MissingColumns
  }

  /// <summary>
  /// Represents a data entry from a Bill of Materials file.
  /// </summary>
  public struct BomInfo
  {
    public string Comment;
    public string Description;
    public string[] Designators;
    public string Footprint;
    public string Manufacturer;
    public string OrderCode;
    public string Package;
    public string Supplier;
  }
}
