using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Common;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ParticleForm
{
    public class GLDrawer : Drawer
    {
        public GLDrawer(float scale) : base(scale)
        {
            int w = 600;
            int h = 600;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, h, 0, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
            GL.Scale(scale, scale, scale);
        }

        public void Draw(GLDrawable d)
        {
            d.Draw();
        }

        public override void Reset()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            base.Reset();
        }
    }
}
