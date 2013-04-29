using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace ParticlePhysics
{
    /// <summary>
    /// Contains all constant values used in the Particle Simulator.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Physics constants.
        /// </summary>
        public static class Physics
        {
            public readonly static Vector3 GRAVITY_DIRECTION = new Vector3(0f, 1f, 0f);
            public const float GRAVITY_MAGNITUDE = 60f;
            public const float STIFFNESS_STRETCH = 1000000f;
            public const float STIFFNESS_BEND = 1000f;
            public const float DAMPING_MASS = 2f;
        }

        /// <summary>
        /// Constant values for drawing. These are best left untouched.
        /// </summary>
        public static class Graphics
        {
            public const int DRAW_NOTHING = -1;
            public const int DRAW_POINT = 0;
            public const int DRAW_LINE = 1;
            public const int DRAW_CIRCLE = 2;
            public readonly static int[] DRAW_PT_LOOKUP_TABLE = new int[] { 1, 2, 2 };
        }
    }
}
