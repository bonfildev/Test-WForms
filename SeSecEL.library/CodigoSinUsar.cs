using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeSecEL.library
{
    internal class CodigoSinUsar
    {


        //Captura una camara IP de formato JPEG
        //private void openJPEGURLToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    URLForm form = new URLForm();
        //    form.Description = "Enter URL of an updating JPEG from a web camera:";
        //    form.URLs = new string[]
        //        {
        //            "http://195.243.185.195/axis-cgi/jpg/image.cgi?camera=1",
        //            "http://162.191.138.108:81/cgi-bin/camera?resolution=640&amp;quality=1&amp;Language=0&amp;1693169686"
        //        };
        //    CloseCurrentVideoSource();

        //    if (form.ShowDialog(this) == DialogResult.OK)
        //    {
        //        bValRec = true;
        //        FileWriter.Open(GetPath() + System.DateTime.Now.ToString("ddMMyyyy-HH-mm-ss") + "video.mp4", 640, 480);
        //        // create video source
        //        JPEGStream jpegSource = new JPEGStream(form.URL);
        //        // open it
        //        OpenVideoSource(jpegSource);
        //        jpegSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
        //        jpegSource.Start();
        //    }

        //}
        ///// <summary>
        ///// Se agrega un nuevo frame all video para camaras ip
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="eventArgs"></param>
        //private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        //{
        //    IVideoSource videoSource = videoSourcePlayer1.VideoSource;
        //    int fmrte = videoSource.FramesReceived;
        //    //get frame
        //    Bitmap image = eventArgs.Frame;
        //    ///-------------------
        //    ///Agrega la fecha y hora en la que se esta guardando el video
        //    DateTime now = DateTime.Now;
        //    Graphics g = Graphics.FromImage(image);
        //    // paint current time
        //    SolidBrush brush = new SolidBrush(Color.Red);
        //    g.DrawString(now.ToString(), this.Font, brush, new PointF(5, 5));
        //    brush.Dispose();
        //    g.Dispose();
        //    ///-------------------
        //    //add the image as a new frame of video file
        //    if (image != null)
        //    {
        //        if (fmrte > 0 && fmrte < 60)
        //        {
        //            for (int i = 0; i <= 60 / fmrte; i++)
        //            {
        //                if (chkMotionDetector.Checked)
        //                {
        //                    if (LevelDetection >= CommonCache.Sensitivity)
        //                    {
        //                        FileWriter.WriteVideoFrame(image);
        //                    }
        //                }
        //                else
        //                    FileWriter.WriteVideoFrame(image);
        //            }
        //        }
        //        else
        //        {
        //            if (chkMotionDetector.Checked)
        //            {
        //                if (LevelDetection >= CommonCache.Sensitivity)
        //                {
        //                    FileWriter.WriteVideoFrame(image);
        //                }
        //            }
        //            else
        //                FileWriter.WriteVideoFrame(image);
        //        }
        //    }

        //}

        //private void openMJPEGURLToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    URLForm form = new URLForm();

        //    form.Description = "Enter URL of an MJPEG video stream:";
        //    form.URLs = new string[]
        //        {
        //            "http://195.243.185.195/axis-cgi/mjpg/video.cgi?camera=4",
        //            "http://195.243.185.195/axis-cgi/mjpg/video.cgi?camera=3",
        //        };

        //    if (form.ShowDialog(this) == DialogResult.OK)
        //    {
        //        // create video source
        //        MJPEGStream mjpegSource = new MJPEGStream(form.URL);

        //        // open it
        //        OpenVideoSource(mjpegSource);
        //    }
        //}
    }
}