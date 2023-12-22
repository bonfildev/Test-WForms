using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BasicAudio;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using FFMpegCore;
using SeSecEL.library;

namespace SeSecEL
{
    public partial class CaptureDeviceTest : Form
    {
        SqlTools sql = new SqlTools();
        DateTime timeRec;
        DateTime timeRecRestart;
        private TimeSpan diff;
        //Video
        IBackgroundSubtractor backgroundSubtractor;
        VideoWriter outputVideo; //Video de salida
        VideoCapture video;     //REPRODUCIR UN VIDEO   
        VideoCapture capture;   //capturar un video
        Mat frame;              //Para visualizar lo que transmite la camara
        Bitmap image;           //Se pasa lo que se obtuvo en ese frame y se visualiza en el PB
        bool Pause;             //Sin Utilizar, Variable para reproducir un video
        bool isCameraRunning = false;   //Variable para ver si la captura esta detenida
        private Stopwatch stopWatch = null;
        //Video- Deteccion del rostro
        static readonly CascadeClassifier faceCascadeClassifier = new CascadeClassifier("haarcascade_frontalface_alt_tree.xml");
        static readonly CascadeClassifier eyesCascadeClassifier = new CascadeClassifier("haarcascade_eye.xml");
        //AUdio
        bool isMicrophoneJustStarted;
        Recording audioRecorder;
        private long duration;

        private int iRec { get; set; } = 0;
        private int iRecVisible = 5;
        private int iRecNotVisible = 10;

        private string GetPath() => ConfigurationManager.AppSettings["FilePath"];
        private string vFile;
        private string aFile;
        private string iFile;

        public CaptureDeviceTest()
        {
            InitializeComponent();
        }
        private void CaptureDevice_Load(object sender, EventArgs e)
        {
            panelContainer.BackColor = System.Drawing.Color.FromArgb(CommonCache.BackGroundColorR, CommonCache.BackGroundColorG, CommonCache.BackGroundColorB);
            lblRecCam1.Visible = false;
            InitCampos();
        }

        private void InitCampos()
        {
            var videoDevices = new VideoCapture();
            //foreach (var device in videoDevices)
            //{
            //    ddlSelectCamera.Items.Add(device.Name);
            //}


            ResizeControls();
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Dock = DockStyle.None;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

            pictureBox1.Dock = DockStyle.Fill;
        }

        private void ResizeControls()
        {
            pictureBox1.Width = panelContainer.Width / 2;
            pictureBox1.Height = panelContainer.Height / 2;
        }

        private void CaptureDevice_Resize(object sender, EventArgs e)
        {
            ResizeControls();
        }

        private void CaptureDevice_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveVideoRecorded();
            if (isCameraRunning)
            {
                Task.Run(async () => mergefile(aFile, vFile));
            }
        }

        private void StartCamera()
        {
            DisposeCameraResources();
            isCameraRunning = true;
            capture = new VideoCapture(0);
            capture.Start();
            vFile = $"video_{System.DateTime.Now.ToString("ddMMyyyy-HH-mm-ss")}.mp4";
            outputVideo = new VideoWriter(GetPath() + vFile, 29, new System.Drawing.Size(640, 480), true);
        }

        private void StartMicrophone()
        {
            audioRecorder = new Recording();
            aFile = $"Audio_{System.DateTime.Now.ToString("ddMMyyyy-HH-mm-ss")}.wav";
            audioRecorder.Filename = GetPath()+ aFile;
            isMicrophoneJustStarted = true;
        }

        private void StopMicrophone()
        {
            audioRecorder.StopRecording();
        }

