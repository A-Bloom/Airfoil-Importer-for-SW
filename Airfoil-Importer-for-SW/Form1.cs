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
                // Updated filter to look for .txt and .dat simultaneously
                ofd.Filter = "Airfoil Files (*.txt;*.dat)|*.txt;*.dat|All Files (*.*)|*.*";
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

            // --- VALIDATE FEATURE NAME ---
            if (string.IsNullOrWhiteSpace(featureName))
            {
                ShowStatus("Error: Please enter a name for the feature.", Color.Red);
                return;
            }

            if (!File.Exists(inputFile))
            {
                ShowStatus("Error: Please select a valid airfoil text file.", Color.Red);
                return;
            }

            SldWorks.SldWorks swApp;
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

            // Automatic Unit Scaling
            double scaleToMeters = 1.0;
            int docUnit = swModel.LengthUnit;

            if (docUnit == 0) scaleToMeters = 0.001;
            else if (docUnit == 1) scaleToMeters = 0.01;
            else if (docUnit == 3) scaleToMeters = 0.0254;
            else if (docUnit == 4) scaleToMeters = 0.3048;

            double[] le = {
                (double)numLeX.Value * scaleToMeters,
                (double)numLeY.Value * scaleToMeters,
                (double)numLeZ.Value * scaleToMeters
            };

            double[] te = {
                (double)numTeX.Value * scaleToMeters,
                (double)numTeY.Value * scaleToMeters,
                (double)numTeZ.Value * scaleToMeters
            };

            double twistDeg = (double)numTwist.Value;

            double[] pointData;
            bool mathSuccess = ProcessAirfoilData(inputFile, le, te, twistDeg, out pointData);
            if (!mathSuccess) return;

            try
            {
                swModel.ClearSelection2(true);

                // --- PARAMETRIC SKETCH EDITING ---
                bool sketchExists = swModel.Extension.SelectByID2(featureName, "SKETCH", 0.0, 0.0, 0.0, false, 0, null, 0);

                if (sketchExists)
                {
                    // 1. Enter the existing sketch
                    swModel.EditSketch();

                    // 2. Grab the active sketch and wipe its internal geometry
                    Sketch activeSketch = (Sketch)swModel.SketchManager.ActiveSketch;
                    if (activeSketch != null)
                    {
                        object[] sketchSegments = (object[])activeSketch.GetSketchSegments();
                        if (sketchSegments != null)
                        {
                            swModel.ClearSelection2(true);
                            foreach (SketchSegment seg in sketchSegments)
                            {
                                seg.Select4(true, null); // True means append to current selection
                            }
                            swModel.EditDelete(); // Deletes all selected segments
                        }
                    }
                }
                else
                {
                    // Catch leftover "Normal" sketches from previous testing
                    if (swModel.Extension.SelectByID2(featureName + " Normal", "SKETCH", 0.0, 0.0, 0.0, false, 0, null, 0))
                    {
                        swModel.EditDelete();
                    }

                    swModel.ClearSelection2(true);

                    // 1. Open a brand new 3D Sketch
                    swModel.SketchManager.Insert3DSketch(true);
                }

                swModel.ClearSelection2(true);

                // 2. Inject the array of points directly into a single continuous spline
                object splineFeature = swModel.SketchManager.CreateSpline((object)pointData);

                // --- 3. EXTRUSION REFERENCE LINE (Twisted, 0.25x Chord Length) ---
                double chordX = le[0] - te[0];
                double chordY = le[1] - te[1];
                double chordZ = le[2] - te[2];
                double chordLength = Math.Sqrt(chordX * chordX + chordY * chordY + chordZ * chordZ);
                double refLength = chordLength * 0.25;

                double ux = chordX / chordLength;
                double uy = chordY / chordLength;
                double uz = chordZ / chordLength;

                double wx = -uz;
                double wy = 0.0;
                double wz = ux;
                double Mw = Math.Sqrt(wx * wx + wz * wz);
                if (Mw < 1e-6) { wx = 1; wy = 0; wz = 0; }
                else { wx /= Mw; wy /= Mw; wz /= Mw; }

                double vx = wy * uz - wz * uy;
                double vy = wz * ux - wx * uz;
                double vz = wx * uy - wy * ux;

                double radians = twistDeg * Math.PI / 180.0;
                double cosT = Math.Cos(radians);
                double sinT = Math.Sin(radians);

                double twistedWx = wx * cosT - vx * sinT;
                double twistedWy = wy * cosT - vy * sinT;
                double twistedWz = wz * cosT - vz * sinT;

                SketchSegment refLine = swModel.SketchManager.CreateLine(
                    le[0], le[1], le[2],
                    le[0] + (twistedWx * refLength),
                    le[1] + (twistedWy * refLength),
                    le[2] + (twistedWz * refLength));

                if (refLine != null) refLine.ConstructionGeometry = true;

                // 4. Close the sketch
                swModel.SketchManager.Insert3DSketch(true);

                if (splineFeature != null)
                {
                    // Only rename if it is a brand new sketch
                    if (!sketchExists)
                    {
                        Feature lastFeature = (Feature)swModel.FeatureByPositionReverse(0);
                        if (lastFeature != null) lastFeature.Name = featureName;
                    }

                    ShowStatus(sketchExists ? $"Airfoil Updated!" : $"3D Sketch Generated!", Color.Green);
                }
                else
                {
                    ShowStatus("Error: SolidWorks rejected the spline data.", Color.Red);
                }
            }
            catch (Exception ex)
            {
                ShowStatus($"COM Error: {ex.Message}", Color.Red);
            }
        }


        // --- UPDATED MATH ENGINE (Auto-Detects Selig vs Lednicer) ---
        private bool ProcessAirfoilData(string inputTxt, double[] le, double[] te, double twistDeg, out double[] pointData)
        {
            pointData = null;
            try
            {
                var lines = File.ReadAllLines(inputTxt);

                // Step 1: Extract all valid, raw 2D coordinates into a temporary list
                List<double[]> rawPoints = new List<double[]>();

                foreach (var line in lines)
                {
                    var parts = line.Split(new[] { ' ', '\t',',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length < 2) continue;

                    if (double.TryParse(parts[0], out double x) && double.TryParse(parts[1], out double y))
                    {
                        // HEADER PROTECTION: Ignore metadata point counts (e.g., "60.0  60.0")
                        if (x > 2.0 || x < -1.0 || y > 2.0 || y < -2.0) continue;

                        rawPoints.Add(new double[] { x, y });
                    }
                }

                // Step 2: Detect Format and Normalize to a Continuous Loop (Selig format)
                List<double[]> normalizedPoints = new List<double[]>();
                int splitIndex = -1;

                // Look for the Lednicer "Jump": X is near the tail (>0.8) then drops to the nose (<0.2)
                for (int i = 0; i < rawPoints.Count - 1; i++)
                {
                    if (rawPoints[i][0] > 0.8 && rawPoints[i + 1][0] < 0.2)
                    {
                        splitIndex = i + 1; // This is where the lower surface begins
                        break;
                    }
                }

                if (splitIndex != -1)
                {
                    // It is a Lednicer file! We must stitch it together.
                    // 1. Read the upper surface backward (Tail -> Nose)
                    for (int i = splitIndex - 1; i >= 0; i--)
                    {
                        normalizedPoints.Add(rawPoints[i]);
                    }

                    // 2. Read the lower surface forward (Nose -> Tail)
                    // We skip the very first point of the lower surface if it's (0,0) to prevent a duplicate nose coordinate
                    int startLower = (rawPoints[splitIndex][0] < 1e-5) ? 1 : 0;

                    for (int i = splitIndex + startLower; i < rawPoints.Count; i++)
                    {
                        normalizedPoints.Add(rawPoints[i]);
                    }
                }
                else
                {
                    // No jump detected. It is already a continuous Selig loop.
                    normalizedPoints = rawPoints;
                }

                // Step 3: Run the continuous, normalized loop through the 3D Math Engine
                List<double> final3DPoints = new List<double>();
                foreach (var pt in normalizedPoints)
                {
                    double[] transformedPt = TransformAirfoil(pt[0], pt[1], te, le, twistDeg);

                    final3DPoints.Add(transformedPt[0]);
                    final3DPoints.Add(transformedPt[1]);
                    final3DPoints.Add(transformedPt[2]);
                }

                pointData = final3DPoints.ToArray();
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
