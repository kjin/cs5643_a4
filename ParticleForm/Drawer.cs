using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using Common;
using ParticlePhysics;

namespace ParticleForm
{
    using Color = System.Drawing.Color;

    /// <summary>
    /// Draws drawable components of a particle system.
    /// </summary>
    public class Drawer
    {
        float scale;
        Pen pen;
        Vector3[] p = new Vector3[10];

        public Drawer(float scale) { pen = new Pen(Color.Black, 2); this.scale = scale; }

        public Drawer(Pen p, float scale) { this.pen = p; this.scale = scale; }

        public void Draw(Graphics g, Drawable d)
        {
            int pi = 0;
            for (int i = 0; i < d.NumDrawables; i++)
            {
                int drawType = d.DrawableType(i);
                if (drawType == Constants.Graphics.DRAW_NOTHING) continue;
                int numPts = Constants.Graphics.DRAW_PT_LOOKUP_TABLE[drawType];
                Color4 c = d.DrawableColor(i);
                pen.Color = Color.FromArgb(c.ToArgb());
                for (int j = 0; j < numPts; j++)
                    p[j] = scale * d.GetPoint(pi++);
                switch (drawType)
                {
                    case Constants.Graphics.DRAW_NOTHING:
                        break;
                    case Constants.Graphics.DRAW_POINT:
                        g.DrawRectangle(pen, p[0].X - 1f, p[0].Y - 1f, 2f, 2f);
                        break;
                    case Constants.Graphics.DRAW_LINE:
                        g.DrawLine(pen, p[0].X, p[0].Y, p[1].X, p[1].Y);
                        break;
                    case Constants.Graphics.DRAW_CIRCLE:
                        g.DrawEllipse(pen, p[0].X, p[0].Y, (p[1].X - p[0].X), (p[1].Y - p[0].Y));
                        break;
                }
            }
        }
    }
}
