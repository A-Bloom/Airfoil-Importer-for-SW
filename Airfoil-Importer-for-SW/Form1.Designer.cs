namespace AirfoilImporterForSW
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.Positioning3D = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.RotationAngleLabel = new System.Windows.Forms.Label();
            this.numTeX = new System.Windows.Forms.NumericUpDown();
            this.TrailingEdgeLabel = new System.Windows.Forms.Label();
            this.numLeX = new System.Windows.Forms.NumericUpDown();
            this.numTeZ = new System.Windows.Forms.NumericUpDown();
            this.XPosLabel = new System.Windows.Forms.Label();
            this.YPosLabel = new System.Windows.Forms.Label();
            this.ZPosLabel = new System.Windows.Forms.Label();
            this.numLeZ = new System.Windows.Forms.NumericUpDown();
            this.LeadingEdgeLabel = new System.Windows.Forms.Label();
            this.numLeY = new System.Windows.Forms.NumericUpDown();
            this.numTeY = new System.Windows.Forms.NumericUpDown();
            this.numTwist = new System.Windows.Forms.NumericUpDown();
            this.AirfoilDataLabel = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.LogoPic = new System.Windows.Forms.PictureBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.txtFeatureName = new System.Windows.Forms.TextBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this.FeatureTypeBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.rdo3DSketch = new System.Windows.Forms.RadioButton();
            this.check2D = new System.Windows.Forms.CheckBox();
            this.rdoCurve = new System.Windows.Forms.RadioButton();
            this.Positioning3D.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTeX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLeX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTeZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLeZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLeY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTeY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTwist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPic)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.FeatureTypeBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFilePath
            // 
            this.txtFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFilePath.Location = new System.Drawing.Point(226, 8);
            this.txtFilePath.Margin = new System.Windows.Forms.Padding(4, 8, 4, 4);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(165, 34);
            this.txtFilePath.TabIndex = 1;
            // 
            // Positioning3D
            // 
            this.Positioning3D.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Positioning3D.AutoSize = true;
            this.Positioning3D.Controls.Add(this.tableLayoutPanel1);
            this.Positioning3D.Location = new System.Drawing.Point(10, 150);
            this.Positioning3D.Margin = new System.Windows.Forms.Padding(10);
            this.Positioning3D.Name = "Positioning3D";
            this.Positioning3D.Padding = new System.Windows.Forms.Padding(4);
            this.Positioning3D.Size = new System.Drawing.Size(481, 267);
            this.Positioning3D.TabIndex = 2;
            this.Positioning3D.TabStop = false;
            this.Positioning3D.Text = "3D Positioning";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23F));
            this.tableLayoutPanel1.Controls.Add(this.RotationAngleLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.numTeX, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.TrailingEdgeLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.numLeX, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.numTeZ, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.XPosLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.YPosLabel, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.ZPosLabel, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.numLeZ, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.LeadingEdgeLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.numLeY, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.numTeY, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.numTwist, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 31);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(473, 232);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // RotationAngleLabel
            // 
            this.RotationAngleLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.RotationAngleLabel.AutoSize = true;
            this.RotationAngleLabel.Location = new System.Drawing.Point(3, 175);
            this.RotationAngleLabel.Name = "RotationAngleLabel";
            this.RotationAngleLabel.Size = new System.Drawing.Size(92, 56);
            this.RotationAngleLabel.TabIndex = 11;
            this.RotationAngleLabel.Text = "Rotation Angle:";
            // 
            // numTeX
            // 
            this.numTeX.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numTeX.AutoSize = true;
            this.numTeX.DecimalPlaces = 4;
            this.numTeX.Location = new System.Drawing.Point(150, 128);
            this.numTeX.Margin = new System.Windows.Forms.Padding(4);
            this.numTeX.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numTeX.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.numTeX.Name = "numTeX";
            this.numTeX.Size = new System.Drawing.Size(100, 34);
            this.numTeX.TabIndex = 6;
            // 
            // TrailingEdgeLabel
            // 
            this.TrailingEdgeLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.TrailingEdgeLabel.AutoSize = true;
            this.TrailingEdgeLabel.Location = new System.Drawing.Point(4, 117);
            this.TrailingEdgeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TrailingEdgeLabel.Name = "TrailingEdgeLabel";
            this.TrailingEdgeLabel.Size = new System.Drawing.Size(129, 56);
            this.TrailingEdgeLabel.TabIndex = 10;
            this.TrailingEdgeLabel.Text = "Trailing Edge Coordinates:";
            // 
            // numLeX
            // 
            this.numLeX.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numLeX.AutoSize = true;
            this.numLeX.DecimalPlaces = 4;
            this.numLeX.Location = new System.Drawing.Point(150, 70);
            this.numLeX.Margin = new System.Windows.Forms.Padding(4);
            this.numLeX.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numLeX.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.numLeX.Name = "numLeX";
            this.numLeX.Size = new System.Drawing.Size(100, 34);
            this.numLeX.TabIndex = 8;
            // 
            // numTeZ
            // 
            this.numTeZ.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numTeZ.AutoSize = true;
            this.numTeZ.DecimalPlaces = 4;
            this.numTeZ.Location = new System.Drawing.Point(366, 128);
            this.numTeZ.Margin = new System.Windows.Forms.Padding(4);
            this.numTeZ.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numTeZ.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.numTeZ.Name = "numTeZ";
            this.numTeZ.Size = new System.Drawing.Size(103, 34);
            this.numTeZ.TabIndex = 9;
            // 
            // XPosLabel
            // 
            this.XPosLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.XPosLabel.AutoSize = true;
            this.XPosLabel.Location = new System.Drawing.Point(188, 15);
            this.XPosLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.XPosLabel.Name = "XPosLabel";
            this.XPosLabel.Size = new System.Drawing.Size(24, 28);
            this.XPosLabel.TabIndex = 2;
            this.XPosLabel.Text = "X";
            // 
            // YPosLabel
            // 
            this.YPosLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.YPosLabel.AutoSize = true;
            this.YPosLabel.Location = new System.Drawing.Point(296, 15);
            this.YPosLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.YPosLabel.Name = "YPosLabel";
            this.YPosLabel.Size = new System.Drawing.Size(23, 28);
            this.YPosLabel.TabIndex = 3;
            this.YPosLabel.Text = "Y";
            // 
            // ZPosLabel
            // 
            this.ZPosLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ZPosLabel.AutoSize = true;
            this.ZPosLabel.Location = new System.Drawing.Point(406, 15);
            this.ZPosLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ZPosLabel.Name = "ZPosLabel";
            this.ZPosLabel.Size = new System.Drawing.Size(23, 28);
            this.ZPosLabel.TabIndex = 4;
            this.ZPosLabel.Text = "Z";
            // 
            // numLeZ
            // 
            this.numLeZ.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numLeZ.AutoSize = true;
            this.numLeZ.DecimalPlaces = 4;
            this.numLeZ.Location = new System.Drawing.Point(366, 70);
            this.numLeZ.Margin = new System.Windows.Forms.Padding(4);
            this.numLeZ.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numLeZ.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.numLeZ.Name = "numLeZ";
            this.numLeZ.Size = new System.Drawing.Size(103, 34);
            this.numLeZ.TabIndex = 5;
            // 
            // LeadingEdgeLabel
            // 
            this.LeadingEdgeLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LeadingEdgeLabel.AutoSize = true;
            this.LeadingEdgeLabel.Location = new System.Drawing.Point(4, 59);
            this.LeadingEdgeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LeadingEdgeLabel.Name = "LeadingEdgeLabel";
            this.LeadingEdgeLabel.Size = new System.Drawing.Size(135, 56);
            this.LeadingEdgeLabel.TabIndex = 1;
            this.LeadingEdgeLabel.Text = "Leading Edge Coordinates:";
            // 
            // numLeY
            // 
            this.numLeY.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numLeY.AutoSize = true;
            this.numLeY.DecimalPlaces = 4;
            this.numLeY.Location = new System.Drawing.Point(258, 70);
            this.numLeY.Margin = new System.Windows.Forms.Padding(4);
            this.numLeY.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numLeY.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.numLeY.Name = "numLeY";
            this.numLeY.Size = new System.Drawing.Size(100, 34);
            this.numLeY.TabIndex = 0;
            // 
            // numTeY
            // 
            this.numTeY.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numTeY.AutoSize = true;
            this.numTeY.DecimalPlaces = 4;
            this.numTeY.Location = new System.Drawing.Point(258, 128);
            this.numTeY.Margin = new System.Windows.Forms.Padding(4);
            this.numTeY.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numTeY.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.numTeY.Name = "numTeY";
            this.numTeY.Size = new System.Drawing.Size(100, 34);
            this.numTeY.TabIndex = 7;
            // 
            // numTwist
            // 
            this.numTwist.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numTwist.AutoSize = true;
            this.numTwist.DecimalPlaces = 4;
            this.numTwist.Location = new System.Drawing.Point(149, 186);
            this.numTwist.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numTwist.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.numTwist.Name = "numTwist";
            this.numTwist.Size = new System.Drawing.Size(102, 34);
            this.numTwist.TabIndex = 12;
            // 
            // AirfoilDataLabel
            // 
            this.AirfoilDataLabel.AutoSize = true;
            this.AirfoilDataLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AirfoilDataLabel.Location = new System.Drawing.Point(4, 0);
            this.AirfoilDataLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.AirfoilDataLabel.Name = "AirfoilDataLabel";
            this.AirfoilDataLabel.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.AirfoilDataLabel.Size = new System.Drawing.Size(214, 44);
            this.AirfoilDataLabel.TabIndex = 3;
            this.AirfoilDataLabel.Text = "Airfoil Data File or URL:";
            this.AirfoilDataLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBrowse.Location = new System.Drawing.Point(399, 4);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(92, 36);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.SystemColors.Control;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.ForeColor = System.Drawing.Color.Lime;
            this.lblStatus.Location = new System.Drawing.Point(353, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(145, 69);
            this.lblStatus.TabIndex = 7;
            // 
            // LogoPic
            // 
            this.LogoPic.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.LogoPic.Image = ((System.Drawing.Image)(resources.GetObject("LogoPic.Image")));
            this.LogoPic.Location = new System.Drawing.Point(136, 3);
            this.LogoPic.MaximumSize = new System.Drawing.Size(222, 80);
            this.LogoPic.MinimumSize = new System.Drawing.Size(222, 80);
            this.LogoPic.Name = "LogoPic";
            this.LogoPic.Size = new System.Drawing.Size(222, 80);
            this.LogoPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.LogoPic.TabIndex = 11;
            this.LogoPic.TabStop = false;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnGenerate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerate.Location = new System.Drawing.Point(154, 7);
            this.btnGenerate.Margin = new System.Windows.Forms.Padding(4);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(192, 58);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "Generate Airfoil";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnGenerate, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblStatus, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(10, 539);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(501, 69);
            this.tableLayoutPanel2.TabIndex = 13;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel3.Controls.Add(this.txtFilePath, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnBrowse, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.AirfoilDataLabel, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 93);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(495, 44);
            this.tableLayoutPanel3.TabIndex = 14;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel4, 0, 4);
            this.tableLayoutPanel5.Controls.Add(this.Positioning3D, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.FeatureTypeBox1, 0, 3);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 5;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(501, 529);
            this.tableLayoutPanel5.TabIndex = 16;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 3;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.LogoPic, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnAbout, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnHelp, 2, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(495, 84);
            this.tableLayoutPanel6.TabIndex = 17;
            // 
            // btnAbout
            // 
            this.btnAbout.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnAbout.Location = new System.Drawing.Point(3, 21);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(97, 42);
            this.btnAbout.TabIndex = 12;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnHelp.Location = new System.Drawing.Point(398, 20);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(94, 44);
            this.btnHelp.TabIndex = 13;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.txtFeatureName, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.NameLabel, 0, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 498);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(495, 28);
            this.tableLayoutPanel4.TabIndex = 15;
            // 
            // txtFeatureName
            // 
            this.txtFeatureName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFeatureName.Location = new System.Drawing.Point(113, 3);
            this.txtFeatureName.Name = "txtFeatureName";
            this.txtFeatureName.Size = new System.Drawing.Size(379, 34);
            this.txtFeatureName.TabIndex = 6;
            // 
            // NameLabel
            // 
            this.NameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(3, 0);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(82, 28);
            this.NameLabel.TabIndex = 5;
            this.NameLabel.Text = "Feature Name:";
            this.NameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FeatureTypeBox1
            // 
            this.FeatureTypeBox1.Controls.Add(this.flowLayoutPanel1);
            this.FeatureTypeBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FeatureTypeBox1.Location = new System.Drawing.Point(3, 430);
            this.FeatureTypeBox1.Name = "FeatureTypeBox1";
            this.FeatureTypeBox1.Size = new System.Drawing.Size(495, 62);
            this.FeatureTypeBox1.TabIndex = 18;
            this.FeatureTypeBox1.TabStop = false;
            this.FeatureTypeBox1.Text = "Feature Type";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.rdo3DSketch);
            this.flowLayoutPanel1.Controls.Add(this.check2D);
            this.flowLayoutPanel1.Controls.Add(this.rdoCurve);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 30);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(489, 29);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // rdo3DSketch
            // 
            this.rdo3DSketch.AutoSize = true;
            this.rdo3DSketch.Location = new System.Drawing.Point(3, 3);
            this.rdo3DSketch.Name = "rdo3DSketch";
            this.rdo3DSketch.Size = new System.Drawing.Size(125, 32);
            this.rdo3DSketch.TabIndex = 0;
            this.rdo3DSketch.TabStop = true;
            this.rdo3DSketch.Text = "3D Sketch";
            this.rdo3DSketch.UseVisualStyleBackColor = true;
            this.rdo3DSketch.CheckedChanged += new System.EventHandler(this.UpdateUIState);
            // 
            // check2D
            // 
            this.check2D.AutoSize = true;
            this.check2D.Location = new System.Drawing.Point(134, 3);
            this.check2D.Name = "check2D";
            this.check2D.Size = new System.Drawing.Size(229, 32);
            this.check2D.TabIndex = 3;
            this.check2D.Text = "Add Plane+2D Sketch";
            this.check2D.UseVisualStyleBackColor = true;
            // 
            // rdoCurve
            // 
            this.rdoCurve.AutoSize = true;
            this.rdoCurve.Location = new System.Drawing.Point(369, 3);
            this.rdoCurve.Name = "rdoCurve";
            this.rdoCurve.Size = new System.Drawing.Size(87, 32);
            this.rdoCurve.TabIndex = 2;
            this.rdoCurve.TabStop = true;
            this.rdoCurve.Text = "Curve";
            this.rdoCurve.UseVisualStyleBackColor = true;
            this.rdoCurve.CheckedChanged += new System.EventHandler(this.UpdateUIState);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(521, 618);
            this.Controls.Add(this.tableLayoutPanel5);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(450, 538);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Airfoil Importer for SW";
            this.Positioning3D.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTeX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLeX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTeZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLeZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLeY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTeY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTwist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPic)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.FeatureTypeBox1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.GroupBox Positioning3D;
        private System.Windows.Forms.NumericUpDown numLeY;
        private System.Windows.Forms.Label AirfoilDataLabel;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label ZPosLabel;
        private System.Windows.Forms.Label YPosLabel;
        private System.Windows.Forms.Label XPosLabel;
        private System.Windows.Forms.Label LeadingEdgeLabel;
        private System.Windows.Forms.NumericUpDown numTeZ;
        private System.Windows.Forms.NumericUpDown numLeX;
        private System.Windows.Forms.NumericUpDown numTeY;
        private System.Windows.Forms.NumericUpDown numTeX;
        private System.Windows.Forms.NumericUpDown numLeZ;
        private System.Windows.Forms.Label TrailingEdgeLabel;
        private System.Windows.Forms.NumericUpDown numTwist;
        private System.Windows.Forms.Label RotationAngleLabel;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox LogoPic;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TextBox txtFeatureName;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.GroupBox FeatureTypeBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton rdo3DSketch;
        private System.Windows.Forms.RadioButton rdoCurve;
        private System.Windows.Forms.CheckBox check2D;
    }
}