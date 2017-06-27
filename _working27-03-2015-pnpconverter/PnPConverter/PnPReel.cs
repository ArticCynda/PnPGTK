using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PnPConverter
{

  /// <summary>
  /// The form in which a Part is supplied by its manufacturer.
  /// </summary>
  public enum PnPSupplyPackage
  {
    Undefined,
    [Description("Reel 8 mm")]
    Reel8mm,
    [Description("Reel 12 mm")]
    Reel12mm,
    [Description("Reel 16 mm")]
    Reel16mm,
    Tube,
    Tray
  }

  /// <summary>
  /// Represents a Reel.
  /// </summary>
  public class PnPReel
  {
    public Guid GUID;
    public string PartNumberOrValue;
    public string Manufacturer;
    public string Supplier;
    public string OrderCode;
    public PnPSupplyPackage SupplyPackage;
    public int Quantity;
    public PnPFootprint Footprint;
    public float FeedSpacing;
    public PnPMachineNozzle Nozzle;
    public float XOffset;
    public float YOffset;
    public int Speed;
    public bool StackAssigned;
    public int Rotation;
    public float XSize; // X and Y size is necessary for calculating the center point of tray and tube parts
    public float YSize;

    const string _REEL_STORAGE = "ReelCollection.dat";
    private static FileInfo _xReelStorage = new FileInfo(_REEL_STORAGE);


    /// <summary>
    /// Reads all previously defined Reels from disk.
    /// </summary>
    /// <returns>A list of previously defined Reels.</returns>
    public static List<PnPReel> LoadReels()
    {
      // if no file exists then the existing storage has been deleted, skip this routine
      if (!_xReelStorage.Exists)
        return new List<PnPReel>();

      FileStream xStream = new FileStream(_xReelStorage.FullName, FileMode.Open);
      StreamReader xReader = new StreamReader(xStream);
      bool bReading = false;
      List<PnPReel> xReelCollection = new List<PnPReel>();
      while (!xReader.EndOfStream)
      {
        string sLine = xReader.ReadLine();

        if (bReading)
        {
          // next section in this file
          if (sLine.StartsWith("["))
          {
            bReading = false;
            xReader.ReadToEnd(); // skip towards end of stream
          }
          else
          {
            bool bReelErrors = false;
            PnPReel xCurReel = DeSerializeReel(sLine, out bReelErrors);

            if (!bReelErrors)
              xReelCollection.Add(xCurReel);
            //}
            //else
            //{
            //  // Discard incomplete reel definitions, do nothing here
            //}
          }
        }

        // if the key for Reels is read, then start reading them in the next iteration
        if (sLine.ToLower().StartsWith("[reels]"))
          bReading = true;

      }
      xReader.Close();
      xReader.Dispose();
      xStream.Close();
      xStream.Dispose();
      return xReelCollection;
    }

    /// <summary>
    /// Stores the specified Reel collection to disk.
    /// </summary>
    /// <param name="Reels">The list of Reels to store.</param>
    public static void SaveReels(List<PnPReel> Reels)
    {
      FileStream xStream = new FileStream(_xReelStorage.FullName, FileMode.Create);
      StreamWriter xWriter = new StreamWriter(xStream);

      xWriter.WriteLine("[Reels]");

      /* There should be 18 properties per Reel:
       * - GUID
       * - PartNumber or Value
       * - Manufacturer
       * - Supplier
       * - OrderCode
       * - SupplyPackage
       * - Part quantity
       * - Footprint name
       * - Footprint description
       * - Footprint ItemGUID
       * - Footprint RevisionGUID
       * - Footprint height
       * - Feed spacing
       * - Nozzle
       * - Placement speed
       * - Offset X
       * - Offset Y
       * - Rotation
      */
      foreach (PnPReel xCurReel in Reels)
      {
        xWriter.WriteLine(SerializeReel(xCurReel));
        //xWriter.Write(xCurReel.GUID.ToString());
        //xWriter.Write("|");

        //xWriter.Write(xCurReel.PartNumberOrValue);
        //xWriter.Write("|");

        //xWriter.Write(xCurReel.Manufacturer);
        //xWriter.Write("|");

        //xWriter.Write(xCurReel.Supplier);
        //xWriter.Write("|");

        //xWriter.Write(xCurReel.OrderCode);
        //xWriter.Write("|");

        //xWriter.Write(xCurReel.SupplyPackage.ToString());
        //xWriter.Write("|");

        //xWriter.Write(xCurReel.Quantity.ToString());
        //xWriter.Write("|");

        //xWriter.Write(xCurReel.Footprint.Name);
        //xWriter.Write("|");

        //xWriter.Write(xCurReel.Footprint.Description);
        //xWriter.Write("|");

        //xWriter.Write(xCurReel.Footprint.ItemGUID);
        //xWriter.Write("|");

        //xWriter.Write(xCurReel.Footprint.RevisionGUID);
        //xWriter.Write("|");

        //xWriter.Write(xCurReel.Footprint.Height.ToString());
        //xWriter.Write("|");

        //xWriter.Write(xCurReel.FeedSpacing.ToString());
        //xWriter.Write("|");

        //xWriter.WriteLine(xCurReel.Nozzle.ToString());
      }

      xWriter.Flush();
      xWriter.Close();
      xWriter.Dispose();
      xStream.Close();
      xStream.Dispose();
    }

    /// <summary>
    /// Converts a Reel object to a text with its properties delimited by '|' characters.
    /// </summary>
    /// <param name="reel">The Reel object to serialize.</param>
    /// <returns>The serialized Reel.</returns>
    public static string SerializeReel(PnPReel reel)
    {
      StringBuilder sReelData = new StringBuilder();

      sReelData.Append(reel.GUID);
      sReelData.Append('|');

      sReelData.Append(reel.PartNumberOrValue);
      sReelData.Append('|');

      sReelData.Append(reel.Manufacturer);
      sReelData.Append('|');

      sReelData.Append(reel.Supplier);
      sReelData.Append('|');

      sReelData.Append(reel.OrderCode);
      sReelData.Append('|');

      sReelData.Append(reel.SupplyPackage);
      sReelData.Append('|');

      sReelData.Append(reel.Quantity);
      sReelData.Append('|');

      sReelData.Append(reel.Footprint.Name);
      sReelData.Append('|');

      sReelData.Append(reel.Footprint.Description);
      sReelData.Append('|');

      sReelData.Append(reel.Footprint.ItemGUID);
      sReelData.Append('|');

      sReelData.Append(reel.Footprint.RevisionGUID);
      sReelData.Append('|');

      sReelData.Append(reel.Footprint.Height);
      sReelData.Append('|');

      sReelData.Append(reel.FeedSpacing);
      sReelData.Append('|');

      sReelData.Append(reel.Nozzle);
      sReelData.Append('|');
      
      sReelData.Append(reel.Speed.ToString());
      sReelData.Append('|');
 
      sReelData.Append(reel.XOffset.ToString());
      sReelData.Append('|');

      sReelData.Append(reel.YOffset.ToString());
      sReelData.Append('|');
  
      sReelData.Append(reel.Rotation.ToString());

      return sReelData.ToString();
    }


    /// <summary>
    /// Deserializes a string, as read from a configuration file, to a Reel object. If the string does not contain reel information or if it is invalid or incomplete, an empty Reel object will be returned.
    /// </summary>
    /// <param name="reelString">The string containing the reel information, separated by '|' characters.</param>
    /// <param name="errors">False if the conversion succeeded, otherwise False.</param>
    /// <returns>The reconstructed Reel object.</returns>
    public static PnPReel DeSerializeReel(string reelString, out bool errors)
    {
      PnPReel xNewReel = new PnPReel();
      string[] sProperties = reelString.Split(new char[] { '|' });

      /* There should be 18 properties per Reel:
       * - GUID
       * - PartNumber or Value
       * - Manufacturer
       * - Supplier
       * - OrderCode
       * - SupplyPackage
       * - Part quantity
       * - Footprint name
       * - Footprint description
       * - Footprint ItemGUID
       * - Footprint RevisionGUID
       * - Footprint height
       * - Feed spacing
       * - Nozzle
       * - Placement speed
       * - Offset X
       * - Offset Y
       * - Rotation
      */
      if (sProperties.Length == 18)
      {
        string sGUID = sProperties[0];
        xNewReel.GUID = new Guid(sGUID);

        string sPartNumber = sProperties[1];
        xNewReel.PartNumberOrValue = sPartNumber;

        string sManufacturer = sProperties[2];
        xNewReel.Manufacturer = sManufacturer;

        string sSupplier = sProperties[3];
        xNewReel.Supplier = sSupplier;

        string sOrderCode = sProperties[4];
        xNewReel.OrderCode = sOrderCode;

        PnPSupplyPackage xPackage = PnPSupplyPackage.Undefined;
        switch (sProperties[5])
        {
          case "Reel8mm":
            xPackage = PnPSupplyPackage.Reel8mm;
            break;
          case "Reel12mm":
            xPackage = PnPSupplyPackage.Reel12mm;
            break;
          case "Reel16mm":
            xPackage = PnPSupplyPackage.Reel16mm;
            break;
          case "Tube":
            xPackage = PnPSupplyPackage.Tube;
            break;
          case "Tray":
            xPackage = PnPSupplyPackage.Tray;
            break;
        }
        xNewReel.SupplyPackage = xPackage;

        int iQuantity = 0;
        int.TryParse(sProperties[6], out iQuantity);
        xNewReel.Quantity = iQuantity;

        PnPFootprint xFootprint = new PnPFootprint();
        xFootprint.Name = sProperties[7];
        xFootprint.Description = sProperties[8];
        xFootprint.ItemGUID = sProperties[9];
        xFootprint.RevisionGUID = sProperties[10];
        string sHeight = sProperties[11];
        float.TryParse(sHeight, out xFootprint.Height);
        xNewReel.Footprint = xFootprint;

        string sFeedSpacing = sProperties[12];
        float fFeedSpacing;
        float.TryParse(sFeedSpacing, out fFeedSpacing);
        xNewReel.FeedSpacing = fFeedSpacing;

        PnPMachineNozzle xNozzle = PnPMachineNozzle.Undefined;
        switch (sProperties[13])
        {
          case "XS":
            xNozzle = PnPMachineNozzle.XS;
            break;
          case "S":
            xNozzle = PnPMachineNozzle.S;
            break;
          case "M":
            xNozzle = PnPMachineNozzle.M;
            break;
          case "L":
            xNozzle = PnPMachineNozzle.L;
            break;
        }
        xNewReel.Nozzle = xNozzle;

        string sSpeed = sProperties[14];
        int iSpeed;
        int.TryParse(sSpeed, out iSpeed);
        xNewReel.Speed = iSpeed;

        string sOffsetX = sProperties[15], sOffsetY = sProperties[16];
        float fOffsetX, fOffsetY;
        float.TryParse(sOffsetX, out fOffsetX);
        float.TryParse(sOffsetY, out fOffsetY);
        xNewReel.XOffset = fOffsetX;
        xNewReel.YOffset = fOffsetY;

        string sRotation = sProperties[17];
        int iRotation = 0;
        int.TryParse(sRotation, out iRotation);
        xNewReel.Rotation = iRotation;

        errors = false;
        return xNewReel;
      }
      else
      {
        errors = true;
        return new PnPReel();
      }
    }

    public override string ToString()
    {
      StringBuilder sReel = new StringBuilder();
      sReel.Append(Footprint.Name);
      sReel.Append(", ");
      sReel.Append(PartNumberOrValue);
      sReel.Append(" [");
      sReel.Append(GUID);
      sReel.Append(']');

      return sReel.ToString();
    }
  }
}
