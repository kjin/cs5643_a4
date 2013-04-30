using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace ParticlePhysics
{
    /// <summary>
    /// Represents a global gravity force that acts on all particles. The magnitude and direction of the gravity force is defined in Constants.Physics.
    /// </summary>
    public class GravityForce : Force
    {
        ParticleSystem PS;
        Vector3 forceVector;

        /// <summary>
        /// Constructs a new gravity force.
        /// </summary>
        /// <param name="PS">The particle system affected by this gravity force.</param>
        public GravityForce(ParticleSystem PS)
        {
            this.PS = PS;
            forceVector = Constants.Physics.GRAVITY_MAGNITUDE * Constants.Physics.GRAVITY_DIRECTION;
        }

        public override void ApplyForce()
        {
            foreach (Particle p in PS.Particles)
                p.Force += forceVector;
        }
    }
}
