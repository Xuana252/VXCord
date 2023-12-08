using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VsCorder
{
    public partial class Form1 : Form
    {
        bool recording = false;
        string VXPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + "\\VXCord";
        static string VDoutputPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)+ "\\VXCord\\Video";
        static string IMoutputPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)+ "\\VXCord\\Picture";
        int CountIM = Directory.GetFiles(IMoutputPath, "*", SearchOption.TopDirectoryOnly).Length;
        int CountVD = Directory.GetFiles(VDoutputPath, "*", SearchOption.TopDirectoryOnly).Length;

        ScrRecorder scrRec = new ScrRecorder(Screen.PrimaryScreen.Bounds,VDoutputPath );

        public Form1()
        {
            InitializeComponent();
            string path=Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            SetFolder();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!recording)
            {
                    if(textBox1.Text!="")
                        scrRec.finalName = textBox1.Text + ".mp4";
                    button3.BackgroundImage = Properties.Resources.record_button_removebg_preview;
                    timer1.Start();

                    recording = true;
            }
            else
            {
                scrRec.finalName = "Video" + CountVD + ".mp4";
                button3.BackgroundImage = Properties.Resources.record_button_not_press_removebg_preview;
                pictureBox1.Image = Properties.Resources.default_scr;
                timer1.Stop();
                scrRec.Stop();
                label1.Text = scrRec.GetElapsed();
                recording = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "chon folder de luu video";
            
            if(dialog.ShowDialog() == DialogResult.OK )
            {
                VDoutputPath=dialog.SelectedPath;
                textBox2.Text = VDoutputPath;
                Rectangle bounds = Screen.PrimaryScreen.Bounds;
                scrRec = new ScrRecorder(bounds, VDoutputPath);
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

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox1.Text=="Dark")
            {
                textBox1.BackColor = Color.Gray;
                textBox2.BackColor = Color.Gray;
                textBox3.BackColor = Color.Gray;
                comboBox1.BackColor = Color.Gray;
                button1.BackColor = Color.Gray;
                button2.BackColor = Color.Black;
                button3.BackColor = Color.Black;
                button4.BackColor = Color.Gray;
                tabPage2.BackColor = Color.DimGray;
                tabPage1.BackColor = Color.DimGray;
            }
            else
            {
                textBox1.BackColor = Color.White;
                textBox2.BackColor = Color.White;
                textBox3.BackColor = Color.White;   
                comboBox1.BackColor = Color.White;
                button1.BackColor= Color.White;
                button2.BackColor= Color.White;
                button3.BackColor= Color.White;
                button4.BackColor= Color.White;
                tabPage2.BackColor = Color.WhiteSmoke;
                tabPage1.BackColor = Color.WhiteSmoke;
            }    
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            
            string name = IMoutputPath + "//scrshot" + CountIM + ".png";
            scrRec.TakeScrShots(name);
            this.Show();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "chon folder de luu anh";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                IMoutputPath = dialog.SelectedPath;
                textBox3.Text = IMoutputPath;
                
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            scrRec.CleanUp();
        }

        private void SetFolder()
        {
            if (!Directory.Exists(VXPath))
            {
                Directory.CreateDirectory(VXPath);
                Directory.CreateDirectory( VDoutputPath);
                Directory.CreateDirectory( IMoutputPath);

            }
        }
    }
}
