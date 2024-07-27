using System;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using Accord.Video.DirectShow;
using Accord.Video;
using Accord.Vision.Motion;

//RECORD
using Accord.Video.FFMPEG;
using Accord.DirectSound;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SeSecEL.library;
using Accord.Audio;
using Accord.Audio.Formats;
using System.IO;
using System.Text.RegularExpressions;

namespace SeSecEL
{
    public partial class Capture : Form
    {
        private string GetPath() => ConfigurationManager.AppSettings["FilePath"];
        SqlTools sql = new SqlTools();
        private int iRec { get; set; } = 0;
        private bool bValRec = false;
        private int iRecVisible = 5;
        private int iRecNotVisible = 10;
        //-----------------RECORD 
        //-----------------Video 
        private VideoCaptureDevice FinalVideo = null;
        private VideoCaptureDeviceForm captureDevice;
        private Bitmap video;
        public VideoFileWriter FileWriter = new VideoFileWriter(); //video with compression for an ipcammera or Webcam
        private SaveFileDialog saveAvi;
        private SaveFileDialog saveFileDialog1;
        private Stopwatch stopWatch = null;
        private string vFile;
        //-----------------Audio 
        private WaveEncoder encoder;
        private float[] current;
        private IAudioSource source;
        private MemoryStream stream;
        private Signal aSignal;
        private string aFile;
        private TimeSpan duration;
        ///-----------------------
        //Deteccion de movimiento
        //----------------- 
        private MotionDetector MDetector;
        private double LevelDetection;
        private double sensitivity = 0;

        private string iFile;
        //Border
        //----------------- 
        private Color borderColor = Color.FromArgb(128, 128, 255);
        //----------------- 
        //Drag Form Borders And Colors
        //----------------- 
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void Captura_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void Captura_Activated(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        private void Captura_SizeChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        /// <summary>
        /// Inicia la forma
        /// </summary>
        public Capture()
        {
            InitializeComponent();
            this.panelContainer.BackColor = borderColor;

            // Configure the wavechart
            chart.SimpleMode = true;
            chart.AddWaveform("wave", Color.Green, 1, false);
        }
        private void Captura_Load(object sender, EventArgs e)
        {
            MotionDetector();
            txtMotionDetector.Text = "0";
            panelContainer.BackColor = System.Drawing.Color.FromArgb(CommonCache.BackGroundColorR, CommonCache.BackGroundColorG, CommonCache.BackGroundColorB); 
            lblRecCam1.Visible = false;
            source = new AudioCaptureDevice()
            {
                // Listen on 22050 Hz
                DesiredFrameSize = 4096,
                SampleRate = 22050,

                // We will be reading 16-bit PCM
                Format = SampleFormat.Format16Bit
            };
        }

        private void InitCampos()
        {
            chkMotionDetector.Checked = false;
            txtMotionDetector.Text = "0";
        }

        private void Captura_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseCurrentVideoSource();
        }

        /// <summary>
        /// Cierra el video si se esta reproduciendo
        /// no modificar el orden
        /// </summary>
        private void CloseCurrentVideoSource()
        {
            if (videoSourcePlayer1.VideoSource != null)
            {
                videoSourcePlayer1.SignalToStop();
                // wait ~ 3 seconds
                for (int i = 0; i < 30; i++)
                {
                    if (!videoSourcePlayer1.IsRunning)
                        break;
                    System.Threading.Thread.Sleep(100);
                }
                if (FileWriter.IsOpen)
                {
                    FileWriter.Close();
                }
                if (videoSourcePlayer1.IsRunning)
                {
                    videoSourcePlayer1.Stop();
                }
                videoSourcePlayer1.VideoSource = null;
            }

            // Stops both cases
            if (source != null)
            {
                // If we were recording
                source.SignalToStop();
                source.WaitForStop();
            }

            //updateButtons();

            // Also zero out the buffers and screen
            if (current != null)
            {
                Array.Clear(current, 0, current.Length);
                updateWaveform(current, current.Length);
            }
        }

