using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace ParticleForm
{
    public class TextBoxStreamWriter : TextWriter
    {
        TextBox output;

        public TextBoxStreamWriter(TextBox output)
        {
            this.output = output;
        }

        public override void Write(char value)
        {
            base.Write(value);
            output.AppendText(value.ToString());
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
