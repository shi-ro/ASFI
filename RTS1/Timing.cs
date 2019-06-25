using System;
using System.Diagnostics;

namespace RTS1
{
    public class Timing
    {
        public TimeSpan Elapsed { get; private set; } = TimeSpan.Zero;
        public TimeSpan RealElapsed => Sw.Elapsed;
        public TimeSpan Dt => Elapsed - _prevElapsed;
        public static Timing DrawTiming { get; } = new Timing();
        public static Timing UpdateTiming { get; } = new Timing();

        private Timing() { }
        private readonly Stopwatch Sw = Stopwatch.StartNew();
        private TimeSpan _prevElapsed = TimeSpan.Zero;

        public void Reset()
        {
            Sw.Restart();
            Elapsed = _prevElapsed = TimeSpan.Zero;
        }
        
        public void Update()
        {
            _prevElapsed = Elapsed;
            Elapsed = Sw.Elapsed;
        }
    }
}

