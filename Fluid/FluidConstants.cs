using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Fluid
{
    public static class FluidConstants
    {
        public static double DEFAULT_TIMESTEP = 0.001;

        public static double WATER_PRESSURE = 1000;
        public static double GRAVITY_MAGNITUDE = 9.81;

        public static double BUOYANCY = 500;
        public static double VORTICITY = 0.1;
    }
}
