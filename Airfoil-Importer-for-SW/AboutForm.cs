using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AirfoilImporterForSW
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Changes the link color to purple so the user knows they clicked it
            linkLabel1.LinkVisited = true;

            // Asks Windows to open the default browser to this exact URL
            System.Diagnostics.Process.Start("https://github.com/YourUsername/SolidworksAirfoilImporter");
        }
    }
}
