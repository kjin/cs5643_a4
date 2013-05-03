using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace ParticlePhysics
{
    /// <summary>
    /// Represents a stretching spring force between two particles.
    /// </summary>
    public class SpringForceStretch : Force
    {
        Particle p1;
        Particle p2;
        float restLength;

        /// <summary>
        /// Constructs a new stretching spring force.
        /// </summary>
        /// <param name="p1">A particle.</param>
        /// <param name="p2">A particle.</param>
        public SpringForceStretch(Particle p1, Particle p2)
        {
            this.p1 = p1;
            this.p2 = p2;
            restLength = (p2.Position - p1.Position).Length;
        }

        public override void ApplyForce()
        {
            Vector3 diff = p2.Position - p1.Position;
            float currentLength = diff.Length;
            diff.Normalize();

            // DAMPING: dv-dot-dpHat
            float dvDot = Vector3.Dot(diff, p2.Velocity) - Vector3.Dot(diff, p1.Velocity);
            
            diff *= Constants.Physics.STIFFNESS_STRETCH * (currentLength - restLength) + Constants.Physics.DAMPING_MASS * dvDot;

            p1.Force += diff;
            p2.Force -= diff;
        }

        public override int NumDrawables { get { return 1; } }
        public override int DrawableType(int index) { return Constants.Graphics.DRAW_LINE; }
        public override Vector3 GetPoint(int index) { return index == 1 ? p2.Position : p1.Position; }
    }
}
