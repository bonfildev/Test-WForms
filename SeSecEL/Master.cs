using SeSecEL.library;
using System;
using System.Windows.Forms;

namespace SeSecEL
{
    public partial class Master : Form
    {
        public Master()
        {
            InitializeComponent();
        }
        private void Master_Load(object sender, EventArgs e)
        {
            this.BackColor = System.Drawing.Color.FromArgb(CommonCache.BackGroundColorR,CommonCache.BackGroundColorG,CommonCache.BackGroundColorB);
        }

        private void capturaToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Form form = new Capture();
            form.MdiParent = this;
            form.Show();
        }

        private void usuariosToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Form form = new UsersCatalog();
            form.MdiParent = this;
            //form.Dock = DockStyle.Fill;
            form.Show();

        }

        private void Master_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            //Environment.Exit(0);
        }

        private void coloresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new Colors();
            form.MdiParent = this;
            //form.Dock = DockStyle.Fill;
            form.Show();
        }

        private void parametrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new Parameters();
            form.MdiParent = this;
            form.Show();
        }

        private void recorAudioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new RecordAudio();
            form.MdiParent = this;
            form.Show();
        }

        private void captureDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new CaptureDevice();
            form.MdiParent = this;
            form.Show();
        }
    }
}
