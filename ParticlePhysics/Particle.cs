using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Common;

namespace ParticlePhysics
{
    /// <summary>
    /// Represents a basic particle object.
    /// </summary>
    public class Particle : Drawable, GLDrawable
    {
        //mechanics
        Vector3 initialPosition;
        Vector3 position;
        Vector3 velocity;
        Vector3 force;
        float mass;

        /// <summary>
        /// Constructs a new particle object.
        /// </summary>
        /// <param name="position">The position of the particle.</param>
        public Particle(Vector3 position)
        {
            initialPosition = this.position = position;
            mass = 1;
        }

        /// <summary>
        /// Updates the particle's velocity and location, and resets its force vector, using the Forward Euler method.
        /// </summary>
        /// <param name="dt">The timestep size.</param>
        public void Update(float dt)
        {
            velocity += (dt / mass) * force;
            position += dt * velocity;
            force = Vector3.Zero;
        }

        /// <summary>
        /// Resets the particle's properties.
        /// </summary>
        public void Reset()
        {
            position = initialPosition;
            velocity = force = Vector3.Zero;
        }

        public virtual int NumDrawables { get { return 1; } }
        public virtual int DrawableType(int index) { return index == 1 ? Constants.Graphics.DRAW_LINE : Constants.Graphics.DRAW_POINT; }
        public virtual Color4 DrawableColor(int index) { return index == 1 ? Color4.Yellow : Color4.White; }
        public Vector3 GetPoint(int index) { return index == 2 ? position + velocity / 10 : position; }

        public virtual void Draw()
        {
            GL.Color4(Color4.White);
            GL.Begin(BeginMode.Points);
            GL.Vertex3(position);
            GL.End();
        }

        public Vector3 Position
        {
            get
            {
                return position;
            }
        }

        public Vector3 Velocity
        {
            get
            {
                return velocity;
            }
        }

        public float Mass
        {
            get
            {
                return mass;
            }
        }

        public Vector3 Force
        {
            get
            {
                return force;
            }
            set
            {
                force = value;
            }
        }
    }
}
