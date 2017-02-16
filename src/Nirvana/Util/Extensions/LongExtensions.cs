using System;

namespace Nirvana.Util.Extensions
{
    public static class LongExtensions
    {
        public static double MillisecondsToDaysPrecise(this long x)
        {
            return TimeSpan.FromMilliseconds(x).TotalDays;
        }

        public static int MillisecondsToDays(this long x)
        {
            return TimeSpan.FromMilliseconds(x).Days;
        }

        public static int ToInt32(this long x)
        {
            return NumericExtensions.ToInt32(x);
        }

        public static long ToMilisecondsFromDays(this long days)
        {
            return (long) TimeSpan.FromDays(days).TotalMilliseconds;
        }
    }
}