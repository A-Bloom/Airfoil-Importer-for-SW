# Airfoil Importer for SW

## About the Project
Airfoil Importer for SW is a standalone Windows Forms application designed to automate the generation of 3D aerodynamic profiles in SolidWorks.

Bridging the gap between computational aerodynamics and CAD, this tool parses standard 2D airfoil coordinate files, applies spatial transformations (including scaling, translation, and wingtip washout/twist), and leverages the SolidWorks COM API to inject precise 3D sketches directly into the active part document.

### Key Features
Native SolidWorks Integration: Injects 3D sketches directly into the active ModelDoc2 feature tree using the native COM interop dictionaries.

Advanced 3D Positioning: Configure Leading Edge (LE) and Trailing Edge (TE) coordinates anywhere in 3D space.

Parametric Twist: Allows for precise angular twist inputs for immediate washout modeling.

Robust Parsing: Automatically normalizes, scales, and aligns raw .txt and .dat Selig/NACA and Lednicer airfoil coordinate data to the specified chord vector.

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

1. **Prepare SolidWorks:** Ensure a Part document is currently active.
2. **Select Data:** Click 'Browse' to select a local `.txt` or `.dat` file, or paste a direct URL. The tool automatically handles both Selig and Lednicer formats.
3. **Position the Wing:** Enter the 3D coordinates for the Leading Edge and Trailing Edge (uses document units).
4. **Apply Twist:** Enter the aerodynamic rotation angle in degrees (0° keeps the chord perfectly horizontal).
5. **Name the Feature:** Enter a Feature Name. *Note: Using an existing sketch name will parametrically update the existing sketch's coordinates.*
6. **Generate:** Click the button! The tool will draw the 3D continuous spline and include a permanent normal reference line at the leading edge to guide extrusions.