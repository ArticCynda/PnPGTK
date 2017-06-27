using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PnPConverter
{
  public partial class MainWindow : Form
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      string filename;
      openFileDialog1.ShowDialog();
      filename = openFileDialog1.FileName;

      string[] splits = "hallo, wereld, hoe, gaat, het, er, mee?".Split(new char[] {','});

      bool bIsMetric, bReadErrors;
      List<PnPPart> xPartList = PnPFileReader.ReadPnPFile(new System.IO.FileInfo(filename), out bIsMetric, out bReadErrors);

      MessageBox.Show("File was read successfully!");
    }

    private void button2_Click(object sender, EventArgs e)
    {
      ADLibReader.GetFootprints(new System.IO.FileInfo("C:\\Temp\\Lib"));
    }

    private void StartNewPCBWizard()
    {
      NewProjectWizard wizard = new NewProjectWizard();
      wizard.ShowDialog();

    }

    private void button3_Click(object sender, EventArgs e)
    {
      StartNewPCBWizard();
    }
  }
}
