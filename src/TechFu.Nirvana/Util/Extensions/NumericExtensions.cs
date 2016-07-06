using System;

namespace TechFu.Nirvana.Util.Extensions
{
    public static class NumericExtensions
    {
        public static long ToLong<T>(this T x)
        {
            return Convert.ToInt64(x);
        }

        public static int ToInt32<T>(this T x)
        {
            return Convert.ToInt32(x);
        }

        public static int ToInt32<T>(this T x, int defaultValue)
        {
            int int32;
            try
            {
                int32 = Convert.ToInt32(x);
            }
            catch (Exception)
            {
                int32 = defaultValue;
            }
            return int32;
        }


        public static int ToInt32RoundUp<T>(this T x)
        {
            return Convert.ToInt32(Math.Round(Convert.ToDecimal(x), MidpointRounding.AwayFromZero));
        }

        public static int ToInt32RoundDown<T>(this T x)
        {
            return Convert.ToInt32(Math.Floor(Convert.ToDecimal(x)));
        }
    }
}