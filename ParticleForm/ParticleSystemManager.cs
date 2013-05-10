using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Drawing;
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
        ParticleSystem ps;
        GLDrawer drawer;
        float scale = 0.1f;
        BasicFluid2D fluid;

        /// <summary>
        /// Constructs a new particle system manager.
        /// </summary>
        /// <param name="width">The width of the drawing field.</param>
        /// <param name="height">The height of the drawing field.</param>
        public ParticleSystemManager(int width, int height)
        {
            ParticleSystem.TicksPerUpdate = 1000;
            ps = ParticleSystem.BuildParticleSystem();
            Particle p1 = ps.CreateParticle(new Vector3(20f, 10f, 0f));
            Particle p2 = ps.CreateParticle(new Vector3(20f, 12f, 0f));
            Particle p3 = ps.CreateParticle(new Vector3(20f, 16f, 0f));
            ps.AddForce(new SpringForceStretch(p1, p2));
            ps.AddForce(new SpringForceStretch(p2, p3));
            ps.AddForce(new SpringForceBend(p1, p2, p3));
            ps.AddConstraint(new PinConstraint(p2));
            Vector3 buffer = 5f * Vector3.One;
            //ps.CreateRectangleBounds(buffer, new Vector3(width * scale, height * scale, 0f) - buffer);
            drawer = new GLDrawer(1f / scale);

            fluid = new BasicFluid2D(60, 60);
        }

        public void Run()
        {
            ps.Run();
            fluid.TimeStep(0.0001);
        }

        public void Stop()
        {
            ps.Stop();
        }

        public void SetMouseStatus(float x, float y, bool clicked)
        {
            ps.MousePosition = new Vector2(x * scale, y * scale);
            ps.MouseDown = clicked;
        }

        public void Draw(Graphics g)
        {
            ps.Mutex.WaitOne();
            drawer.Reset();
            foreach (GLDrawable p in ps.Particles)
                drawer.Draw(p);
            foreach (GLDrawable f in ps.Forces)
                drawer.Draw(f);
            foreach (GLDrawable c in ps.Constraints)
                drawer.Draw(c);
            ps.Mutex.ReleaseMutex();

            drawer.Draw(fluid);
        }
    }
}
