using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ParticlePhysics
{
    /// <summary>
    /// Contains methods that deal with forces that require special interaction with the particle system.
    /// </summary>
    public partial class ParticleSystem
    {
        MouseSpringForce mouseForce;
        GravityForce gravityForce;

        /// <summary>
        /// Adds a force that requires special interaction with the particle system.
        /// </summary>
        /// <param name="f">The force to add.</param>
        private void AddSpecialForce(Force f)
        {
            if (f is MouseSpringForce)
            {
                if (mouseForce != null) throw new ArgumentException();
                else
                    mouseForce = (MouseSpringForce)f;
            }
            else if (f is GravityForce)
            {
                if (gravityForce != null) throw new ArgumentException();
                else
                    gravityForce = (GravityForce)f;
            }
        }

        /// <summary>
        /// Updates the status of the mouse, if this particle system contains a MouseSpringForce.
        /// </summary>
        /// <param name="x">The x coordinate of the mouse position.</param>
        /// <param name="y">The y coordinate of the mouse position.</param>
        /// <param name="pressed">Whether the mouse button is currently pressed.</param>
        public void SetMouseStatus(float x, float y, bool pressed)
        {
            if (mouseForce != null)
            {
                Vector3 v = new Vector3(x, y, 0);
                if (!pressed) mouseForce.Particle = null;
                if (pressed && mouseForce.Particle == null)
                {
                    float min = Single.PositiveInfinity;
                    Particle p1 = null;
                    foreach (Particle p in particles)
                    {
                        float dist = (p.Position - v).LengthSquared();
                        if (dist < min)
                        {
                            p1 = p;
                            min = dist;
                        }
                    }
                    mouseForce.Particle = p1;
                }
                mouseForce.MousePosition = v;
            }
        }
    }
}
