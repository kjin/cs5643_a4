using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ParticlePhysics
{
    /// <summary>
    /// Represents a constraint on a particle that zeroes out its force vector.
    /// </summary>
    public class PinConstraint : Constraint
    {
        Particle p1;

        /// <summary>
        /// Constructs a new pin constraint.
        /// </summary>
        /// <param name="p1">The particle to pin.</param>
        public PinConstraint(Particle p1)
        {
            this.p1 = p1;
        }

        public override void ApplyForce()
        {
            p1.Force = Vector3.Zero;
        }

        public override int NumDrawables { get { return 1; } }
        public override int DrawableType(int index) { return Constants.Graphics.DRAW_POINT; }
        public override Color DrawableColor(int index) { return Color.Red; }
        public override Vector3 GetPoint(int index) { return p1.Position; }
    }
}
