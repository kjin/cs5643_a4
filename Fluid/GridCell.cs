using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluid
{
    public class GridCell
    {
        //positive in fuel-filled regions, zero at reaction zone, negative elsewhere
        public double ImplicitSurface;
        public double Temperature;
        public double Density;
        public double Pressure;

        public double VelocityDivergence;
        public double PreconditionedValue;

        public short Adiag;
        public short Aplusi;
        public short Aplusj;
    }
}
