using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;

namespace ParticlePhysics
{
    /// <summary>
    /// Represents a system of particles which interact with forces.
    /// </summary>
    public partial class ParticleSystem
    {
        List<Particle> particles = new List<Particle>();
        List<Force> forces = new List<Force>();
        List<Constraint> constraints = new List<Constraint>();

        /// <summary>
        /// Builds a particle system with gravity and viscous drag forces. This particle system interacts with the mouse.
        /// </summary>
        /// <returns>The newly built particle system.</returns>
        public static ParticleSystem BuildParticleSystem()
        {
            ParticleSystem ps = new ParticleSystem();
            ps.AddForce(new GravityForce(ps.particles));
            ps.AddForce(new ViscousDragForce(ps.particles));
            ps.AddForce(new MouseSpringForce());
            return ps;
        }

        /// <summary>
        /// Constructs a new particle system. Use BuildParticleSystem() instead of this.
        /// </summary>
        private ParticleSystem() { }

        /// <summary>
        /// Creates a particle given a position and adds it to the particle system.
        /// </summary>
        /// <param name="position">The position of the newly created particle.</param>
        /// <returns>The newly created particle.</returns>
        public Particle CreateParticle(Vector3 position)
        {
            Particle p = new Particle(position);
            particles.Add(p);
            return p;
        }

        /// <summary>
        /// Adds a force to the particle system.
        /// </summary>
        /// <param name="f">The force to add to the particle system.</param>
        public void AddForce(Force f)
        {
            if (f is Constraint)
                constraints.Add((Constraint)f);
            else
            {
                forces.Add(f);
                AddSpecialForce(f);
            }
        }

        /// <summary>
        /// Adds a constraint to the particle system.
        /// </summary>
        /// <param name="c">The constraint to add to the particle system.</param>
        public void AddConstraint(Constraint c)
        {
            constraints.Add(c);
        }

        /// <summary>
        /// Advances the particle system state using the Forward Euler method.
        /// </summary>
        /// <param name="dt">The timestep size.</param>
        private void AdvanceTime(float dt)
        {
            foreach (Particle p in particles)
                p.Force = Vector3.Zero;
            foreach (Force f in forces)
                f.ApplyForce();
            foreach (Constraint c in constraints)
                c.ApplyForce();
            foreach (Particle p in particles)
                p.Update(dt);
        }

        /// <summary>
        /// Resets the particle system.
        /// </summary>
        private void Reset()
        {
            foreach (Particle p in particles)
                p.Reset();
        }

        /// <summary>
        /// Builds a rectangle of pinned particles and spring forces.
        /// </summary>
        /// <param name="topLeft">The top-left corner of the rectangle.</param>
        /// <param name="bottomRight">The bottom-right corner of the rectangle.</param>
        public void CreateRectangleBounds(Vector3 topLeft, Vector3 bottomRight)
        {
            Particle tl = CreateParticle(topLeft);
            Particle tr = CreateParticle(new Vector3(bottomRight.X, topLeft.Y, 0f));
            Particle bl = CreateParticle(new Vector3(topLeft.X, bottomRight.Y, 0f));
            Particle br = CreateParticle(bottomRight);
            AddConstraint(new PinConstraint(tl));
            AddConstraint(new PinConstraint(tr));
            AddConstraint(new PinConstraint(bl));
            AddConstraint(new PinConstraint(br));
            AddForce(new SpringForceStretch(tl, tr));
            AddForce(new SpringForceStretch(tr, br));
            AddForce(new SpringForceStretch(br, bl));
            AddForce(new SpringForceStretch(bl, tl));
        }

        public List<Particle> Particles
        {
            get
            {
                return particles;
            }
        }

        public List<Force> Forces
        {
            get
            {
                return forces;
            }
        }

        public List<Constraint> Constraints
        {
            get
            {
                return constraints;
            }
        }
    }
}