        private void SaveVideoRecorded()
        {
            if (isCameraRunning)
            {
                isCameraRunning = false;
                recordingTimer.Stop();
                recordingTimer.Enabled = false;
                TimerF.Stop();
                TimerF.Enabled = false;
                DisposeCameraResources();
                StopMicrophone();
                lblStatus.Text = "Recording ended.";
                //OutputRecordingAsync();
            }
        }
        private void RestartRecording()
        {
            if (!isCameraRunning)
            {
                timeRecRestart = System.DateTime.UtcNow.AddMinutes(30);
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
        }

        private void btnStartRecording_Click(object sender, EventArgs e)
        {
            if (!isCameraRunning)
            {
                timeRecRestart = System.DateTime.UtcNow.AddMinutes(30);
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
                SaveVideoRecorded();
                Task.Run(async () => mergefile(aFile, vFile));
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
                timeRec = System.DateTime.UtcNow;
                diff = timeRecRestart -timeRec;
                if(diff < TimeSpan.Zero)
                {
                    SaveVideoRecorded();
                    Task.Run(async () => mergefile(aFile, vFile));
                    RestartRecording();
                }
                
            }
        }

        private void TimerF_Tick(object sender, EventArgs e)
        {
            if (capture.IsOpened)
            {
                TimerF.Interval = (1000 / (int)capture.Get(Emgu.CV.CvEnum.CapProp.Fps));
                try
                {
                    frame = new Mat();
                    capture.Read(frame);
                    int fmrte = (int)capture.Get(Emgu.CV.CvEnum.CapProp.Fps);
                    if (frame != null)
                    {
                        image = frame.ToBitmap();
                        capture.ImageGrabbed += Capture_ImageGrabbed;
                        //pictureBox1.Image = image;  
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
            try
            {
                Mat m = new Mat();
                capture.Retrieve(m);
                ProcesarImagen(m.ToBitmap());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Procesa la imagen del video para detectar los rostros
        /// y lo muestra en el picturebox
        /// </summary>
        /// <param name="btm"></param>
        private void ProcesarImagen(Bitmap btm)
        {
            try
            {
                Image<Bgr, byte> bgrFrame = capture.QueryFrame().ToImage<Bgr, Byte>();
                Bitmap bitmap = (Bitmap)btm.Clone();
                Image<Gray, byte> grayframe = bgrFrame.Convert<Gray, byte>();
                Rectangle[] rectangles = faceCascadeClassifier.DetectMultiScale(grayframe, 1.1, 1);
                foreach (var rectangle in rectangles)
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Pen pen = new Pen(Color.Blue, 4))
                        {
                            graphics.DrawRectangle(pen, rectangle);
                        }

                    }
                }
                pictureBox1.Image = bitmap;
            }
            catch(Exception ex)
            {

            }
        }

        /// <summary>
        /// Mezcla 2 archivos Audio y video
        /// y los guarda en un tercer archivo
        /// </summary>
        /// <param name="wavefile"></param>
        /// <param name="videofile"></param>
        private async void mergefile(string wavefile, string videofile)
        {
            try
            {
                lblStatus.Text = $"Procesando el video.";
                string outputPath = $"output_{System.DateTime.Now.ToString("ddMMyyyy-HH-mm-ss")}.mp4";
                string args = "/c ffmpeg -i \"" + videofile + "\" -i \"" + wavefile + "\" -shortest " + outputPath;
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = false;
                startInfo.FileName = "cmd.exe";
                startInfo.WorkingDirectory = @"C:\Users\mxsae\Documents\TestingRecord\";
                startInfo.Arguments = args;
                using Process exeProcess = Process.Start(startInfo);
                exeProcess.WaitForExit();
                exeProcess.Close();
                lblStatus.Text = $"Recording saved to local disk with the file name {outputPath}.";
                File.Delete(GetPath() + wavefile);
                File.Delete(GetPath() + videofile);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Recording cannot be saved.";

                MessageBox.Show($"Recording cannot be saved because {ex.Message}", "Error on Recording Saving", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        ///////////////////////////////////////////////
        // Funciones que no utilizo pero pueden servir
        ///////////////////////////////////////////////
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

        private void btnCaptureFrame_Click(object sender, EventArgs e)
        {
            if(capture.IsOpened)
            {
                iFile = $"img_{System.DateTime.Now.ToString("ddMMyyyy-HH-mm-ss")}.jpeg";
                pictureBox1.Image.Save(GetPath() + iFile,System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        /// <summary>
        /// Abre una imagen y detecta los rostros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Multiselect = false, Filter = "JPEG|*.jpeg" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bitmap = new Bitmap(Image.FromFile(ofd.FileName));
                    Image<Bgr, byte> bgrFrame = bitmap.ToImage<Bgr, Byte>();
                    //Bitmap bitmap = (Bitmap)btm.Clone();
                    Image<Gray, byte> grayframe = bgrFrame.Convert<Gray, byte>();
                    Rectangle[] rectangles = faceCascadeClassifier.DetectMultiScale(grayframe, 1.1, 1);
                    foreach (var rectangle in rectangles)
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            using (Pen pen = new Pen(Color.Red, 1))
                            {
                                graphics.DrawRectangle(pen, rectangle);
                            }
                        }
                    }
                    rectangles = eyesCascadeClassifier.DetectMultiScale(grayframe, 1.1, 1);
                    foreach (var rectangle in rectangles)
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            using (Pen pen = new Pen(Color.Red, 1))
                            {
                                graphics.DrawRectangle(pen, rectangle);
                            }
                        }
                    }

                    pictureBox1.Image = bitmap;
                }
            }
        }
    }
}
