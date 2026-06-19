using System;
using System.Collections.Generic;
using System.Drawing; // Added for changing text color
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SldWorks; // Updated namespaces
// using SwConst; (Removed if not needed to avoid errors)

namespace AirfoilImporterForSW
{
    public partial class Form1 : Form
    {
        // Declare a background timer for the status text
        private Timer statusTimer;

        public Form1()
        {
            InitializeComponent();

            // Set up the 5-second timer when the app launches
            statusTimer = new Timer();
            statusTimer.Interval = 5000; // 5000 milliseconds = 5 seconds
            statusTimer.Tick += StatusTimer_Tick;
        }

        // --- THE STATUS MESSAGE LOGIC ---
        private void ShowStatus(string message, Color color)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = color;

            // Restart the timer so it stays for exactly 5 seconds from the last click
            statusTimer.Stop();
            statusTimer.Start();
        }

        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            lblStatus.Text = ""; // Clear the text
            statusTimer.Stop();  // Stop counting until the next action
        }

        // --- THE BROWSE BUTTON LOGIC ---
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                ofd.Title = "Select Airfoil Coordinates File";

                // If the user selects a file and clicks OK, put the path in the text box
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = ofd.FileName;
                }
            }
        }

        // --- THE GENERATE BUTTON LOGIC ---
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string inputFile = txtFilePath.Text;
            string featureName = txtFeatureName.Text;

            if (!File.Exists(inputFile))
            {
                ShowStatus("Error: Please select a valid airfoil text file.", Color.Red);
                return;
            }

            double[] le = { (double)numLeX.Value, (double)numLeY.Value, (double)numLeZ.Value };
            double[] te = { (double)numTeX.Value, (double)numTeY.Value, (double)numTeZ.Value };
            double twistDeg = (double)numTwist.Value;

            string tempFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp_sw_curve.txt");

            bool mathSuccess = ProcessAirfoilData(inputFile, tempFilePath, le, te, twistDeg);
            if (!mathSuccess) return;

            SldWorks.SldWorks swApp; // Explicitly calling the application object
            try
            {
                swApp = (SldWorks.SldWorks)Marshal.GetActiveObject("SldWorks.Application");
            }
            catch
            {
                ShowStatus("Error: SolidWorks is not running.", Color.Red);
                return;
            }

            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
            if (swModel == null)
            {
                ShowStatus("Error: No active document open in SolidWorks.", Color.Red);
                return;
            }

            try
            {
                swModel.ClearSelection2(true);

                bool existingFound = swModel.Extension.SelectByID2(featureName, "REFERENCECURVES", 0.0, 0.0, 0.0, false, 0, null, 0);
                if (existingFound)
                {
                    swModel.EditDelete();
                }

                swModel.ClearSelection2(true);
                bool success = swModel.InsertCurveFile(tempFilePath);

                if (success)
                {
                    SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
                    if (swSelMgr.GetSelectedObjectCount2(-1) > 0)
                    {
                        Feature newFeature = (Feature)swSelMgr.GetSelectedObject6(1, -1);
                        newFeature.Name = featureName;
                    }
                    else
                    {
                        Feature lastFeature = (Feature)swModel.FeatureByPositionReverse(0);
                        if (lastFeature != null) lastFeature.Name = featureName;
                    }

                    // The satisfying, non-intrusive success flash!
                    ShowStatus($"Airfoil Generated!", Color.Green);
                }
                else
                {
                    ShowStatus("Error: SolidWorks rejected the file import.", Color.Red);
                }
            }
            catch (Exception ex)
            {
                ShowStatus($"COM Error: {ex.Message}", Color.Red);
            }
            finally
            {
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }

        // --- MATH ENGINE REMAINS EXACTLY THE SAME ---
        private bool ProcessAirfoilData(string inputTxt, string outputTxt, double[] le, double[] te, double twistDeg)
        {
            try
            {
                var lines = File.ReadAllLines(inputTxt);
                List<double[]> coords2D = new List<double[]>();

                foreach (var line in lines)
                {
                    var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2 && double.TryParse(parts[0], out double x) && double.TryParse(parts[1], out double y))
                    {
                        coords2D.Add(new double[] { x, y });
                    }
                }

                double[] chordVec = VectorMath.Subtract(te, le);
                double chordLen = VectorMath.Magnitude(chordVec);
                double[] chordDir = VectorMath.Scale(chordVec, 1.0 / chordLen);

                List<double[]> coords3D = new List<double[]>();
                foreach (var pt in coords2D)
                {
                    coords3D.Add(new double[] { pt[0] * chordLen, pt[1] * chordLen, 0.0 });
                }

                double twistRad = twistDeg * (Math.PI / 180.0);
                double cosT = Math.Cos(twistRad);
                double sinT = Math.Sin(twistRad);
                double[,] R_twist = {
                    { 1, 0, 0 },
                    { 0, cosT, -sinT },
                    { 0, sinT, cosT }
                };

                for (int i = 0; i < coords3D.Count; i++)
                {
                    coords3D[i] = VectorMath.Multiply(R_twist, coords3D[i]);
                }

                double[] xAxis = { 1.0, 0.0, 0.0 };
                double[] crossProd = VectorMath.CrossProduct(xAxis, chordDir);
                double sinVal = VectorMath.Magnitude(crossProd);
                double cosVal = VectorMath.DotProduct(xAxis, chordDir);

                double[,] R_align = new double[3, 3] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };

                if (sinVal > 1e-6)
                {
                    double[,] vx = {
                        { 0, -crossProd[2], crossProd[1] },
                        { crossProd[2], 0, -crossProd[0] },
                        { -crossProd[1], crossProd[0], 0 }
                    };
                    double[,] vx2 = VectorMath.Multiply(vx, vx);
                    double scale = (1 - cosVal) / (sinVal * sinVal);

                    for (int i = 0; i < 3; i++)
                        for (int j = 0; j < 3; j++)
                            R_align[i, j] += vx[i, j] + (vx2[i, j] * scale);
                }
                else if (cosVal < 0)
                {
                    R_align = new double[,] { { -1, 0, 0 }, { 0, -1, 0 }, { 0, 0, 1 } };
                }

                using (StreamWriter writer = new StreamWriter(outputTxt))
                {
                    for (int i = 0; i < coords3D.Count; i++)
                    {
                        double[] aligned = VectorMath.Multiply(R_align, coords3D[i]);
                        double[] final = VectorMath.Add(aligned, le);
                        writer.WriteLine($"{final[0]:F6}\t{final[1]:F6}\t{final[2]:F6}");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                ShowStatus($"Math Error: {ex.Message}", Color.Red);
                return false;
            }
        }

        private void numTwist_ValueChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TrailingEdgeLabel_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtFilePath_TextChanged(object sender, EventArgs e)
        {

        }

        private void NameLabel_Click(object sender, EventArgs e)
        {

        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            // Create an instance of your new form
            using (HelpForm helpWindow = new HelpForm())
            {
                helpWindow.ShowDialog();
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            using (AboutForm aboutWindow = new AboutForm())
            {
                aboutWindow.ShowDialog();
            }
        }
    }

    // --- Lightweight Linear Algebra Helper ---
    public static class VectorMath
    {
        public static double[] Add(double[] a, double[] b) => new double[] { a[0] + b[0], a[1] + b[1], a[2] + b[2] };
        public static double[] Subtract(double[] a, double[] b) => new double[] { a[0] - b[0], a[1] - b[1], a[2] - b[2] };
        public static double[] Scale(double[] v, double s) => new double[] { v[0] * s, v[1] * s, v[2] * s };
        public static double DotProduct(double[] a, double[] b) => a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
        public static double Magnitude(double[] v) => Math.Sqrt(DotProduct(v, v));
        public static double[] CrossProduct(double[] a, double[] b) => new double[] {
            a[1] * b[2] - a[2] * b[1],
            a[2] * b[0] - a[0] * b[2],
            a[0] * b[1] - a[1] * b[0]
        };
        public static double[] Multiply(double[,] m, double[] v) => new double[] {
            m[0,0]*v[0] + m[0,1]*v[1] + m[0,2]*v[2],
            m[1,0]*v[0] + m[1,1]*v[1] + m[1,2]*v[2],
            m[2,0]*v[0] + m[2,1]*v[1] + m[2,2]*v[2]
        };
        public static double[,] Multiply(double[,] a, double[,] b)
        {
            double[,] result = new double[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    for (int k = 0; k < 3; k++)
                        result[i, j] += a[i, k] * b[k, j];
            return result;
        }
    }
}