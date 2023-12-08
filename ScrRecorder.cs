
using System.Collections.Generic;

using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;

using Accord.Video.FFMPEG;


namespace VsCorder
{
    internal class ScrRecorder
    {
        //Cac bien lien quan den video
        private Rectangle bounds;//khung quay cua video
        private string outputPath = "";//duong dan folder video 
        public string tempPath = "";//duong dan folder luu anh chup man hinh de ghep thanh video sau do se xoa
        public int fileCount = 1;//so thu tu cho tung anh chup man hinh de ghep thanh video
        private List<string> inputImageSequence = new List<string>();//list luu lai cac ten file anh chup man hinh

        //Cac bien lien quan den file
        private string audioName = "mic.wav";//ten file audio
        private string videoName = "video.mp4";//ten file video
        public string finalName = "finalvideo.mp4";//ten file video sau khi da ghep 2 file video va audio lai thanh 1

        //su dung stopwatch de lien tuc chup man hinh moi lan tick
        private Stopwatch stopwatch = new Stopwatch();

        //Bien lien quan den Audio
        public static class NativeMethod
        {
            [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            public static extern int record(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);
        }

        //constuctor 
        public ScrRecorder(Rectangle khung,string outPath) 
        {
            CreateTempFolder("TempScrshots");
            bounds = khung;
            outputPath = outPath;
        }

        //Tao folder tam thoi de luu cac anh chup man hinh sau do se xoa
        private void CreateTempFolder(string name)
        {
            if (Directory.Exists("D://"))//kiem tra xem pc co o dia D hay khong neu co thi tao folder tam thoi trong o dia D, neu khong thi tao o do dia C
            {
                string Pathname = $"D://{name}";
                Directory.CreateDirectory(Pathname);
                tempPath = Pathname;
            }
            else
            {
                string Pathname = $"C://{name}";
                Directory.CreateDirectory(Pathname);
                tempPath = Pathname;
            }
        }
        //Xoa folder tam thoi sau khi da tao video thanh cong
        public void DeletePath(string targetDir)
        {
            string[] files=Directory.GetFiles(targetDir);
            

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file); 
            }
            
        }

        //vi san pham sau cung chi can 1 file mp4 da ghep tu 2 file audio va video nen can them 1 method de xoa 2 file du thua do ngoai tru file san pham cuoi cung
        public void DeleteFile(string targetDir,string AudioDelete,string VideoDelete )
        {
            string[] files= Directory.GetFiles(targetDir);
            foreach(string file in files)
            {
                if(file == AudioDelete||file==VideoDelete)
                {
                    File.SetAttributes(file,FileAttributes.Normal);
                    File.Delete(file);
                }
            }
        }
        //Tu dong xoa cac file anh chup man hinh neu nhu nguoi dung bat ngo thoat khoi chuong trinh trong qua trinh dang quay video
        public void CleanUp()
        {
            string[] files = Directory.GetFiles(tempPath);
            string[] dir = Directory.GetDirectories(tempPath);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }
            foreach(string file in dir)
            {
                Directory.Delete(file);
            } 
            Directory.Delete(tempPath);
        }

        public string GetElapsed()
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", stopwatch.Elapsed.Hours,stopwatch.Elapsed.Minutes,stopwatch.Elapsed.Seconds);
        }

        //Luu anh man hinh sau do luu lai vao tempFolder
        public void RecordVideo()
        {
            stopwatch.Start();
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }
                string name = tempPath + "//scrshot-" + fileCount + ".png";
                bitmap.Save(name, ImageFormat.Png);
                inputImageSequence.Add(name);
                bitmap.Dispose();
   
            }
            
                
            
        }
        //Ghi audio
        public void RecordAudio()
        {
            NativeMethod.record("open new Type waveaudio Alias recsound", "", 0, 0);
            NativeMethod.record("record recsound", "", 0, 0);
        }

        //Luu video bang cach ghep cac anh chup man hinh da luu trong tempfolder
        private void SaveVideo(int width,int height, int framerate)
        {
            using(VideoFileWriter vFWriter=new VideoFileWriter()) 
            {
                vFWriter.Open(outputPath + "//" + videoName, width, height, framerate, VideoCodec.MPEG2,6000000);

                foreach(string imagelocation in inputImageSequence)
                {
                    Bitmap imageFrame = Image.FromFile(imagelocation) as Bitmap;
                    vFWriter.WriteVideoFrame(imageFrame);
                    imageFrame.Dispose();
                }

                vFWriter.Close();
            }
        }

        //Luu audio
        private void SaveAudio()
        {
            string audioPath = "save recsound " + outputPath + "//" + audioName;
            NativeMethod.record(audioPath, "", 0, 0);
            NativeMethod.record("close recsound", "", 0, 0);
        }

        //ghep 2 file video va audio thanh 1 file video cuoi cung bang FFMPEG
        private void CombineVideoAudio(string video, string audio)
        {
            string command = $"/c ffmpeg -y -i \"{video}\" -i \"{audio}\" -shortest {finalName}";
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                FileName = "cmd.exe",
                WorkingDirectory = outputPath,
                Arguments = command

            };
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            using (Process exeProcess =Process.Start(startInfo))
            {
                exeProcess.WaitForExit();
            }
        }

        public void Stop()
        {
            stopwatch.Stop();

            int width = bounds.Width;
            int height = bounds.Height;
            int frameRate = inputImageSequence.Count/((int)stopwatch.Elapsed.TotalMilliseconds/1000);

            SaveAudio();
            
            SaveVideo(width, height,frameRate);

            CombineVideoAudio(videoName, audioName);

            stopwatch = new Stopwatch();

            DeletePath(tempPath);

            inputImageSequence.Clear();

            DeleteFile(outputPath, outputPath + "\\" + audioName, outputPath + "\\" + videoName);
        }

        public void TakeScrShots(string name)
        {
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }
                
                bitmap.Save(name, ImageFormat.Png);
            }
            
        }

    }
}
