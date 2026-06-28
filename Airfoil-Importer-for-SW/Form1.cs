using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SldWorks;
using System.Net.Http;

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

            // Force the UI to evaluate its starting state
            UpdateUIState(null, null);
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


        // --- THE MASTER GENERATOR (ASYNC) ---
        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            string inputSource = txtFilePath.Text;
            string baseName = txtFeatureName.Text;

            if (string.IsNullOrWhiteSpace(baseName))
            {
                ShowStatus("Error: Please enter a Feature Name.", Color.Red);
                return;
            }

            if (string.IsNullOrWhiteSpace(inputSource))
            {
                ShowStatus("Error: Please select a file or paste a URL.", Color.Red);
                return;
            }

            // 1. FETCH DATA (URL vs File)
            string[] fileLines;
            if (Uri.TryCreate(inputSource, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                try
                {
                    ShowStatus("Downloading airfoil data...", Color.Blue);
                    using (HttpClient client = new HttpClient())
                    {
                        string rawText = await client.GetStringAsync(uriResult);
                        fileLines = rawText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    }
                }
                catch (Exception ex)
                {
                    ShowStatus($"Network Error: {ex.Message}", Color.Red);
                    return;
                }
            }
            else
            {
                if (!File.Exists(inputSource))
                {
                    ShowStatus("Error: Could not find the file or invalid URL.", Color.Red);
                    return;
                }
                fileLines = File.ReadAllLines(inputSource);
            }

            // 2. CONNECT TO SOLIDWORKS
            ISldWorks swApp;
            try { swApp = (ISldWorks)Marshal.GetActiveObject("SldWorks.Application"); }
            catch { ShowStatus("Error: SolidWorks is not running.", Color.Red); return; }

            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
            if (swModel == null) { ShowStatus("Error: No active part document.", Color.Red); return; }

            // Document Scale
            double scaleToMeters = 1.0;
            int docUnit = swModel.LengthUnit;
            if (docUnit == 0) scaleToMeters = 0.001;
            else if (docUnit == 1) scaleToMeters = 0.01;
            else if (docUnit == 3) scaleToMeters = 0.0254;
            else if (docUnit == 4) scaleToMeters = 0.3048;

            double[] le = { (double)numLeX.Value * scaleToMeters, (double)numLeY.Value * scaleToMeters, (double)numLeZ.Value * scaleToMeters };
            double[] te = { (double)numTeX.Value * scaleToMeters, (double)numTeY.Value * scaleToMeters, (double)numTeZ.Value * scaleToMeters };
            double twistDeg = (double)numTwist.Value;

            // 3. GENERATE AIRFOIL SPLINE POINTS
            if (!ProcessAirfoilData(fileLines, le, te, twistDeg, out double[] pointData)) return;

            // Calculate Normal Tip for Reference Geometry
            // FIX: Pointing from TE to LE to match the TransformAirfoil math engine!
            double cx = le[0] - te[0];
            double cy = le[1] - te[1];
            double cz = le[2] - te[2];
            double chordLength = Math.Sqrt(cx * cx + cy * cy + cz * cz);
            double refLength = chordLength * 0.25;

            double ux = cx / chordLength;
            double uy = cy / chordLength;
            double uz = cz / chordLength;

            double wx = -uz; double wy = 0.0; double wz = ux;
            double Mw = Math.Sqrt(wx * wx + wz * wz);
            // THE FIX: Default to the X-axis to maintain orthogonality
            if (Mw < 1e-6) { wx = 1; wy = 0; wz = 0; } else { wx /= Mw; wy /= Mw; wz /= Mw; }

            double vx = wy * uz - wz * uy;
            double vy = wz * ux - wx * uz;
            double vz = wx * uy - wy * ux;

            double radians = twistDeg * Math.PI / 180.0;
            double cosT = Math.Cos(radians);
            double sinT = Math.Sin(radians);

            // --- THE FIX: Orthogonal Rigid Body Rotation ---
            // The sine terms MUST have opposite signs to maintain a perfect 90-degree angle!

            // 1. The Spanwise Normal Vector (Uses the minus sign)
            double twistedWx = wx * cosT - vx * sinT;
            double twistedWy = wy * cosT - vy * sinT;
            double twistedWz = wz * cosT - vz * sinT;

            // 2. The Thickness "Up" Vector (Uses the plus sign to match TransformAirfoil)
            double twistedVx = vx * cosT + wx * sinT;
            double twistedVy = vy * cosT + wy * sinT;
            double twistedVz = vz * cosT + wz * sinT;

            try
            {
                // --- INTERCEPT CURVE GENERATION ---
                // --- FREEFORM CURVE GENERATION ---
                if (rdoCurve.Checked)
                {
                    string nameCurve = baseName + " Curve";
                    bool existsCurve = swModel.Extension.SelectByID2(nameCurve, "REFERENCECURVES", 0, 0, 0, false, 0, null, 0);

                    // 1. THE CLEAN WIPE (If Overwriting)
                    if (existsCurve)
                    {
                        swModel.ClearSelection2(true);
                        swModel.Extension.SelectByID2(nameCurve, "REFERENCECURVES", 0, 0, 0, false, 0, null, 0);
                        swModel.EditDelete();
                    }

                    // 2. GENERATE TEMPORARY CURVE FILE
                    // SolidWorks explicitly requires a physical file formatted in X Y Z columns.
                    // FIX: InsertCurveFile assumes Document Units. We must reverse the meter 
                    // scaling here to prevent SolidWorks from double-scaling the import!
                    string tempFilePath = Path.GetTempFileName();
                    using (StreamWriter writer = new StreamWriter(tempFilePath))
                    {
                        for (int i = 0; i < pointData.Length; i += 3)
                        {
                            double unscaledX = pointData[i] / scaleToMeters;
                            double unscaledY = pointData[i + 1] / scaleToMeters;
                            double unscaledZ = pointData[i + 2] / scaleToMeters;

                            // Write coordinates with high precision to prevent surfacing artifacts
                            writer.WriteLine($"{unscaledX:F6} {unscaledY:F6} {unscaledZ:F6}");
                        }
                    }

                    // 3. COMMAND SOLIDWORKS TO BUILD THE CURVE
                    swModel.ClearSelection2(true);
                    bool curveSuccess = swModel.InsertCurveFile(tempFilePath);

                    // 4. CLEAN UP THE TEMP FILE
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
                    }

                    if (!curveSuccess)
                    {
                        ShowStatus("Error: SolidWorks failed to generate the Curve.", Color.Red);
                        return;
                    }

                    // 5. RENAME THE FEATURE
                    Feature featCurve = (Feature)swModel.FeatureByPositionReverse(0);
                    if (featCurve != null)
                    {
                        featCurve.Name = nameCurve;
                    }

                    ShowStatus($"Successfully Generated: {baseName}!", Color.Green);
                    return; // Exit here so it doesn't run the 3D Sketch logic below
                }

                // --- 3D SKETCH & OPTIONAL 2D SKETCH GENERATION ---
                if (rdo3DSketch.Checked)
                {
                    // 1. SETUP NAMES & STATE
                    string name3D = baseName + " 3D";
                    string name2D = baseName + " 2D";
                    string namePlaneA = baseName + " Plane A";
                    string namePlaneB = baseName + " Plane B";

                    bool exists3D = swModel.Extension.SelectByID2(name3D, "SKETCH", 0, 0, 0, false, 0, null, 0);
                    bool exists2D = swModel.Extension.SelectByID2(name2D, "SKETCH", 0, 0, 0, false, 0, null, 0);

                    // 2. FLIP-FLOP PLANE NAMING
                    string oldPlane = "";
                    string newPlane = namePlaneA; // Default to Plane A

                    if (swModel.Extension.SelectByID2(namePlaneA, "PLANE", 0, 0, 0, false, 0, null, 0))
                    {
                        oldPlane = namePlaneA;
                        newPlane = namePlaneB;
                    }
                    else if (swModel.Extension.SelectByID2(namePlaneB, "PLANE", 0, 0, 0, false, 0, null, 0))
                    {
                        oldPlane = namePlaneB;
                        newPlane = namePlaneA;
                    }

                    // Pre-calculate exact XYZ coordinates
                    double normX = le[0] + (twistedWx * refLength);
                    double normY = le[1] + (twistedWy * refLength);
                    double normZ = le[2] + (twistedWz * refLength);

                    double upX = le[0] + (twistedVx * refLength);
                    double upY = le[1] + (twistedVy * refLength);
                    double upZ = le[2] + (twistedVz * refLength);

                    // 3. WIPE EXISTING 3D OR START FRESH
                    if (exists3D)
                    {
                        swModel.ClearSelection2(true);
                        swModel.Extension.SelectByID2(name3D, "SKETCH", 0, 0, 0, false, 0, null, 0);
                        swModel.EditSketch();

                        Sketch active3D = (Sketch)swModel.SketchManager.ActiveSketch;
                        if (active3D != null)
                        {
                            swModel.ClearSelection2(true);

                            // Highlight all segments and points
                            object[] segs = (object[])active3D.GetSketchSegments();
                            if (segs != null) foreach (SketchSegment s in segs) s.Select4(true, null);

                            object[] pts = (object[])active3D.GetSketchPoints2();
                            if (pts != null) foreach (SketchPoint p in pts) p.Select4(true, null);

                            // Wipe everything. (The old plane goes temporarily dangling here).
                            swModel.EditDelete();
                        }
                    }
                    else
                    {
                        swModel.ClearSelection2(true);
                        swModel.SketchManager.Insert3DSketch(true);
                    }

                    // 4. DRAW NEW 3D GEOMETRY
                    swModel.ClearSelection2(true);
                    swModel.SketchManager.CreateSpline((object)pointData);

                    SketchSegment refLine = swModel.SketchManager.CreateLine(le[0], le[1], le[2], normX, normY, normZ);
                    if (refLine != null) refLine.ConstructionGeometry = true;

                    swModel.SketchManager.CreatePoint(le[0], le[1], le[2]); // LE Point
                    swModel.SketchManager.CreatePoint(te[0], te[1], te[2]); // TE Point
                    swModel.SketchManager.CreatePoint(upX, upY, upZ);       // UP Point

                    swModel.SketchManager.Insert3DSketch(true); // Close 3D Sketch

                    // 5. RE-ACQUIRE 3D SKETCH POINTER
                    Feature feat3D = null;
                    if (!exists3D)
                    {
                        feat3D = (Feature)swModel.FeatureByPositionReverse(0);
                        if (feat3D != null) feat3D.Name = name3D;
                    }
                    else
                    {
                        swModel.Extension.SelectByID2(name3D, "SKETCH", 0, 0, 0, false, 0, null, 0);
                        SelectionMgr selMgr = (SelectionMgr)swModel.SelectionManager;
                        feat3D = (Feature)selMgr.GetSelectedObject6(1, -1);
                    }

                    // 6. OPTIONAL 2D SKETCH GENERATION & RE-PARENTING
                    if (check2D.Checked)
                    {
                        swModel.ClearSelection2(true);

                        Sketch swSketch3D = (Sketch)feat3D.GetSpecificFeature2();
                        object[] vPoints = (object[])swSketch3D.GetSketchPoints2();

                        SketchPoint spLE = null, spTE = null, spUp = null;
                        double tol = 1e-4; // Tolerance for floating point matching

                        if (vPoints != null)
                        {
                            foreach (SketchPoint sp in vPoints)
                            {
                                if (Math.Abs(sp.X - le[0]) < tol && Math.Abs(sp.Y - le[1]) < tol && Math.Abs(sp.Z - le[2]) < tol) spLE = sp;
                                else if (Math.Abs(sp.X - te[0]) < tol && Math.Abs(sp.Y - te[1]) < tol && Math.Abs(sp.Z - te[2]) < tol) spTE = sp;
                                else if (Math.Abs(sp.X - upX) < tol && Math.Abs(sp.Y - upY) < tol && Math.Abs(sp.Z - upZ) < tol) spUp = sp;
                            }
                        }

                        if (spLE != null && spTE != null && spUp != null)
                        {
                            SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
                            SelectData mark0 = swSelMgr.CreateSelectData(); mark0.Mark = 0;
                            SelectData mark1 = swSelMgr.CreateSelectData(); mark1.Mark = 1;
                            SelectData mark2 = swSelMgr.CreateSelectData(); mark2.Mark = 2;

                            spLE.Select4(true, mark0);
                            spTE.Select4(true, mark1);
                            spUp.Select4(true, mark2);

                            // Build the NEW Flip-Flop Plane
                            RefPlane customPlane = (RefPlane)swModel.FeatureManager.InsertRefPlane(4, 0.0, 4, 0.0, 4, 0.0);
                            Feature newPlaneFeat = (Feature)customPlane;

                            if (newPlaneFeat != null)
                            {
                                newPlaneFeat.Name = newPlane;

                                if (exists2D)
                                {
                                    // --- THE OVERWRITE WORKFLOW ---

                                    // A. Re-parent existing 2D sketch to the new plane
                                    swModel.ClearSelection2(true);
                                    swModel.Extension.SelectByID2(name2D, "SKETCH", 0, 0, 0, false, 0, null, 0);
                                    newPlaneFeat.Select2(true, 0);
                                    swModel.Extension.ChangeSketchPlane(1, null);

                                    // B. Delete the Old, Dangling Plane
                                    if (!string.IsNullOrEmpty(oldPlane))
                                    {
                                        swModel.ClearSelection2(true);
                                        if (swModel.Extension.SelectByID2(oldPlane, "PLANE", 0, 0, 0, false, 0, null, 0))
                                        {
                                            swModel.EditDelete();
                                        }
                                    }

                                    // C. Open the re-parented 2D sketch and wipe old projected lines
                                    swModel.ClearSelection2(true);
                                    swModel.Extension.SelectByID2(name2D, "SKETCH", 0, 0, 0, false, 0, null, 0);
                                    swModel.EditSketch();

                                    Sketch active2D = (Sketch)swModel.SketchManager.ActiveSketch;
                                    if (active2D != null)
                                    {
                                        object[] segs2D = (object[])active2D.GetSketchSegments();
                                        if (segs2D != null)
                                        {
                                            swModel.ClearSelection2(true);
                                            foreach (SketchSegment seg in segs2D) seg.Select4(true, null);
                                            swModel.EditDelete();
                                        }
                                    }
                                }
                                else
                                {
                                    // --- THE FIRST-TIME WORKFLOW ---
                                    swModel.ClearSelection2(true);
                                    newPlaneFeat.Select2(false, 0);
                                    swModel.SketchManager.InsertSketch(true);
                                }

                                // Convert Entities from the CLEAN 3D Sketch
                                swModel.ClearSelection2(true);
                                swModel.Extension.SelectByID2(name3D, "SKETCH", 0, 0, 0, false, 0, null, 0);
                                swModel.SketchManager.SketchUseEdge3(false, false);

                                swModel.SketchManager.InsertSketch(true); // Close 2D Sketch

                                if (!exists2D)
                                {
                                    Feature feat2D = (Feature)swModel.FeatureByPositionReverse(0);
                                    if (feat2D != null) feat2D.Name = name2D;
                                }

                                // Hide the New Plane to keep viewport clean
                                swModel.ClearSelection2(true);
                                newPlaneFeat.Select2(false, 0);
                                swModel.BlankRefGeom();

                                // Hide the Master 3D Sketch to keep viewport clean
                                if (feat3D != null)
                                {
                                    swModel.ClearSelection2(true);
                                    feat3D.Select2(false, 0);
                                    swModel.BlankSketch();
                                }
                            }
                        }
                        else
                        {
                            ShowStatus("Error: Could not extract reference points for Plane.", Color.Red);
                            return;
                        }
                    }
                }

                ShowStatus($"Successfully Generated: {baseName}!", Color.Green);
            }
            catch (Exception ex)
            {
                ShowStatus($"COM Error: {ex.Message}", Color.Red);
            }

        }

        // --- UPDATED MATH ENGINE (Accepts Text Lines Directly) ---
        private bool ProcessAirfoilData(string[] lines, double[] le, double[] te, double twistDeg, out double[] pointData)
        {
            pointData = null;
            try
            {
                // We removed File.ReadAllLines because the lines are passed in directly now!
                List<double[]> rawPoints = new List<double[]>();

                foreach (var line in lines)
                {
                    var parts = line.Split(new[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length < 2) continue;

                    if (double.TryParse(parts[0], out double x) && double.TryParse(parts[1], out double y))
                    {
                        if (x > 2.0 || x < -1.0 || y > 2.0 || y < -2.0) continue;
                        rawPoints.Add(new double[] { x, y });
                    }
                }

                List<double[]> normalizedPoints = new List<double[]>();
                int splitIndex = -1;

                for (int i = 0; i < rawPoints.Count - 1; i++)
                {
                    if (rawPoints[i][0] > 0.8 && rawPoints[i + 1][0] < 0.2)
                    {
                        splitIndex = i + 1;
                        break;
                    }
                }

                if (splitIndex != -1)
                {
                    for (int i = splitIndex - 1; i >= 0; i--)
                        normalizedPoints.Add(rawPoints[i]);

                    int startLower = (rawPoints[splitIndex][0] < 1e-5) ? 1 : 0;

                    for (int i = splitIndex + startLower; i < rawPoints.Count; i++)
                        normalizedPoints.Add(rawPoints[i]);
                }
                else
                {
                    normalizedPoints = rawPoints;
                }

                // ==========================================
                // --- THE FIX: FORCE CLOSED CONTOUR ---
                // ==========================================
                if (normalizedPoints.Count > 1)
                {
                    double[] firstPt = normalizedPoints[0];
                    double[] lastPt = normalizedPoints[normalizedPoints.Count - 1];

                    double dx = firstPt[0] - lastPt[0];
                    double dy = firstPt[1] - lastPt[1];
                    double gapDistance = Math.Sqrt(dx * dx + dy * dy);

                    // If the gap is larger than a microscopic tolerance, seal it
                    if (gapDistance > 1e-6)
                    {
                        normalizedPoints.Add(new double[] { firstPt[0], firstPt[1] });
                    }
                }
                // ==========================================

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

        private void btnHelp_Click(object sender, EventArgs e)
        {
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

        // --- UI STATE MANAGEMENT ---
        private void UpdateUIState(object sender, EventArgs e)
        {
            if (rdoCurve.Checked)
            {
                check2D.Enabled = false; // Greys it out
                check2D.Checked = false; // Forces it unchecked so background logic doesn't trigger
            }
            else if (rdo3DSketch.Checked)
            {
                check2D.Enabled = true;  // Brings it back
            }
        }
    }
}
