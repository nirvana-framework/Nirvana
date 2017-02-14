using System;
using System.Globalization;

namespace Nirvana.Util.Extensions
{
    public static class TypeConvert
    {
        public static T? ToScalar<T>(object source) where T : struct
        {
            if (source == null)
                return null;

            if (source is T)
                return source as T?;

            try
            {
                return Convert.ChangeType(source, typeof(T), CultureInfo.InvariantCulture) as T?;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static DateTime? ToDate(dynamic date)
        {
            if (date == null)
                return null;

            int? year = TypeConvert.ToScalar<int>(date.Year);
            if (!year.HasValue)
                return null;

            int? month = TypeConvert.ToScalar<int>(date.Month);
            int? day = TypeConvert.ToScalar<int>(date.Day);

            return new DateTime(year.Value, month ?? 1, day ?? 1);
        }
    }
}