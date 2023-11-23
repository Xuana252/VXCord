using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VsCorder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Start")
            {
                button1.Text = "Stop";
                button1.BackColor = Color.DarkRed;
                button1.ForeColor = Color.Black;
            }
            else
            {
                button1.Text = "Start";
                button1.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                button1.ForeColor = Color.Silver;
            }
        }
    }
}
