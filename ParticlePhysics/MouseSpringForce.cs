using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ParticlePhysics
{
    /// <summary>
    /// Represents a spring force that exists between the affected particle and a point, i.e. the mouse.
    /// </summary>
    public class MouseSpringForce : Force
    {
        Particle p1;
        Vector3 x2;
        float restLength;
        bool activated;

        public MouseSpringForce() { }

        public Particle Particle
        {
            get { return p1; }
            set
            {
                p1 = value;
                activated = p1 != null;
                if (activated)
                    restLength = (x2 - p1.Position).Length();
            }
        }
        public Vector3 MousePosition { get { return x2; } set { x2 = value; } }

        public override void ApplyForce()
        {
            if (activated)
            {
                Vector3 diff = x2 - p1.Position;
                float currentLength = diff.Length();
                diff.Normalize();

                float dvDot = -Vector3.Dot(diff, p1.Velocity);

                diff *= Constants.Physics.STIFFNESS_STRETCH * (currentLength - restLength) + Constants.Physics.DAMPING_MASS * dvDot;

                p1.Force += diff * p1.Mass;
            }
        }

        public override int NumDrawables { get { return activated ? 1 : 0; } }
        public override int DrawableType(int index) { return Constants.Graphics.DRAW_LINE; }
        public override Color DrawableColor(int index) { return Color.LimeGreen; }
        public override Vector3 GetPoint(int index) { return index == 1 ? x2 : p1.Position; }
    }
}
