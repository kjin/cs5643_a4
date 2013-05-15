using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Drawing;
using System.Threading;
using ParticlePhysics;
using OpenTK;
using Fluid;

namespace ParticleForm
{
    /// <summary>
    /// Creates, runs, draws, and manages a ParticleSystem object.
    /// </summary>
    public class ParticleSystemManager
    {
        Vector2 dimensions;
        GLDrawer drawer;
        float scale = 0.05f;
        BasicFluid2D fluid;
        Vector2 xOld, xOld2;

        /// <summary>
        /// Constructs a new particle system manager.
        /// </summary>
        /// <param name="width">The width of the drawing field.</param>
        /// <param name="height">The height of the drawing field.</param>
        public ParticleSystemManager(int width, int height)
        {
            drawer = new GLDrawer(1f / scale);
            dimensions = new Vector2(width, height);
            fluid = new BasicFluid2D((int)(width * scale), (int)(height * scale));
        }

        public void Run()
        {
            fluid.DX = 0.001;
            fluid.GlobalForce = new Vector2(0, 5);
            new Thread(fluid.Run).Start();
        }

        public void Stop()
        {
            fluid.Stop();
        }

        public void SetMouseStatus(float x, float y, bool clicked, bool mod)
        {
            Vector2 xNew = new Vector2(x, y);
            Vector2 dx = xNew - xOld2;
            //Console.Write("Mouse: ({0}, {1})\n", x, y);
            //fluid.SetVelocity(x / dimensions.X, y / dimensions.Y, 100 * new Vector2d(dx.X, dx.Y));
            if (clicked)
            {
                Console.WriteLine(fluid.error);
                if (mod)
                    fluid.OnClick(x * scale, y * scale);
            }
            xOld = xNew;
            xOld2 = xOld;
        }

        public void Draw(Graphics g)
        {
            drawer.Reset();
            drawer.Draw(fluid);
        }
    }
}
