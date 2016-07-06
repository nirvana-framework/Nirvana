using System;
using System.Collections.Generic;
using System.Linq;

namespace TechFu.Nirvana.Util.Extensions
{
    /// <summary>
    ///     System.Convert extention methods.  Not all types have been implmented.  Add missing types as needed.
    /// </summary>
    public static class TypeConversionExtensions
    {
        //DOUBLE
        public static double ToDouble(this int TIn)
        {
            return Convert.ToDouble(TIn);
        }

        public static double ToDouble(this decimal TIn)
        {
            return Convert.ToDouble(TIn);
        }

        public static double ToDouble(this float TIn)
        {
            return Convert.ToDouble(TIn);
        }

        public static double ToDouble(this short TIn)
        {
            return Convert.ToDouble(TIn);
        }

        public static double ToDouble(this long TIn)
        {
            return Convert.ToDouble(TIn);
        }

        public static double ToDouble(this bool TIn)
        {
            return Convert.ToDouble(TIn);
        }

        public static double ToDouble(this string TIn)
        {
            return Convert.ToDouble(TIn);
        }

        public static double ToDouble(this object TIn)
        {
            return Convert.ToDouble(TIn);
        }

        public static IEnumerable<double> ToDouble<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, decimal> selector)
        {
            return enumerable.Select(selector).Select(Convert.ToDouble);
        }

        public static IEnumerable<double> ToDouble<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, int> selector)
        {
            return enumerable.Select(selector).Select(Convert.ToDouble);
        }

        public static IEnumerable<double> ToDouble<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, short> selector)
        {
            return enumerable.Select(selector).Select(Convert.ToDouble);
        }

        public static IEnumerable<double> ToDouble<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, long> selector)
        {
            return enumerable.Select(selector).Select(Convert.ToDouble);
        }

        public static IEnumerable<double> ToDouble<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, float> selector)
        {
            return enumerable.Select(selector).Select(Convert.ToDouble);
        }

        public static IEnumerable<double> ToDouble<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, bool> selector)
        {
            return enumerable.Select(selector).Select(Convert.ToDouble);
        }

        public static IEnumerable<double> ToDouble<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, object> selector)
        {
            return enumerable.Select(selector).Select(Convert.ToDouble);
        }

        public static IEnumerable<double> ToDouble<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, string> selector)
        {
            return enumerable.Select(selector).Select(double.Parse);
        }

        //DECIMAL
        public static decimal ToDecimal(this int TIn)
        {
            return Convert.ToDecimal(TIn);
        }

        public static decimal ToDecimal(this double TIn)
        {
            return Convert.ToDecimal(TIn);
        }

        public static decimal ToDecimal(this float TIn)
        {
            return Convert.ToDecimal(TIn);
        }

        public static decimal ToDecimal(this short TIn)
        {
            return Convert.ToDecimal(TIn);
        }

        public static decimal ToDecimal(this long TIn)
        {
            return Convert.ToDecimal(TIn);
        }

        public static decimal ToDecimal(this bool TIn)
        {
            return Convert.ToDecimal(TIn);
        }

        public static IEnumerable<decimal> ToDecimal<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, int> selector)
        {
            return enumerable.Select(selector).Select(Convert.ToDecimal);
        }

        public static IEnumerable<decimal> ToDecimal<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, double> selector)
        {
            return enumerable.Select(selector).Select(Convert.ToDecimal);
        }

        public static IEnumerable<decimal> ToDecimal<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, float> selector)
        {
            return enumerable.Select(selector).Select(Convert.ToDecimal);
        }

        public static IEnumerable<decimal> ToDecimal<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, short> selector)
        {
            return enumerable.Select(selector).Select(Convert.ToDecimal);
        }

        public static IEnumerable<decimal> ToDecimal<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, long> selector)
        {
            return enumerable.Select(selector).Select(Convert.ToDecimal);
        }

        public static IEnumerable<decimal> ToDecimal<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, bool> selector)
        {
            return enumerable.Select(selector).Select(Convert.ToDecimal);
        }

        public static IEnumerable<decimal> ToDecimal<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, string> selector)
        {
            return enumerable.Select(selector).Select(decimal.Parse);
        }

        public static decimal ToDecimal(this string TIn)
        {
            return Convert.ToDecimal(TIn);
        }

        //DECIMAL
        public static int ToInt32(this int TIn)
        {
            return Convert.ToInt32(TIn);
        }

        public static int ToInt32(this double TIn)
        {
            return Convert.ToInt32(TIn);
        }

        public static int ToInt32(this float TIn)
        {
            return Convert.ToInt32(TIn);
        }

        public static int ToInt32(this short TIn)
        {
            return Convert.ToInt32(TIn);
        }

        public static int ToInt32(this long TIn)
        {
            return Convert.ToInt32(TIn);
        }

        public static int ToInt32(this bool TIn)
        {
            return Convert.ToInt32(TIn);
        }

        public static IEnumerable<int> ToInt32<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, int> selector)
        {
            return enumerable.Select(selector).Select(Convert.ToInt32);
        }

        public static IEnumerable<int> ToInt32<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, double> selector)
        {
            return enumerable.Select(selector).Select(Convert.ToInt32);
        }

        public static IEnumerable<int> ToInt32<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, float> selector)
        {
            return enumerable.Select(selector).Select(Convert.ToInt32);
        }

        public static IEnumerable<int> ToInt32<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, short> selector)
        {
            return enumerable.Select(selector).Select(Convert.ToInt32);
        }

        public static IEnumerable<int> ToInt32<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, long> selector)
        {
            return enumerable.Select(selector).Select(Convert.ToInt32);
        }

        public static IEnumerable<int> ToInt32<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, bool> selector)
        {
            return enumerable.Select(selector).Select(Convert.ToInt32);
        }

        public static IEnumerable<int> ToInt32<TIn>(this IEnumerable<TIn> enumerable, Func<TIn, string> selector)
        {
            return enumerable.Select(selector).Select(int.Parse);
        }

        public static int ToInt32(this string TIn)
        {
            return Convert.ToInt32(TIn);
        }
    }
}