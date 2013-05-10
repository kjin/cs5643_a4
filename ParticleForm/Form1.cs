using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ParticlePhysics;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ParticleForm
{
    using Timer = System.Windows.Forms.Timer;
    using Color = System.Drawing.Color;

    public partial class Form1 : Form
    {
        ParticleSystemManager psm;
        bool loaded = false;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Console.SetOut(new TextBoxStreamWriter(this.textBox1));
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            this.DoubleBuffered = true;
            psm = new ParticleSystemManager(600, 600);
            psm.Run();
            timer1.Interval = 10;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            glControl1.Refresh();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            psm.Stop();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //psm.Draw(e.Graphics);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            //psm.SetMouseStatus(e.X, e.Y, true);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            //psm.SetMouseStatus(e.X, e.Y, e.Button != System.Windows.Forms.MouseButtons.None);
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            //psm.SetMouseStatus(e.X, e.Y, false);
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            loaded = true;
            GL.ClearColor(Color.Black);
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            if (!loaded) return;
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded) return;
            psm.Draw(e.Graphics);
            glControl1.SwapBuffers();
        }

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            psm.SetMouseStatus(e.X, e.Y, true);
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            psm.SetMouseStatus(e.X, e.Y, e.Button != System.Windows.Forms.MouseButtons.None);
        }

        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            psm.SetMouseStatus(e.X, e.Y, false);
        }
    }
}
