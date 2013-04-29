using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace ParticlePhysics
{
    /// <summary>
    /// Represents a global drag force on all particles.
    /// </summary>
    public class ViscousDragForce : Force
    {
        List<Particle> particles;

        /// <summary>
        /// Constructs a new drag force.
        /// </summary>
        /// <param name="particles">A list of affected particles.</param>
        public ViscousDragForce(List<Particle> particles)
        {
            this.particles = particles;
        }

        public override void ApplyForce()
        {
            foreach (Particle p in particles)
                p.Force -= Constants.Physics.DAMPING_MASS * p.Velocity;
        }
    }
}
