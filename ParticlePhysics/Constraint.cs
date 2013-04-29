using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParticlePhysics
{
    /// <summary>
    /// Represents a constraint that can be applied to any number of particles.
    /// Constraints are functionally identical to forces, but constraints
    /// are processed only after all forces have been processed.
    /// </summary>
    public abstract class Constraint : Force { }
}
