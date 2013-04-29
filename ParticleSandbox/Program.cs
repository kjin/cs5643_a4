using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ParticleSandbox
{
    class Program
    {
        static int a;
        static int b;
        static volatile bool _running = true;

        static void Main(string[] args)
        {
            Thread ta = new Thread(IncrementAndDisplayA);
            Thread tb = new Thread(IncrementAndDisplayB);
            ta.Start();
            tb.Start();
            Thread.Sleep(10000);
            _running = false;
            Console.ReadLine();

        }

        static void IncrementAndDisplayA() { while (_running) Console.WriteLine("a = " + a++); Console.WriteLine("A has stopped running"); }
        static void IncrementAndDisplayB() { while (_running) Console.WriteLine("b = " + b++); Console.WriteLine("B has stopped running"); }
    }
}
