using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BasicAudio;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using FFMpegCore;
using SeSecEL.library;

namespace SeSecEL
{
    public partial class CaptureDevice : Form
    {

        //Video
        VideoWriter outputVideo;
        VideoCapture video;     //REPRODUCIR UN VIDEO   
        VideoCapture capture;   //capturar un video
        Bitmap bitmap;
        Mat frame;
        Bitmap imageAlternate;
        Bitmap image;
        bool isUsingImageAlternate = false;
        bool Pause;
        bool isCameraRunning = false;
        SqlTools sql = new SqlTools();
        private Stopwatch stopWatch = null;

        //AUdio
        bool isMicrophoneJustStarted;
        Recording audioRecorder;
        private string aFile;

        private long duration;

        private int iRec { get; set; } = 0;
        private int iRecVisible = 5;
        private int iRecNotVisible = 10;

        private string GetPath() => ConfigurationManager.AppSettings["FilePath"];
        private string vFile;
        public CaptureDevice()
        {
            InitializeComponent();
        }
        private void CaptureDevice_Load(object sender, EventArgs e)
        {
            panelContainer.BackColor = System.Drawing.Color.FromArgb(CommonCache.BackGroundColorR, CommonCache.BackGroundColorG, CommonCache.BackGroundColorB);
            lblRecCam1.Visible = false;
        }
        private void StartCamera()
        {
            DisposeCameraResources();

            isCameraRunning = true;

            capture = new VideoCapture(0);
            capture.Start();
            vFile = "video.mp4";
            outputVideo = new VideoWriter(GetPath() + vFile, 29, new System.Drawing.Size(640, 480), true);
        }

        private void StartMicrophone()
        {
            audioRecorder = new Recording();
            aFile = "Audio.wav";
            audioRecorder.Filename = GetPath()+ aFile;
            isMicrophoneJustStarted = true;
        }

        private void StopMicrophone()
        {
            audioRecorder.StopRecording();
        }

        private void btnStartRecording_Click(object sender, EventArgs e)
        {
            if (!isCameraRunning)
            {
                lblStatus.Text = "Starting recording...";
                isCameraRunning = true;
                // reset stop watch
                stopWatch = null;
                recordingTimer.Enabled = true;
                recordingTimer.Start();
                TimerF.Enabled = true;
                TimerF.Start();
                StartCamera();
                StartMicrophone();
                //capture.ImageGrabbed += Capture_ImageGrabbed;
                capture.Start();
                lblStatus.Text = "Recording...";
            }
            else
            {

                isCameraRunning = false;
                recordingTimer.Stop();
                recordingTimer.Enabled = false;
                TimerF.Stop();
                TimerF.Enabled = false;
                DisposeCameraResources();
                StopMicrophone();
                lblStatus.Text = "Recording ended.";
                OutputRecordingAsync();
            }
        }
        private void DisposeCameraResources()
        {
            if (video != null)
            {
                video.Dispose();
            }

            if (capture != null)
            {
                capture.Dispose();
            }
            if (capture != null)
            {
                capture.Pause();
                capture.Stop();
                capture.Dispose();
            }
            if (outputVideo != null)
            {
                outputVideo.Dispose();
            }
        }


        private void recordingTimer_Tick(object sender, EventArgs e)
        {
            if (capture.IsOpened)
            {
                // get number of frames since the last timer tick
                int fmrte = (int)capture.Get(Emgu.CV.CvEnum.CapProp.Fps);

                if (stopWatch == null)
                {
                    stopWatch = new Stopwatch();
                    stopWatch.Start();
                }
                else
                {
                    stopWatch.Stop();
                    float fps = 1000.0f * fmrte / stopWatch.ElapsedMilliseconds;
                    labelFps.Text = fps.ToString("F2") + " fps";
                    stopWatch.Reset();
                    stopWatch.Start();
                    lbLength.Text = String.Format("Length: {0:00.00} sec.", (int)(duration/1000));
                }
            }
            Recording();
        }


        private void Recording()
        {
            if (isCameraRunning)
            {
                iRec++;
                if (iRec <= iRecVisible)
                {
                    lblRecCam1.Visible = true;
                }
                else if (iRec > iRecVisible && iRec <= iRecNotVisible)
                {
                    lblRecCam1.Visible = false;
                }
                else if (iRec >= iRecNotVisible)
                {
                    iRec = 0;
                }
            }
        }

