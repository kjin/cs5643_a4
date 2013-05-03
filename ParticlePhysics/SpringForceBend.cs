using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace ParticlePhysics
{
    /// <summary>
    /// Represents a spring bending force between three particles.
    /// </summary>
    public class SpringForceBend : Force
    {
        Particle p0;
        Particle p1;
        Particle p2;

        /// <summary>
        /// Constructs a new spring bending force.
        /// </summary>
        /// <param name="p0">A particle that should be part of a SpringForceStretch along with p1.</param>
        /// <param name="p1">A particle that should be part of SpringForceStretch objects with both p0 and p2.</param>
        /// <param name="p2">A particle that should be part of a SpringForceStretch along with p1.</param>
        public SpringForceBend(Particle p0, Particle p1, Particle p2)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
        }

        public override void ApplyForce()
        {
            Vector3 a = p1.Position - p0.Position;
            Vector3 b = p2.Position - p1.Position;
            float length = a.Length * b.Length;
            float adb = Vector3.Dot(a, b);
            float lhs = Constants.Physics.STIFFNESS_STRETCH / length;
            Vector3 f0 = lhs * (adb / a.LengthSquared * a - b);
            Vector3 f2 = lhs * (a - adb / b.LengthSquared * b);
            p0.Force += f0;
            p1.Force -= f0 + f2;
            p2.Force += f2;
        }
    }
}
