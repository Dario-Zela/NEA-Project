using SharpGL;

namespace Pixel_Engine
{
    partial class Form1
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
            this.Controls.Clear();
            this.glControl = new OpenGLControl();
            ((System.ComponentModel.ISupportInitialize)(this.glControl)).BeginInit();
            this.SuspendLayout();
            this.ForeColor = System.Drawing.Color.Transparent;

            this.glControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl.FrameRate = 20;
            this.glControl.Name = "GLControl";
            this.glControl.TabIndex = 0;
            this.glControl.Size = new System.Drawing.Size(0, 0);

            this.Controls.Add(glControl);
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.glControl)).EndInit();
        }

        public OpenGLControl GetGLControl()
        {
            return glControl;
        }

        private OpenGLControl glControl;
        #endregion
    }
}

