using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H1Chess
{
    /// <summary>
    /// A dialog form for getting the IP address that the user wants to connect to.
    /// </summary>
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        // Holds the result of the dialog aka the IP address
        public string Result { get; set; }

        private void Form2_Load(object sender, EventArgs e)
        {
            Result = "";
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            FinishDialog();
        }

        private void ipTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // If the user presses ENTER it works as well as a mouseclick on connect
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                FinishDialog();
            }
        }

        /// <summary>
        /// Helper function for testing if the user actually entered an IP address.
        /// </summary>
        private void FinishDialog()
        {
            string ip = ipTextbox.Text;

            try
            {
                IPAddress add = IPAddress.Parse(ip);
            } catch (FormatException e)
            {
                MessageBox.Show(this, "The IP address you have entered is not a valid IP address.", "Error", MessageBoxButtons.OK);
                return;
            }
            
            Result = ip;
            Close();
        }
    }
}
