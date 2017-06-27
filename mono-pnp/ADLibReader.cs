using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Globalization;

namespace PnPConverter
{
  /// <summary>
  /// Provides an interface for Altium Designer footprint libraries.
  /// </summary>
  class ADLibReader
  {
    private const string _TEMP_DIR_NAME = "ADlib";
    private const string _TEMP_DIR2_NAME = "ADlib2";

    /// <summary>
    /// Provides a platform independent temporary directory for file extraction. This field is read only.
    /// </summary>
    private static DirectoryInfo TempDir
    {
      get
      {
        string dir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), _TEMP_DIR_NAME);
        return new DirectoryInfo(dir);
      }
    }

    /// <summary>
    /// Provides an alternative platform independent temporary directory for file extraction. This field is read only.
    /// </summary>
    private static DirectoryInfo TempDir2
    {
      get
      {
        string dir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), _TEMP_DIR_NAME);
        return new DirectoryInfo(dir);
      }
    }

    /// <summary>
    /// The directories used for file extraction. These are platform independent and always located in the current user's temporary folder.
    /// </summary>
    public static List<DirectoryInfo> FileCache
    {
      get
      {
        return new List<DirectoryInfo>() {TempDir, TempDir2};
      }
    }

    /// <summary>
    /// Verifies if the specified file is an Altium Designer compatible footprint library.
    /// </summary>
    /// <param name="libFile">The suspected library file.</param>
    /// <returns>A value indicating whether the specified file is a valid Altium Designer compatible footprint library.</returns>
    public static bool IsADLib(FileInfo libFile)
    {
      ExtractLib(libFile, TempDir2);

      // contents extracted, check what's inside
      string HEADERFILE = System.IO.Path.Combine(TempDir2.FullName, "FileHeader");// + "\\FileHeader";
      if (File.Exists(HEADERFILE))
      {
        //FileStream xStream = new FileStream(HEADERFILE, FileAccess.Read);
        StreamReader xReader = new StreamReader(HEADERFILE);
        if (!xReader.EndOfStream)
        {
          string sMagicCode = xReader.ReadLine();
          if (sMagicCode.Trim().ToLower().EndsWith("binary library file"))
          {
            xReader.Close();
            return true;
          }
        }
        xReader.Close();
      }
      return false;
    }

    /// <summary>
    /// Extracts an Altium Designer library to disk.
    /// </summary>
    /// <param name="libFile">The library to extract.</param>
    /// <param name="tempDir">The directory where extracted files and folders should be placed.</param>
    private static void ExtractLib(FileInfo libFile, DirectoryInfo tempDir)
    {
      // delete the existing archive folder here!
      if (tempDir.Exists)
        RemoveTempDir(tempDir);

      //OpenFileDialog xSearch7zip = new OpenFileDialog();
      //xSearch7zip.FileName = "C:\\Program Files\\7-Zip\\7z.exe";
      //xSearch7zip.Title = "Select 7z.exe";
      //xSearch7zip.Filter = "7z.exe|7z.exe";
      //xSearch7zip.ShowDialog();
      //string zippath = xSearch7zip.FileName;
      //string zippath = System.Environment.CurrentDirectory + "\\" + "7za.exe";
      string zippath = "C:\\Program Files\\7-Zip\\7z.exe";

      //OpenFileDialog xSearchLib = new OpenFileDialog();
      //xSearchLib.FileName = "D:\\Dropbox\\PCBtech\\libs\\mylibrary.PcbLib";
      //xSearchLib.Title = "Select PCB Lib";
      //xSearchLib.Filter = "*.PcbLib|*.PcbLib";
      //xSearchLib.ShowDialog();
      //string libbpath = xSearchLib.FileName;
      string libbpath = libFile.FullName;

      string sCurDir = System.Environment.CurrentDirectory;
      //System.Diagnostics.Process zipprocess = System.Diagnostics.Process.Start("%COMSPEC% /c \"" + zippath + "\" x \"" + libbpath + "\" -o\"C:\\Temp\\ADlib\"");
      System.Diagnostics.ProcessStartInfo xZipSettings = new System.Diagnostics.ProcessStartInfo();
      xZipSettings.Arguments = "x \"" + libbpath + "\" -o\"" + tempDir + "\"";
      xZipSettings.FileName = zippath;
      xZipSettings.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
      //System.Diagnostics.Process zipprocess = System.Diagnostics.Process.Start(zippath, "x \"" + libbpath + "\" -o\"C:\\Temp\\ADlib\" > nul"); 
      System.Diagnostics.Process zipprocess = System.Diagnostics.Process.Start(xZipSettings);
      zipprocess.WaitForExit();
    }

    /// <summary>
    /// Reads a collection of footprints from an Altium Designer footprint library file.
    /// </summary>
    /// <param name="libFile">The file containing the footprint data.</param>
    /// <returns>A collection of footprints.</returns>
    public static List<PnPFootprint> GetFootprints(FileInfo libFile)
    {
      ExtractLib(libFile, TempDir);

      //MessageBox.Show("Extraction complete!");

      List<PnPFootprint> xFootprints = new List<PnPFootprint>();
      
      string[] sFootprints = Directory.GetDirectories(TempDir.FullName);   
      foreach (string sFootprint in sFootprints)
      {
        PnPFootprint xCurFootprint = new PnPFootprint();
      
        string sParamFile = sFootprint + "\\Parameters";
        FileStream xStream = new FileStream(sParamFile, FileMode.OpenOrCreate);
        StreamReader xReader = new StreamReader(xStream);
        string sParamString = xReader.ReadToEnd();
        string[] sParams = sParamString.Split(new char[] {'|'});

        int iComplete = 0;
        for (int i = 0; i < sParams.Length; i++)
        {
          
          if (sParams[i].ToUpper().StartsWith("PATTERN="))
          {
            iComplete++;
            xCurFootprint.Name = sParams[i].Substring(8);
          }

          if (sParams[i].ToUpper().StartsWith("HEIGHT="))
          {
            iComplete++;
            string sHeight = sParams[i].Substring(7);    
            if (sHeight.ToLower().EndsWith("mil"))
            {
              string sNumValue = sHeight.Substring(0, sHeight.Length - 3);
              float fValue = 0;
              if (float.TryParse(sNumValue, out fValue))
              {
                CultureInfo xDecPoint = new CultureInfo("en-US");
                fValue = Convert.ToSingle(sNumValue, xDecPoint);
              }
              xCurFootprint.Height = (float)Math.Round(fValue * 0.0254, 2); // conversion from mil to mm
              
            }
            else if (sHeight.ToLower().EndsWith("mm"))
            {
              string sNumValue = sHeight.Substring(0, sHeight.Length -2);
              float fValue = 0;
              if (float.TryParse(sNumValue, out fValue))
              {
                CultureInfo xDecPoint = new CultureInfo("en-US");
                fValue = Convert.ToSingle(sNumValue, xDecPoint);
              }
              xCurFootprint.Height = (float)Math.Round(fValue, 2);
            }
            else
              xCurFootprint.Height = 0;
          }

          if (sParams[i].ToUpper().StartsWith("DESCRIPTION="))
          {
            iComplete++;
            xCurFootprint.Description = sParams[i].Substring(12).Trim();
          }

          if (sParams[i].ToUpper().StartsWith("ITEMGUID="))
            xCurFootprint.ItemGUID = sParams[i].Substring(9).Trim();
 
          if (sParams[i].ToUpper().StartsWith("REVISIONGUID="))
            xCurFootprint.RevisionGUID = sParams[i].Substring(13).Trim();
        }
        if (iComplete == 3)
          xFootprints.Add(xCurFootprint);

        xReader.Close();
        xStream.Close();
      }
      
      // remove the extracted files
      RemoveTempDir(TempDir);

      return xFootprints;
    }

    /// <summary>
    /// Deletes a directory and all of its sub folders and files.
    /// </summary>
    /// <param name="dir">The directory to delete.</param>
    private static void RemoveTempDir(DirectoryInfo dir)
    {
      foreach (FileInfo file in dir.GetFiles())
        File.Delete(file.FullName);
  
      foreach (DirectoryInfo subdir in dir.GetDirectories())
        RemoveTempDir(subdir);

      Directory.Delete(dir.FullName);
    }
  }
}
