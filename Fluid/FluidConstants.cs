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
        public static double GRAVITY_MAGNITUDE = 100;

        public static double BUOYANCY = 500;
        public static double VORTICITY = 0.1;
        public static double BLUE_CORE_EMISSION_RATE = 1.0;
    }
}
