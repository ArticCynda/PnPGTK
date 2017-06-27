using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PnPConverter
{
  /// <summary>
  /// Handles a Pick and Place parts list.
  /// </summary>
  class PnPFileReader
  {
    /// <summary>
    /// Retrieves a Parst list from a Pick and Place file. If the specified file does not exist, cannot be read or is invalid, an empty list will be returned. Supports Pick and Place files in TXT and CSV formats. Imperial data (mils) will be automatically converted to metric (mm).
    /// </summary>
    /// <param name="file">The file to be read.</param>
    /// <param name="status">Reports any errors that occurred.</param>
    /// <returns>A list of Parts initialized with data from the specified file.</returns>
    public static List<PnPPart> ReadPnPFile(FileInfo file, out DataFileStatus status)
    {
      // check if the file exists before attempting to open it:
      if (!file.Exists)
      {
        status = DataFileStatus.FileNotFound;
        return new List<PnPPart>();
      }

      List<PnPPart> xComponentList = new List<PnPPart>();
      DataFileStatus xStatus = DataFileStatus.Undefined;
      FileStream xStream; 
      StreamReader xReader;
      string[] sParams = new string[11];

      try
      {
        // try to open a stream to the data file:
        xStream = new FileStream(file.FullName, FileMode.Open);
        xReader = new StreamReader(xStream);

        if (!xReader.EndOfStream)
        {

          // first determine the type of file: metric or imperial, csv or txt:
          string sHeader = xReader.ReadLine();
          string CSV_HEADER = "\"Designator\",\"Footprint\",\"Mid X\",\"Mid Y\",\"Ref X\",\"Ref Y\",\"Pad X\",\"Pad Y\",\"Layer\",\"Rotation\",\"Comment\"";
          string TXT_HEADER = "Designator Footprint";
          if (sHeader.Trim().ToLower() == CSV_HEADER.Trim().ToLower())
          {
            // process line in CSV format:
            while (!xReader.EndOfStream)
            {
              string sComponentString = xReader.ReadLine();
              if (sComponentString != string.Empty)
              {
                sComponentString = sComponentString.Trim();

                if (!sComponentString.ToLower().StartsWith("\"designator") && !sComponentString.StartsWith("\"\"") && !sComponentString.Equals(string.Empty))
                {
                  // split the string in separate chunks for every column
                  // note: do NOT use the Split function of a string, because this might cause side effects when the part value contains a comma!
                  int iColCounter = 0;
                  bool bReading = false;
                  for (int iCol = 0; iCol < sParams.Length; iCol++)
                    sParams[iCol] = string.Empty;

                  for (int iPos = 0; iPos < sComponentString.Length; iPos++)
                  {
                    if (bReading)
                    {
                      if (sComponentString[iPos] == '\"')
                      {
                        bReading = false;
                        iColCounter++;
                      }
                      else
                      {
                        sParams[iColCounter] += sComponentString[iPos];
                      }
                    }
                    else
                    {
                      if (sComponentString[iPos] == '\"')
                      {
                        bReading = true;
                      }
                    }
                  }

                  if (iColCounter == 11)
                  {
                    // parse data, create a new Part and add it to the list:
                    DataFileStatus xParseStatus;
                    xComponentList.Add(ParseToPart(sParams, out xParseStatus));
                    if (xParseStatus == DataFileStatus.Valid)
                      xStatus = DataFileStatus.Valid;
                  }
                  else
                  {
                    xStatus = DataFileStatus.MissingColumns;
                  }
                }
              }
            }
          }
          else if (sHeader.Trim().ToLower().StartsWith(TXT_HEADER.Trim().ToLower()))
          {
            // process file in TXT format.
            // figure out how the file is spaced.
            int[,] iColWidth = new int[11, 2];

            // search the start index of "footprint":
            const string FOOTPRINT = "Footprint";
            for (int iPos = 0; iPos < sHeader.Length - FOOTPRINT.Length; iPos++)
            {
              if (sHeader.Substring(iPos, FOOTPRINT.Length).ToLower() == FOOTPRINT.ToLower())
              {
                iColWidth[0,0] = 0; // start index of "designator" column is 0 by default
                iColWidth[0,1] = iPos;
                break;
              }
            }
            
            // search the width of the "footprint" column:
            const string MIDX = "Mid X";
            for (int iPos = 0; iPos < sHeader.Length - MIDX.Length; iPos++)
            {
              if (sHeader.Substring(iPos, MIDX.Length).ToLower() == MIDX.ToLower())
              {
                int iMidXEnd = iPos + MIDX.Length;
                // calculate the start index and width of the "footprint" column:
                iColWidth[1,0] = iColWidth[0,1]; // equals the next position next to the "designator" column
                iColWidth[1,1] = iMidXEnd - 14 - iColWidth[1,0]; // 14 is the default width for coordinate columns

                // calcualte the start index and width of the "Mid X" column:
                iColWidth[2,0] = iColWidth[1,0] + iColWidth[1,1];
                iColWidth[2,1] = 14;
  
                // calculate the start index and width of the "Mid Y" column:
                iColWidth[3,0] = iColWidth[2,0] + iColWidth[2,1];
                iColWidth[3,1] = 14;

                // calculate the start index and width of the "Ref X" column:
                iColWidth[4,0] = iColWidth[3,0] + iColWidth[3,1];
                iColWidth[4,1] = 14;
  
                // calculate the start index and width of the "Ref Y" column:
                iColWidth[5,0] = iColWidth[4,0] + iColWidth[4,1];
                iColWidth[5,1] = 14;

                // calculate the start index and width of the "Pad X" column:
                iColWidth[6,0] = iColWidth[5,0] + iColWidth[5,1];
                iColWidth[6,1] = 14;

                // calculate the start index and width of the "Pad Y" column:
                iColWidth[7,0] = iColWidth[6,0] + iColWidth[6,1];
                iColWidth[7,1] = 14;

                // calculate the start index and width of the "layer" column:
                iColWidth[8,0] = iColWidth[7,0] + iColWidth[7,1];
                iColWidth[8,1] = 3; // always 3

                // calculate the start index and width of the "Rotation" column:
                iColWidth[9,0] = iColWidth[8,0] + iColWidth[8,1];
                iColWidth[9,1] = 14;
        
                // calculate the start index and width of the "Comment" column:
                iColWidth[10,0] = iColWidth[9,0] + iColWidth[9,1];
                iColWidth[10,1] = sHeader.Length - iColWidth[10,0]; // WARNING: this will be different for every row!!

                break;
              }
            }

            // read any remaining lines from the file and parse them into Parts:
            while (!xReader.EndOfStream)
            {
              string sInput = xReader.ReadLine();
              if (sInput != string.Empty) // skip empty rows
              {
                sInput = sInput.Trim();
                
                // check if the row is complete, i.e. that there are at least enough characters to complete all columns until "Rotation":
                if (sInput.Length > iColWidth[9,0] + iColWidth[9,1] + 1) // check this
                {
                  // split the line in chunks for every column and trim white spaces:
                  for (int iCol = 0; iCol < 10; iCol++)
                    sParams[iCol] = sInput.Substring(iColWidth[iCol,0], iColWidth[iCol,1]).Trim();
                  sParams[10] = sInput.Substring(iColWidth[9,0]+ iColWidth[9,1]).Trim();

                  // parse data, create a new Part and add it to the list:
                  DataFileStatus xParseStatus;
                  xComponentList.Add(ParseToPart(sParams, out xParseStatus));
                  if (xParseStatus == DataFileStatus.Valid)
                    xStatus = DataFileStatus.Valid;
                }
                else
                {
                  // row is shorter than header row
                  xStatus = DataFileStatus.MissingData;
                }
              }
            }
          }
          else
          {
            // file isn't in a recognized CSV or TXT format, or has been damaged
            xStatus = DataFileStatus.MissingColumns;
          }
        }
        else
        {
          status = DataFileStatus.EmptyFile;
          return xComponentList;
        }
      }
      catch (IOException)
      {
        status = DataFileStatus.IoError;
        return xComponentList;
      }

      xReader.Close();
      xStream.Close();

      status = xStatus;
      return xComponentList;
    }

    /// <summary>
    /// Parses an array with column data in string format to a Part object.
    /// </summary>
    /// <param name="data">The data to be parsed. Must have 11 columns, otherwise null will be returned.</param>
    /// <param name="status">Reports any errors that may have occurred during parsing.</param>
    /// <returns>A Part object initialized with the specified data.</returns>
    private static PnPPart ParseToPart(string[] data, out DataFileStatus status)
    {
      DataFileStatus xStatus = DataFileStatus.Undefined;
      CultureInfo xDecPoint = new CultureInfo("en-US");
      if (data.Length != 11)
      {
        status = DataFileStatus.MissingColumns;
        return null;
      }

      // copy designator data:
      string sDesignator = data[0];

      // copy footprint data:
      PnPFootprint xFootprint = new PnPFootprint();
      xFootprint.Name = data[1];

      // loop through the coordinate columns and convert them from mil to mm if necessary
      PnPPartCoordinates xCoordinates = new PnPPartCoordinates();

      for (int iCol = 2; iCol < 8; iCol++)
      {
        float fValue = 0;

        // extract numerical data from the string and convert it to the appropriate format:
        if (data[iCol].ToLower().EndsWith("mm"))
        {
          string sNumeric = data[iCol].Substring(0, data[iCol].Length - 2);
          if (float.TryParse(sNumeric, out fValue))
          {
            fValue = Convert.ToSingle(sNumeric, xDecPoint);
          }
          else
          {
            xStatus = DataFileStatus.MissingData;
          }
        }
        else if (data[iCol].ToLower().EndsWith("mil"))
        {
          string sNumeric = data[iCol].Substring(0, data[iCol].Length - 3);
          if (float.TryParse(sNumeric, out fValue))
          {
            fValue = Convert.ToSingle(sNumeric, xDecPoint);
            fValue = (float)(fValue * 0.0254); // coversion from  mil to mm
          }
          else
          {
            xStatus = DataFileStatus.MissingData;
          }
        }
        else
        {
          xStatus = DataFileStatus.MissingData;
        }

        // store data in the coordinates:
        switch (iCol)
        {
          case 2:
            xCoordinates.MidX = fValue;
            break;
          case 3:
            xCoordinates.MidY = fValue;
            break;
          case 4:
            xCoordinates.RefX = fValue;
            break;
          case 5:
            xCoordinates.RefY = fValue;
            break;
          case 6:
            xCoordinates.PadX = fValue;
            break;
          case 7:
            xCoordinates.RefY = fValue;
            break;
        }
      }

      // assign the Part to the appropriate layer:
      PCBLayer xLayer;
      if (data[8].Trim().ToUpper().Equals("T"))
        xLayer = PCBLayer.TopLayer;
      else if (data[8].Trim().ToUpper().Equals("B"))
        xLayer = PCBLayer.BottomLayer;
      else
        xLayer = PCBLayer.Undefined;

      // calculate the correct rotation:
      float fRotation;
      if (!float.TryParse(data[9], out fRotation))
      {
        fRotation = 0;
        xStatus = DataFileStatus.MissingData;
      }
      fRotation = Convert.ToSingle(data[9], xDecPoint);
      fRotation = fRotation % (float)360;

      // and lastly, get the comment, which may equal part number or value:
      string sPartNumberOrValue = data[10];

      // create a new Part and add it to the Part list:
      PnPPart xPart = new PnPPart(
        sDesignator,
        xFootprint,
        sPartNumberOrValue,
        xLayer,
        xCoordinates,
        fRotation
      );

      if (xStatus == DataFileStatus.Undefined)
      {
        // no parsing errors encountered, return valid!
        status = DataFileStatus.Valid;
      }
      else
      {
        // parsing errors encountered, return them to the calling routine
        status = xStatus;
      }

      return xPart;
    }
  }
}
