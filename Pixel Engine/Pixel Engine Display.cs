using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

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
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler((sender, e) => Close());
            worker.WorkerReportsProgress = true;

            worker.RunWorkerAsync();
            Application.Run(this);
        }
    }
}
