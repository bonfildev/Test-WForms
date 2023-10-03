
namespace SeSecEL
{
    partial class Capture
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Capture));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.fpsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.timerMD = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblRecCam1 = new System.Windows.Forms.Label();
            this.txtMotionDetector = new System.Windows.Forms.TextBox();
            this.chkMotionDetector = new System.Windows.Forms.CheckBox();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.buttonRecSave = new System.Windows.Forms.Button();
            this.buttonRecStop = new System.Windows.Forms.Button();
            this.buttonRecStart = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chart = new Accord.Controls.Wavechart();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.pictureBox1 = new Accord.Controls.PictureBox();
            this.videoSourcePlayer1 = new Accord.Controls.VideoSourcePlayer();
            this.statusStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panelContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fpsLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 644);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip.Size = new System.Drawing.Size(1608, 22);
            this.statusStrip.TabIndex = 12;
            this.statusStrip.Text = "statusStrip1";
            // 
            // fpsLabel
            // 
            this.fpsLabel.Name = "fpsLabel";
            this.fpsLabel.Size = new System.Drawing.Size(1585, 15);
            this.fpsLabel.Spring = true;
            this.fpsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "AVI files (*.avi)|*.avi|All files (*.*)|*.*";
            this.openFileDialog.Title = "Opem movie";
            // 
            // timerMD
            // 
            this.timerMD.Interval = 500;
            this.timerMD.Tick += new System.EventHandler(this.timerMD_Tick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(325, 644);
            this.panel1.TabIndex = 25;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(325, 644);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(317, 611);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblRecCam1);
            this.groupBox2.Controls.Add(this.txtMotionDetector);
            this.groupBox2.Controls.Add(this.chkMotionDetector);
            this.groupBox2.Controls.Add(this.buttonOpenFile);
            this.groupBox2.Controls.Add(this.buttonRecSave);
            this.groupBox2.Controls.Add(this.buttonRecStop);
            this.groupBox2.Controls.Add(this.buttonRecStart);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(311, 168);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Camara 1";
            // 
            // lblRecCam1
            // 
            this.lblRecCam1.AutoSize = true;
            this.lblRecCam1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecCam1.ForeColor = System.Drawing.Color.Red;
            this.lblRecCam1.Location = new System.Drawing.Point(227, 22);
            this.lblRecCam1.Name = "lblRecCam1";
            this.lblRecCam1.Size = new System.Drawing.Size(75, 25);
            this.lblRecCam1.TabIndex = 10;
            this.lblRecCam1.Text = "[• REC]";
            // 
            // txtMotionDetector
            // 
            this.txtMotionDetector.Enabled = false;
            this.txtMotionDetector.Location = new System.Drawing.Point(6, 136);
            this.txtMotionDetector.Name = "txtMotionDetector";
            this.txtMotionDetector.Size = new System.Drawing.Size(246, 26);
            this.txtMotionDetector.TabIndex = 10;
            // 
            // chkMotionDetector
            // 
            this.chkMotionDetector.AutoSize = true;
            this.chkMotionDetector.Location = new System.Drawing.Point(6, 109);
            this.chkMotionDetector.Name = "chkMotionDetector";
            this.chkMotionDetector.Size = new System.Drawing.Size(265, 24);
            this.chkMotionDetector.TabIndex = 9;
            this.chkMotionDetector.Text = "Activar Deteccion de Movimiento";
            this.chkMotionDetector.UseVisualStyleBackColor = true;
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Location = new System.Drawing.Point(218, 68);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(84, 35);
            this.buttonOpenFile.TabIndex = 8;
            this.buttonOpenFile.Text = "Open File";
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // buttonRecSave
            // 
            this.buttonRecSave.Location = new System.Drawing.Point(128, 68);
            this.buttonRecSave.Name = "buttonRecSave";
            this.buttonRecSave.Size = new System.Drawing.Size(84, 35);
            this.buttonRecSave.TabIndex = 7;
            this.buttonRecSave.Text = "Record";
            this.buttonRecSave.UseVisualStyleBackColor = true;
            this.buttonRecSave.Click += new System.EventHandler(this.buttonRecSave_Click);
            // 
            // buttonRecStop
            // 
            this.buttonRecStop.Location = new System.Drawing.Point(6, 68);
            this.buttonRecStop.Name = "buttonRecStop";
            this.buttonRecStop.Size = new System.Drawing.Size(116, 35);
            this.buttonRecStop.TabIndex = 6;
            this.buttonRecStop.Text = "Stop Camera";
            this.buttonRecStop.UseVisualStyleBackColor = true;
            this.buttonRecStop.Click += new System.EventHandler(this.buttonRecStop_Click);
            // 
            // buttonRecStart
            // 
            this.buttonRecStart.Location = new System.Drawing.Point(6, 27);
            this.buttonRecStart.Name = "buttonRecStart";
            this.buttonRecStart.Size = new System.Drawing.Size(114, 35);
            this.buttonRecStart.TabIndex = 5;
            this.buttonRecStart.Text = "Start Camera";
            this.buttonRecStart.UseVisualStyleBackColor = true;
            this.buttonRecStart.Click += new System.EventHandler(this.buttonRecStart_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(317, 611);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chart);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(311, 185);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Camara 1";
            // 
            // chart
            // 
            this.chart.BackColor = System.Drawing.Color.Black;
            this.chart.ForeColor = System.Drawing.Color.DarkGreen;
            this.chart.Location = new System.Drawing.Point(22, 98);
            this.chart.Margin = new System.Windows.Forms.Padding(4);
            this.chart.Name = "chart";
            this.chart.RangeX = ((Accord.DoubleRange)(resources.GetObject("chart.RangeX")));
            this.chart.RangeY = ((Accord.DoubleRange)(resources.GetObject("chart.RangeY")));
            this.chart.SimpleMode = false;
            this.chart.Size = new System.Drawing.Size(214, 62);
            this.chart.TabIndex = 7;
            this.chart.Text = "chart1";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(22, 67);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(104, 24);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.Text = "Maximizar";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(22, 37);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(84, 24);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Normal";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // panelContainer
            // 
            this.panelContainer.BackColor = System.Drawing.Color.Lavender;
            this.panelContainer.Controls.Add(this.pictureBox1);
            this.panelContainer.Controls.Add(this.videoSourcePlayer1);
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(325, 0);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(1283, 644);
            this.panelContainer.TabIndex = 26;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = null;
            this.pictureBox1.Location = new System.Drawing.Point(638, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(564, 461);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // videoSourcePlayer1
            // 
            this.videoSourcePlayer1.Location = new System.Drawing.Point(3, 4);
            this.videoSourcePlayer1.Name = "videoSourcePlayer1";
            this.videoSourcePlayer1.Size = new System.Drawing.Size(629, 461);
            this.videoSourcePlayer1.TabIndex = 0;
            this.videoSourcePlayer1.Text = "videoSourcePlayer1";
            this.videoSourcePlayer1.VideoSource = null;
            this.videoSourcePlayer1.NewFrame += new Accord.Controls.VideoSourcePlayer.NewFrameHandler(this.videoSourcePlayer1_NewFrame);
            // 
            // Capture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1608, 666);
            this.Controls.Add(this.panelContainer);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip);
            this.Name = "Capture";
            this.Text = "Captura";
            this.Activated += new System.EventHandler(this.Captura_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Captura_FormClosing);
            this.Load += new System.EventHandler(this.Captura_Load);
            this.SizeChanged += new System.EventHandler(this.Captura_SizeChanged);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Captura_MouseDown);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel fpsLabel;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Timer timerMD;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblRecCam1;
        private System.Windows.Forms.TextBox txtMotionDetector;
        private System.Windows.Forms.CheckBox chkMotionDetector;
        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.Button buttonRecSave;
        private System.Windows.Forms.Button buttonRecStop;
        private System.Windows.Forms.Button buttonRecStart;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Panel panelContainer;
        private Accord.Controls.Wavechart chart;
        private Accord.Controls.VideoSourcePlayer videoSourcePlayer1;
        private Accord.Controls.PictureBox pictureBox1;
    }
}