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
        ParticleSystem PS;

        /// <summary>
        /// Constructs a new drag force.
        /// </summary>
        /// <param name="PS">A particle system containing affected particles.</param>
        public ViscousDragForce(ParticleSystem PS)
        {
            this.PS = PS;
        }

        public override void ApplyForce()
        {
            foreach (Particle p in PS.Particles)
                p.Force -= Constants.Physics.DAMPING_MASS * p.Velocity;
        }
    }
}
