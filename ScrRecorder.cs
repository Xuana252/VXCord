using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using Accord.Video.FFMPEG;

namespace VsCorder
{
    internal class ScrRecorder
    {
        //Cac bien lien quan den video
        private Rectangle bounds;
        private string outputPath = "";//duong dan folder video 
        private string tempPath = "";//duong dan folder luu anh chup man hinh de ghep thanh video sau do se xoa
        private int fileCount = 1;//so thu tu cho tung anh chup man hinh de ghep thanh video
        private List<string> inputImageSequence = new List<string>();//list luu lai cac ten file anh chup man hinh

        //Cac bien lien quan den file
        private string audioName = "mic.wav";//ten file audio
        private string videoName = "video.mp4";//ten file video
        private string finalName = "finalvideo.mp4";//ten file video sau khi da ghep 2 file video va audio rieng le lai thanh 1

    }
}