        private void MotionDetector()
        {
            MDetector = new MotionDetector(new TwoFramesDifferenceDetector(), new MotionBorderHighlighting());
        }
        // New frame received by the player
        private void videoSourcePlayer1_NewFrame(object sender, ref Bitmap image)
        {
            try
            {
                DateTime now = DateTime.Now;
                Graphics g = Graphics.FromImage(image);
                // paint current time
                SolidBrush brush = new SolidBrush(Color.Red);
                g.DrawString(now.ToString(), new Font("Arial",image.Height / 25), brush, new PointF(5, image.Height - 80));
                brush.Dispose();
                g.Dispose();
                //Deteccion de movimiento
                if (chkMotionDetector.Checked)
                {
                    LevelDetection = MDetector.ProcessFrame(image); 
                    if(CommonCache.Sensitivity >= Int64.Parse(LevelDetection.ToString()))
                    {
                        CaptureFrame();
                    }
                }
            }
            catch (Exception ex)
            {
                sql.WriteToFile(ex.Message);
            }
        }

        // On timer event - gather statistics
        private void timer_Tick(object sender, EventArgs e)
        {
            IVideoSource videoSource = videoSourcePlayer1.VideoSource;
            if (videoSource != null)
            {
                // get number of frames since the last timer tick
                int framesReceived = videoSource.FramesReceived;

                if (stopWatch == null)
                {
                    stopWatch = new Stopwatch();
                    stopWatch.Start();
                }
                else
                {
                    stopWatch.Stop();
                    float fps = 1000.0f * framesReceived / stopWatch.ElapsedMilliseconds;
                    fpsLabel.Text = fps.ToString("F2") + " fps";
                    stopWatch.Reset();
                    stopWatch.Start();
                }
            }
            Recording();
        }


        private void timerMD_Tick(object sender, EventArgs e)
        {
            txtMotionDetector.Text = LevelDetection.ToString("00.00000000000000000000");
            lbLength.Text = String.Format("Length: {0:00.00} sec.", duration.Seconds);
        }

        private void Recording()
        {
            if (bValRec)
            {
                iRec++;
                if(iRec <= iRecVisible)
                {
                    lblRecCam1.Visible = true;
                }
                else if(iRec > iRecVisible && iRec <= iRecNotVisible)
                {
                    lblRecCam1.Visible = false;
                }
                else if (iRec >= iRecNotVisible) 
                {
                    iRec = 0;
                }
            }
        }

        // Open video source
        private void OpenVideoSource(IVideoSource source)
        {
            // set busy cursor
            this.Cursor = Cursors.WaitCursor;

            // stop current video source
            CloseCurrentVideoSource();

            // start new video source
            videoSourcePlayer1.VideoSource = source;
            videoSourcePlayer1.Start();

            // reset stop watch
            stopWatch = null;

            // start timer
            timer.Start();
            timerMD.Start();

            this.Cursor = Cursors.Default;
        }

            //Captura la pantalla
        private void capture1stDisplayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenVideoSource(new ScreenCaptureStream(Screen.AllScreens[0].Bounds, 100));

        }
        //Captura una camara
        private void buttonRecStart_Click(object sender, EventArgs e)
        {
            CloseCurrentVideoSource();
            InitCampos();
            captureDevice = new VideoCaptureDeviceForm();

            // Create capture device
            source = new AudioCaptureDevice()
            {
                // Listen on 22050 Hz
                DesiredFrameSize = 4096,
                SampleRate = 22050,

                // We will be reading 16-bit PCM
                Format = SampleFormat.Format16Bit
            };

            if (captureDevice.ShowDialog(this) == DialogResult.OK)
            {
                // create video source
                FinalVideo = captureDevice.VideoDevice;
            
                // open video source
                OpenVideoSource(FinalVideo);

                FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
                source.NewFrame += audioSource_NewFrame;
                source.AudioSourceError += source_AudioSourceError;
                // Create buffer for wavechart control
                current = new float[source.DesiredFrameSize];
                // Create stream to store file
                stream = new MemoryStream();
                encoder = new WaveEncoder(stream);
                source.Start();
                FinalVideo.Start();

                //Startrecording();

                 
            }
        }
        /// <summary>
        /// Start The Video recording
        /// </summary>
        private void Startrecording()
        {
            if (video != null)
            {
                vFile = System.DateTime.Now.ToString("ddMMyyyy-HH-mm-ss") + "video.mp4";
                FileWriter.Open(GetPath() + vFile, 640, 480);
                FileWriter.WriteVideoFrame(video);
                //FileWriter.WriteAudioFrame(aSignal.RawData);
                bValRec = true;
                
            }
        }

