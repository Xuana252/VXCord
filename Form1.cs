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
        bool recording = false;



        private void button3_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(!recording)
                button3.BackgroundImage = Properties.Resources.record_button_removebg_preview;
            else
                button3.BackgroundImage = Properties.Resources.record_button_not_press_removebg_preview;
            recording=!recording;
        }
    }
}
