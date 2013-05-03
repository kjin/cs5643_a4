using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using Common;

namespace ParticlePhysics
{
    /// <summary>
    /// Represents a force that can be applied to any number of particles.
    /// </summary>
    public abstract class Force : Drawable
    {
        /// <summary>
        /// Applies the force on affected particles.
        /// </summary>
        public abstract void ApplyForce();

        public virtual int NumDrawables { get { return 0; } }
        public virtual int DrawableType(int index) { return Constants.Graphics.DRAW_NOTHING; }
        public virtual Color4 DrawableColor(int index) { return Color4.White; }
        public virtual Vector3 GetPoint(int index) { return Vector3.Zero; }
    }
}