        private void TimerF_Tick(object sender, EventArgs e)
        {
            if (capture.IsOpened)
            {
                try
                {
                    frame = new Mat();
                    capture.Read(frame);
                    int fmrte = (int)capture.Get(Emgu.CV.CvEnum.CapProp.Fps);
                    if (frame != null)
                    {
                        if (imageAlternate == null)
                        {
                            isUsingImageAlternate = true;
                            imageAlternate = frame.ToBitmap();
                        }
                        else if (image == null)
                        {
                            isUsingImageAlternate = false;
                            image = frame.ToBitmap();
                        }

                        pictureBox1.Image = isUsingImageAlternate ? imageAlternate : image;
                        if (fmrte > 0 && fmrte < 60)
                        {
                            for (int i = 0; i <= 1; i++)
                            {
                                outputVideo.Write(frame);
                            }
                        }
                        else
                        {
                            outputVideo.Write(frame);
                        }
                    }
                    }
                catch (Exception)
                {
                    pictureBox1.Image = null;
                }
                finally
                {
                    if (frame != null)
                    {
                        frame.Dispose();
                    }

                    if (isUsingImageAlternate && image != null)
                    {
                        image.Dispose();
                        image = null;
                    }
                    else if (!isUsingImageAlternate && imageAlternate != null)
                    {
                        imageAlternate.Dispose();
                        imageAlternate = null;
                    }
                }

                if (isMicrophoneJustStarted)
                {
                    audioRecorder.StartRecording();
                    duration = audioRecorder.TimeElapsed;
                }
            }
        }
        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {
           

        }



        private async void OutputRecordingAsync()
        {
            string outputPath = $"output_{System.DateTime.Now.ToString("ddMMyyyy-HH-mm-ss")}.mp4";

            try
            {
                FFMpeg.ReplaceAudio(GetPath() + vFile, GetPath() + aFile, outputPath, true);

                lblStatus.Text = $"Recording saved to local disk with the file name {outputPath}.";
                /*
                string azureStorageConnectionString = txtAzureStorageConnectionString.Text;
                if (!string.IsNullOrWhiteSpace(azureStorageConnectionString))
                {
                    try
                    {
                        BlobServiceClient blobServiceClient = new BlobServiceClient(azureStorageConnectionString);
                        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("webcam-videos");
                        BlobClient blobClient = containerClient.GetBlobClient(outputPath);

                        using FileStream uploadFileStream = File.OpenRead(outputPath);
                        await blobClient.UploadAsync(uploadFileStream, true);
                        uploadFileStream.Close();

                        lblStatus.Text = $"Recording saved to both local disk and Azure Blob Storage with the file name {outputPath}.";
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = $"Recording saved to both local disk with the file name {outputPath} but cannot be saved on Azure Blob Storage.";
                        MessageBox.Show(ex.Message, "Error on Saving to Azure Blob Storage", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }*/
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Recording cannot be saved.";

                MessageBox.Show($"Recording cannot be saved because {ex.Message}", "Error on Recording Saving", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void ReproduceVideo()
        {
            try
            {
                Mat m = new Mat();
                capture.Retrieve(m);
                pictureBox1.Image = m.ToBitmap();
            }
            catch (Exception ex)
            {
                sql.WriteToFile(ex.Message);
            }
        }


        /// <summary>
        /// Reproduce un video por tiempo indeterminado hay que agregar otro boton de pausa
        /// </summary>
        private async void Play()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                video = new VideoCapture(ofd.FileName);
                Mat m = new Mat();
                //Image<Bgr, byte> emguImage = bitmap.ToImage<Bgr, byte>();
                video.Read(m);
                pictureBox1.Image = m.ToBitmap();
                Play();
            }
            if (video == null)
            {
                return;
            }
            try
            {
                while (!Pause)
                {
                    Mat m = new Mat();
                    video.Read(m);
                    if (!m.IsEmpty)
                    {
                        pictureBox1.Image = m.ToBitmap();
                        //double fps  = Convert.ToInt32(video.Get(Emgu.CV.CvEnum.CapProp.Fps));
                        //await Task.Delay(1000/Convert.ToInt32((double)fps));
                        Thread.Sleep((int)video.Get(Emgu.CV.CvEnum.CapProp.Fps));
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                sql.WriteToFile(ex.Message);
            }

        }

    }
}