        /// <summary>
        /// Se agrega un nuevo frame al video para camaras conectadas al equipo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        void FinalVideo_NewFrame(object sender, Accord.Video.NewFrameEventArgs eventArgs)
        {
            if (bValRec)
            {
                video = (Bitmap)eventArgs.Frame.Clone();
                /// Agrega la fechay hora en la que se esta grabando el video
                DateTime now = DateTime.Now;
                Graphics g = Graphics.FromImage(video);
                // paint current time
                SolidBrush brush = new SolidBrush(Color.Red);
                g.DrawString(now.ToString(), new Font("Arial", video.Height / 25), brush, new PointF(5, video.Height - 80));
                brush.Dispose();
                g.Dispose();
                FileWriter.WriteVideoFrame(video);
            }
            else //Stop
            {
                video = (Bitmap)eventArgs.Frame.Clone();
                pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
            }
        }


        private void audioSource_NewFrame(object sender, Accord.Audio.NewFrameEventArgs eventArgs)
        {
            if (bValRec)
            {
                // Save current frame
                eventArgs.Signal.CopyTo(current);

                // Update waveform
                updateWaveform(current, eventArgs.Signal.Length);

                // Save to memory
                encoder.Encode(eventArgs.Signal);

                // Read current frame...
                aSignal = eventArgs.Signal;
                FileWriter.WriteAudioFrame(aSignal.RawData);

                // Update counters
                duration += eventArgs.Signal.Duration;
            }
            else
            {
                aSignal = (Signal)eventArgs.Signal;
                // Save current frame
                eventArgs.Signal.CopyTo(current);
                // Update waveform
                updateWaveform(current, eventArgs.Signal.Length);
            }
        }

        /// <summary>
        ///   Updates the audio display in the wave chart
        /// </summary>
        /// 
        private void updateWaveform(float[] samples, int length)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    chart.UpdateWaveform("wave", samples, length);
                }));
            }
            else
            {
                chart.UpdateWaveform("wave", current, length);
            }
        }

        /// <summary>
        ///   This callback will be called when there is some error with the audio 
        ///   source. It can be used to route exceptions so they don't compromise 
        ///   the audio processing pipeline.
        /// </summary>

        private void source_AudioSourceError(object sender, AudioSourceErrorEventArgs e)
        {
            throw new Exception(e.Description);
        }


        private void buttonRecStop_Click(object sender, EventArgs e)
        {
            CloseCurrentVideoSource();
            bValRec = false;
            lblRecCam1.Visible = false;
            aFile = System.DateTime.Now.ToString("ddMMyyyy-HH-mm-ss") + "Audio.wav";
            var fileStream = File.Create(GetPath() + aFile);
            stream.WriteTo(fileStream);
            fileStream.Close();
            mergefile(aFile , vFile);
        }

        private void buttonRecSave_Click(object sender, EventArgs e)
        {
            if (video != null)
            {
                saveAvi = new SaveFileDialog();
                saveAvi.Filter = "MP4 files (*.mp4)|*.mp4|Avi Files (*.avi)|*.avi";
                if (saveAvi.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    int h = captureDevice.VideoDevice.VideoResolution.FrameSize.Height;
                    int w = captureDevice.VideoDevice.VideoResolution.FrameSize.Width;
                    FileWriter.Open(saveAvi.FileName, w, h, 25, VideoCodec.Default, 5000000);
                    FileWriter.WriteVideoFrame(video);
                    FileWriter.WriteAudioFrame(aSignal.RawData);
                    bValRec = true;
                }
            }
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // create video source
                FileVideoSource fileSource = new FileVideoSource(openFileDialog.FileName);
                // open it
                OpenVideoSource(fileSource);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            videoSourcePlayer1.Dock = DockStyle.None;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            videoSourcePlayer1.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Mezcla 2 archivos Audio y video
        /// y los guarda en un tercer archivo
        /// </summary>
        /// <param name="wavefile"></param>
        /// <param name="videofile"></param>
        private void mergefile(string wavefile, string videofile)
        {
            string args = "/c ffmpeg -i \""+ videofile + "\" -i \""+ wavefile + "\" -shortest outPutFile.mp4";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.FileName = "cmd.exe";
            startInfo.WorkingDirectory = @"C:\Users\mxsae\Documents\TestingRecord\";
            startInfo.Arguments = args;
            using Process exeProcess = Process.Start(startInfo);
            exeProcess.WaitForExit();
            exeProcess.Close();
        }

        private void btnCaptureFrame_Click(object sender, EventArgs e)
        {
            CaptureFrame();
        }

        private void CaptureFrame()
        {
            if (bValRec)
            {
                iFile = $"img_{System.DateTime.Now.ToString("ddMMyyyy-HH-mm-ss")}.jpeg";
                pictureBox1.Image.Save(GetPath() + iFile, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }
    }
}
