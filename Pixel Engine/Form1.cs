using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Pixel_Engine
{
    public partial class Form1 : Form
    {
        public PictureBox pictureBox = new PictureBox();
        public Form1(Action<BackgroundWorker> action)
        {
            InitializeComponent();
            this.action = action;
            image = new Bitmap(Width, Height);
            pictureBox.Image = image;
            components.Add(pictureBox);
        }

        public Image image;
        private Action<BackgroundWorker> action;

        [MTAThread]
        public void Start()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler((sender, e) => action(sender as BackgroundWorker));
            worker.ProgressChanged += new ProgressChangedEventHandler((sender, e) => Refresh());
            worker.WorkerReportsProgress = true;
            worker.RunWorkerAsync();
            Application.Run(this);
        }
    }
}
