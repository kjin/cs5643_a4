using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluid
{
    public class GridCell
    {
        double[] data = new double[6];

        //positive in fuel-filled regions, zero at reaction zone, negative elsewhere
        public double ImplicitSurface { get { return data[0]; } set { data[0] = value; } }
        public double Temperature { get { return data[1]; } set { data[1] = value; } }
        public double Density { get { return data[2]; } set { data[2] = value; } }
        public double Pressure { get { return data[3]; } set { data[3] = value; } }

        public double VelocityDivergence { get { return data[4]; } set { data[4] = value; } }
        public double PreconditionedValue { get { return data[5]; } set { data[5] = value; } }

        public short Adiag;
        public short Aplusi;
        public short Aplusj;

        public void SetInterpolatedValues(GridCell bottomLeft, GridCell topLeft, GridCell bottomRight, GridCell topRight, double xa, double ya)
        {
            double bl = (1 - xa) * (1 - ya);
            double br = xa * (1 - ya);
            double tl = (1 - xa) * ya;
            double tr = xa * ya;
            for (int i = 0; i < 4; i++)
                data[i] = bl * bottomLeft.data[i] + br * bottomRight.data[i] + tl * topLeft.data[i] + tr * topRight.data[i];
        }
    }
}
