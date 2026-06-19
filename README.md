# Airfoil Importer for SW

## About the Project
Airfoil-Importer-for-SW is a standalone Windows Forms application designed to automate the generation of 3D aerodynamic profiles in SolidWorks.

Bridging the gap between computational aerodynamics and CAD, this tool parses standard 2D airfoil coordinate files, applies spatial transformations (including scaling, translation, and wingtip washout/twist), and leverages the SolidWorks COM API to inject precise reference curves directly into the active part document.

### Key Features
Native SolidWorks Integration: Injects curves directly into the active ModelDoc2 feature tree using the native COM interop dictionaries.

Advanced 3D Positioning: Configure Leading Edge (LE) and Trailing Edge (TE) coordinates anywhere in 3D space.

Parametric Twist: Built-in rotational matrices allow for precise angular twist inputs for immediate washout modeling.

Robust Parsing: Automatically normalizes, scales, and aligns raw .txt airfoil coordinate data to the specified chord vector.

## License & Legal
Distributed under the MIT License. See LICENSE for more information.

Disclaimer: SolidWorks is a registered trademark of Dassault Systèmes. This project is an independent, open-source utility and is not affiliated with, endorsed by, or sponsored by Dassault Systèmes.

## Installation & Setup

Because this tool interacts directly with the SolidWorks COM API, it must be compiled and run on a Windows machine with an active SolidWorks license installed.

1. **Clone the repository:**
```bash
git clone https://github.com/YourUsername/Airfoil-Importer-for-SW.git

```


2. **Compile the executable:**
* Open `Airfoil-Importer-for-SW.sln` in Visual Studio.
* Change the build configuration dropdown at the top of the window from `Debug` to `Release`.
* Go to **Build** > **Build Solution** (or press **Ctrl+Shift+B**).
* Open your Windows File Explorer and navigate to your project folder: `Airfoil-Importer-for-SW\bin\Release\`.
* Inside, you will find the compiled `Airfoil-Importer-for-SW.exe`.

*Note: This is a standalone executable. Once compiled, you can drag this `.exe` file directly to your desktop or taskbar for quick access without needing to open Visual Studio again.*

## Usage Instructions

1. **Launch SolidWorks:** The importer requires an active SolidWorks session to successfully hook into the API. Open SolidWorks and create a new Part Document.
2. **Run the Importer:** Launch the `Airfoil-Importer-for-SW.exe`.
3. **Load Coordinates:** Use the interface to browse for your `.txt` or `.dat` coordinate file.
4. **Generate:** Click the generation button. The tool will parse the data, apply any selected geometric transformations, and loft the curve directly into your active SolidWorks feature tree.

### Formatting Coordinate Files

The parsing engine is designed to handle standard 2D airfoil coordinate data formats. Your `.txt` or `.dat` files must contain clean X and Y coordinates separated by spaces or tabs.

For the most reliable import, remove any textual headers or titles from the top of the file so the first line is strictly numerical data.

**Example Format:**

```text
 1.00000   0.00000
 0.95000   0.01250
 0.90000   0.02500
 ...
 0.00000   0.00000
 ...
 0.90000  -0.01500
 0.95000  -0.00750
 1.00000   0.00000

```

*Tip: Ensure your data points loop continuously from the trailing edge, around the leading edge, and back to the trailing edge to guarantee a fully closed spline profile in SolidWorks.*
