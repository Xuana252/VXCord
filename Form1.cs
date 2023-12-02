using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
        bool folderselected = false;
        bool recording = false;
        string finalVideoName = "finalvideo.mp4";
        string outputPath = "";
        ScrRecorder scrRec = new ScrRecorder(new Rectangle(), "");


        private void button3_Click(object sender, EventArgs e)
        {
            if (!recording)
            {

                if (folderselected)
                {
                    button3.BackgroundImage = Properties.Resources.record_button_removebg_preview;
                    timer1.Start();
                    recording = true;

                }
                else
                {
                    MessageBox.Show("Vui long chon folder de luu video truoc khi quay", "loi");
                }
            }
            else
            {
                button3.BackgroundImage = Properties.Resources.record_button_not_press_removebg_preview;
                pictureBox1.Image = Properties.Resources.default_scr;
                timer1.Stop();
                scrRec.Stop();
                Application.Restart();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "chon folder de luu video";
            
            if(dialog.ShowDialog() == DialogResult.OK )
            {
                outputPath=dialog.SelectedPath;
                textBox2.Text = outputPath;  
                folderselected= true;
                Rectangle bounds = Screen.PrimaryScreen.Bounds;
                scrRec = new ScrRecorder(bounds, outputPath);
            }
            else
            {
                MessageBox.Show("Hay chon folder de luu video","Loi");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            scrRec.RecordVideo();
            FileStream fs = new FileStream(scrRec.tempPath + "//scrshot-" + scrRec.fileCount + ".png", FileMode.Open, FileAccess.Read);
            pictureBox1.Image = Image.FromStream(fs);
            fs.Close();
            scrRec.fileCount++;
            scrRec.RecordAudio();

            label1.Text = scrRec.GetElapsed();
            
        }

    }
}
