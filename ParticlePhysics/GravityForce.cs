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
        List<Particle> particles;
        Vector3 forceVector;

        /// <summary>
        /// Constructs a new gravity force.
        /// </summary>
        /// <param name="particles">The list of particles affected by this gravity force.</param>
        public GravityForce(List<Particle> particles)
        {
            this.particles = particles;
            forceVector = Constants.Physics.GRAVITY_MAGNITUDE * Constants.Physics.GRAVITY_DIRECTION;
        }

        public override void ApplyForce()
        {
            foreach (Particle p in particles)
                p.Force += forceVector;
        }
    }
}
