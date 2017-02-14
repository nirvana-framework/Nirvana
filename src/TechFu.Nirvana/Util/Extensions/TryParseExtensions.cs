using System;
using System.Drawing;
using System.Globalization;

namespace Nirvana.Util.Extensions
{
    public static class TryParseExtensions
    {
        public static uint TryParseUInt32(this string value, uint defaultValue = default(uint))
        {
            return TryParse(value, defaultValue, uint.TryParse);
        }

        public static int TryParseInt32(this string value, int defaultValue = default(int))
        {
            return TryParse(value, defaultValue, int.TryParse);
        }

        public static short TryParseInt16(this string value, short defaultValue = default(short))
        {
            return TryParse(value, defaultValue, short.TryParse);
        }

        public static long TryParseInt64(this string value, long defaultValue = default(long))
        {
            return TryParse(value, defaultValue, long.TryParse);
        }

        public static byte TryParseByte(this string value, byte defaultValue = default(byte))
        {
            return TryParse(value, defaultValue, byte.TryParse);
        }

        public static bool TryParseBoolean(this string value, bool defaultValue = default(bool))
        {
            return TryParse(value, defaultValue, bool.TryParse);
        }

        public static float TryParseSingle(this string value, float defaultValue = default(float))
        {
            return TryParse(value, defaultValue, float.TryParse);
        }

        public static double TryParseDouble(this string value, double defaultValue = default(double))
        {
            return TryParse(value, defaultValue, double.TryParse);
        }

        public static decimal TryParseDecimal(this string value, decimal defaultValue = default(decimal))
        {
            return TryParse(value, defaultValue, decimal.TryParse);
        }

        public static decimal TryParseDecimal(this string value, NumberStyles numberStyles,
            decimal defaultValue = default(decimal))
        {
            decimal result;
            if (!decimal.TryParse(value, numberStyles, NumberFormatInfo.CurrentInfo, out result))
                result = defaultValue;

            return result;
        }

        public static DateTime TryParseDateTime(this string value, DateTime defaultValue = default(DateTime))
        {
            return TryParse(value, defaultValue, DateTime.TryParse);
        }

        public static Guid TryParseGuid(this string value, Guid defaultValue = default(Guid))
        {
            return TryParse(value, defaultValue, Guid.TryParse);
        }

        public static Size TryParseSize(this string value, Size defaultValue = default(Size))
        {
            Size result;

            try
            {
                var parts = value.Split(',');
                result = new Size(int.Parse(parts[0]), int.Parse(parts[1]));
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        public static decimal TryParseCurrency(this string value, decimal defaultValue = default(decimal))
        {
            return TryParseDecimal(value, NumberStyles.Currency, defaultValue);
        }

        private static T TryParse<T>(this string value, T defaultValue, ParseDelegate<T> parse) where T : struct
        {
            T result;
            if (!parse(value, out result))
                result = defaultValue;

            return result;
        }

        private delegate bool ParseDelegate<T>(string s, out T result);
    }
}