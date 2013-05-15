using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenTK.Graphics;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace ParticleSandbox
{
    class Program
    {
        static int a;
        static int b;
        static volatile bool _running = true;

        static void Main(string[] args)
        {
            MathNet.Numerics.LinearAlgebra.Double.Solvers.Preconditioners.IncompleteLU precon = new MathNet.Numerics.LinearAlgebra.Double.Solvers.Preconditioners.IncompleteLU();
            //precon
            
        }

        static void IncrementAndDisplayA() { while (_running) Console.WriteLine("a = " + a++); Console.WriteLine("A has stopped running"); }
        static void IncrementAndDisplayB() { while (_running) Console.WriteLine("b = " + b++); Console.WriteLine("B has stopped running"); }
    }
}
