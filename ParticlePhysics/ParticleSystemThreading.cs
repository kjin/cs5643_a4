using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ParticlePhysics
{
    /// <summary>
    /// Contains methods that deal with particle system threading.
    /// </summary>
    public partial class ParticleSystem
    {
        public static long TicksPerUpdate = 10000;
        Mutex mutex = new Mutex();
        volatile bool _running = true;
        Thread _t;

        public void Run()
        {
            Reset();
            Stop();
            _t = new Thread(Update);
            _running = true;
            _t.Start();
        }

        public void Resume()
        {
            Stop();
            _running = true;
            _t.Resume();
        }

        public void Suspend()
        {
            _t.Suspend();
        }

        public void Stop()
        {
            _running = false;
        }

        private void Update()
        {
            _running = true;
            long nextUpdate = DateTime.Now.Ticks;
            float dt = (float)TicksPerUpdate / 10000000f;
            while (_running)
            {
                if (DateTime.Now.Ticks >= nextUpdate)
                {
                    nextUpdate += TicksPerUpdate;
                    mutex.WaitOne();
                    AdvanceTime(dt);
                    mutex.ReleaseMutex();
                }
            }
        }

        public Mutex Mutex
        {
            get
            {
                return mutex;
            }
        }
    }
}
