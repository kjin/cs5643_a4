using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ParticlePhysics
{
    /// <summary>
    /// Not sure what I'm really doing here... not that it matters
    /// </summary>
    public class CircleConstraint : Constraint
    {
        Particle p1;
        Vector3 x2;
        float radius;

        public CircleConstraint(Particle p1, Vector3 x2)
        {
            this.p1 = p1;
            this.x2 = x2;
            radius = (x2 - p1.Position).Length();
        }

        public override void ApplyForce()
        {
            Vector3 diff = (x2 - p1.Position);
            //Console.WriteLine(diff.LengthSquared());
            float diff2 = Vector3.Dot(diff, diff);
            //float lambda = -(Vector3.Dot(p1.Force, diff) + p1.Mass * Vector3.Dot(p1.Velocity, p1.Velocity)) / diff2;
            float lambda = -(Vector3.Dot(p1.Force, diff) + p1.Mass * (Vector3.Dot(p1.Velocity, p1.Velocity) + Constants.Physics.STIFFNESS_STRETCH * 0.5f * (radius * radius - diff2) + Constants.Physics.DAMPING_MASS * Vector3.Dot(diff, p1.Velocity))) / diff2;
            p1.Force += lambda * Vector3.Normalize(diff);
        }

        public override int NumDrawables { get { return 2; } }
        public override int DrawableType(int index)
        {
            if (index == 2) return Constants.Graphics.DRAW_LINE;
            if (index == 1) return Constants.Graphics.DRAW_CIRCLE;
            return Constants.Graphics.DRAW_POINT;
        }
        public override Color DrawableColor(int index) { return index == 2 ? Color.Yellow : Color.Blue; }
        public override Vector3 GetPoint(int index)
        {
            if (index % 3 == 0) return p1.Position;
            if (index <= 2) return x2 + (index == 1 ? -1f : 1f) * radius * Vector3.One;
            return x2 - radius * Vector3.Normalize(x2 - p1.Position);
        }
    }
}
