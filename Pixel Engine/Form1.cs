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
        public Form1(Action<BackgroundWorker> action)
        {
            InitializeComponent();
            this.action = action;
        }

        private Action<BackgroundWorker> action;

        [MTAThread]
        public void Start()
        {
            glControl.BringToFront();
            glControl.Location = new Point(0, 0);
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler((sender, e) => action(worker));
            worker.ProgressChanged += new ProgressChangedEventHandler((sender, e) => glControl.Refresh());
            worker.WorkerReportsProgress = true;

            worker.RunWorkerAsync();
            Application.Run(this);
        }
    }
}
