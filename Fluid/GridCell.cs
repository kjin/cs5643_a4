using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace Fluid
{
    public class GridCell
    {
        double[] data = new double[7];

        //positive in fuel-filled regions, zero at reaction zone, negative elsewhere
        public double ImplicitSurface { get { return data[0]; } set { data[0] = value; } }
        public double Temperature { get { return data[1]; } set { data[1] = value; } }
        public double Density { get { return data[2]; } set { data[2] = value; } }
        public double Pressure { get { return data[3]; } set { data[3] = value; } }

        public double VelocityDivergence { get { return data[4]; } set { data[4] = value; } }
        public double PreconditionedValue { get { return data[5]; } set { data[5] = value; } }
        public double Residual { get { return data[6]; } set { data[6] = value; } }
        public double ZValue { get { return data[7]; } set { data[7] = value; } }

        public bool IsFluid;

        public short Adiag;
        public short Aplusi;
        public short Aplusj;

        public double this[int i] { get { return data[i]; } set { data[i] = value; } }

        public void SetInterpolatedValues(GridCell bottomLeft, GridCell topLeft, GridCell bottomRight, GridCell topRight, double xa, double ya)
        {
            double bl = (1 - xa) * (1 - ya);
            double br = xa * (1 - ya);
            double tl = (1 - xa) * ya;
            double tr = xa * ya;
            for (int i = 0; i < 4; i++)
                data[i] = bl * bottomLeft.data[i] + br * bottomRight.data[i] + tl * topLeft.data[i] + tr * topRight.data[i];
        }

        public void Draw()
        {
            GL.Color3(0.0, Pressure, IsFluid ? 1.0 : 0.0);
            GL.Begin(BeginMode.Quads);
            GL.Vertex2(0, 0);
            GL.Vertex2(1, 0);
            GL.Vertex2(1, 1);
            GL.Vertex2(0, 1);
            GL.End();
        }
    }
}
