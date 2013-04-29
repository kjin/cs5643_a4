using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParticleForm
{
    public class DeflickerPanel : System.Windows.Forms.Panel
    {
        public DeflickerPanel()
        {
            this.SetStyle(System.Windows.Forms.ControlStyles.UserPaint | System.Windows.Forms.ControlStyles.AllPaintingInWmPaint
                | System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);
        }
    }
}