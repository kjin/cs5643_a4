using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;

namespace ParticlePhysics
{
    /// <summary>
    /// Represents a spring force that exists between the affected particle and a point, i.e. the mouse.
    /// </summary>
    public class MouseSpringForce : Force
    {
        ParticleSystem PS;
        Particle p1;
        Vector3 x2;
        float restLength;
        bool activated;

        public MouseSpringForce(ParticleSystem PS) { this.PS = PS; }

        public override void ApplyForce()
        {
            x2 = new Vector3(PS.MousePosition);
            if (!PS.MouseDown) p1 = null;
            if (PS.MouseDown && p1 == null)
            {
                float min = Single.PositiveInfinity;
                p1 = null;
                foreach (Particle p in PS.Particles)
                {
                    float dist = (p.Position - x2).LengthSquared;
                    if (dist < min)
                    {
                        p1 = p;
                        min = dist;
                        restLength = (x2 - p1.Position).LengthSquared;
                    }
                }
                restLength = (float)Math.Sqrt(restLength);
            }
            activated = p1 != null;
            if (activated)
            {
                Vector3 diff = x2 - p1.Position;
                float currentLength = diff.Length;
                diff.Normalize();

                float dvDot = -Vector3.Dot(diff, p1.Velocity);

                diff *= Constants.Physics.STIFFNESS_STRETCH * (currentLength - restLength) + Constants.Physics.DAMPING_MASS * dvDot;

                p1.Force += diff * p1.Mass;
            }
        }

        public override int NumDrawables { get { return activated ? 1 : 0; } }
        public override int DrawableType(int index) { return Constants.Graphics.DRAW_LINE; }
        public override Color4 DrawableColor(int index) { return Color4.LimeGreen; }
        public override Vector3 GetPoint(int index) { return index == 1 ? x2 : p1.Position; }
    }
}
