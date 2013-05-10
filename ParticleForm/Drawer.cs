using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Common;

namespace ParticleForm
{
    public abstract class Drawer
    {
        protected float scale;

        public Drawer(float scale)
        {
            this.scale = scale;
        }

        public virtual void Reset() { }
    }
}
