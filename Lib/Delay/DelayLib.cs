using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Delay
{
    public static class DelayLib
    {
        private const long TicksPerSecond = TimeSpan.TicksPerSecond;
        private const long TicksPerMillisecond = TimeSpan.TicksPerMillisecond;
        private const long TicksPerMicrosecond = TimeSpan.TicksPerMillisecond / 1000;
        private static readonly double s_tickFrequency = (double)TicksPerSecond / Stopwatch.Frequency;
        public static void Delay(TimeSpan time, bool allowThreadYield)
        {
            long start = Stopwatch.GetTimestamp();
            long delta = (long)(time.Ticks / s_tickFrequency);
            long target = start + delta;

            if (!allowThreadYield)
            {
                do
                {
                    Thread.SpinWait(1);
                }
                while (Stopwatch.GetTimestamp() < target);
            }
            else
            {
                SpinWait spinWait = new SpinWait();
                do
                {
                    spinWait.SpinOnce();
                }
                while (Stopwatch.GetTimestamp() < target);
            }
        }
        public static void DelayMilliseconds(int milliseconds, bool allowThreadYield)
        {
            /* We have this as a separate method for now to make calling code clearer
             * and to allow us to add additional logic to the millisecond wait in the
             * future. If waiting only 1 millisecond we still have ample room for more
             * complicated logic. For 1 microsecond that isn't the case.
             */

            var time = TimeSpan.FromTicks(milliseconds * TicksPerMillisecond);
            Delay(time, allowThreadYield);
        }
        public static void DelayMicroseconds(int microseconds, bool allowThreadYield)
        {
            var time = TimeSpan.FromTicks(microseconds * TicksPerMicrosecond);
            Delay(time, allowThreadYield);
        }
    }
}
