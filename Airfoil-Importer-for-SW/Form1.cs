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

        // --- UPDATED MATH ENGINE ---
        private bool ProcessAirfoilData(string inputTxt, string outputTxt, double[] le, double[] te, double twistDeg)
        {
            try
            {
                var lines = File.ReadAllLines(inputTxt);

                // Open the temporary file to write to
                using (StreamWriter writer = new StreamWriter(outputTxt))
                {
                    foreach (var line in lines)
                    {
                        // Clean up spaces/tabs
                        var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        // If it's a valid coordinate pair, transform it and write it immediately
                        if (parts.Length >= 2 && double.TryParse(parts[0], out double x) && double.TryParse(parts[1], out double y))
                        {
                            double[] finalPt = TransformAirfoil(x, y, te, le, twistDeg);
                            writer.WriteLine($"{finalPt[0]:F6}\t{finalPt[1]:F6}\t{finalPt[2]:F6}");
                        }
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

        // --- NEW TRANSFORMATION LOGIC ---
        private double[] TransformAirfoil(double x_data, double y_data, double[] TE, double[] LE, double twistDegrees)
        {
            // 1. Calculate the 3D Chord Vector and its true physical length
            double Cx = LE[0] - TE[0];
            double Cy = LE[1] - TE[1];
            double Cz = LE[2] - TE[2];
            double chordLength = Math.Sqrt(Cx * Cx + Cy * Cy + Cz * Cz);

            // Calculate the Local X axis (Normalized Chord vector)
            double ux = Cx / chordLength;
            double uy = Cy / chordLength;
            double uz = Cz / chordLength;

            // 2. FORCE HORIZONTAL SPAN
            // Reversing the signs here flips the vector 180 degrees,
            // which forces the resulting "Up" cross-product to point toward the sky instead of the ground.
            double wx = -uz;
            double wy = 0.0; // Strictly locked to horizontal
            double wz = ux;

            double Mw = Math.Sqrt(wx * wx + wz * wz);

            if (Mw < 1e-6) { wx = 1; wy = 0; wz = 0; }
            else { wx /= Mw; wy /= Mw; wz /= Mw; }

            // 3. Calculate Local Y axis (The "Up" Thickness Vector)
            double vx = wy * uz - wz * uy;
            double vy = wz * ux - wx * uz;
            double vz = wx * uy - wy * ux;

            // 4. Apply User Washout (Twist)
            double radians = twistDegrees * Math.PI / 180.0;
            double cosT = Math.Cos(radians);
            double sinT = Math.Sin(radians);

            double twistedVx = vx * cosT + wx * sinT;
            double twistedVy = vy * cosT + wy * sinT;
            double twistedVz = vz * cosT + wz * sinT;

            // 5. Map the 2D data to the new 3D system
            // Standard airfoil text files define the nose (LE) at X=0 and the tail (TE) at X=1.
            // Our 3D chord vector points from TE to LE, so we invert the parametric mapping:
            // When x_data is 0 (Nose) -> mappingFactor is 1.0 (Places it at LE)
            // When x_data is 1 (Tail) -> mappingFactor is 0.0 (Places it at TE)
            double mappingFactor = 1.0 - x_data;

            double finalX = TE[0] + (Cx * mappingFactor);
            double finalY = TE[1] + (Cy * mappingFactor);
            double finalZ = TE[2] + (Cz * mappingFactor);

            // Extrude outward using the twisted Up vector, scaled by actual chord length
            finalX += twistedVx * (y_data * chordLength);
            finalY += twistedVy * (y_data * chordLength);
            finalZ += twistedVz * (y_data * chordLength);

            return new double[] { finalX, finalY, finalZ };
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

}
