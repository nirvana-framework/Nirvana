using System;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;

namespace Nirvana.Util.Extensions
{
    public static class StringExtensions
    {
        public static DateTime ToDate(this string input)
        {
            return DateTime.Parse(input);
        }
  
        public static string ToTitleCase(this string input)
        {
            return input.IfNotNull(i => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(i.ToLower()));
        }

        public static string ToSeparatedWords(this string value)
        {
            return Regex.Replace(value, "([A-Z][a-z])", " $1").Trim();
        }

        public static string RegexReplace(this string value, string pattern, string replacement)
        {
            return Regex.Replace(value, pattern, replacement);
        }

        public static bool EqualsIgnoreCase(this string me, string s)
        {
            return me.ToLower().Equals(s.ToLower());
        }

        public static string Last4Substring(this string input)
        {
            var target = input.Trim();

            if (target.Length < 4)
                throw new ArgumentException("String is less than 4 characters long");

            return target.Substring(target.Length - 4, 4);
        }

        public static bool HasValue(this string val)
        {
            return !string.IsNullOrEmpty(val);
        }

        public static void IfNotNullOrWhiteSpace(this string target, Action<string> lambda)
        {
            if (!string.IsNullOrWhiteSpace(target)) lambda(target);
        }

        public static TResult IfNotNullOrWhiteSpace<TResult>(this string target, Func<string, TResult> lambda)
        {
            return string.IsNullOrWhiteSpace(target) ? default(TResult) : lambda(target);
        }

        public static TResult IfNotNullOrWhiteSpace<TResult>(this string target, Func<string, TResult> lambda,
            TResult defaultResult)
        {
            return string.IsNullOrWhiteSpace(target) ? defaultResult : lambda(target);
        }

        public static void IfNotNullOrEmpty(this string target, Action<string> lambda)
        {
            if (!string.IsNullOrEmpty(target)) lambda(target);
        }

        public static TResult IfNotNullOrEmpty<TResult>(this string target, Func<string, TResult> lambda)
        {
            return string.IsNullOrEmpty(target) ? default(TResult) : lambda(target);
        }

        public static TResult IfNotNullOrEmpty<TResult>(this string target, Func<string, TResult> lambda,
            TResult defaultResult)
        {
            return string.IsNullOrEmpty(target) ? defaultResult : lambda(target);
        }

        public static void IfNullOrEmpty(this string target, Action lambda)
        {
            if (string.IsNullOrEmpty(target)) lambda();
        }

        public static TResult IfNullOrEmpty<TResult>(this string target, Func<TResult> lambda,
            TResult defaultResult = default(TResult))
        {
            return string.IsNullOrEmpty(target) ? lambda() : defaultResult;
        }

        public static SecureString ToSecureString(this string input)
        {
            var secure = new SecureString();
            input.ForEach(secure.AppendChar);
            secure.MakeReadOnly();
            return secure;
        }

        public static bool EqualsOrdinalIgnoreCase(this string a, string b)
        {
            return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }

        public static string Normalize(this string input, Func<char, bool> normalizerAction)
        {
            return string.IsNullOrEmpty(input) ? string.Empty : input.Where(normalizerAction).AsString();
        }
        
        public static string ToCamelCase(this String stringToConvert)
        {
            string[] strings = stringToConvert.Split(' ');
            string result = strings[0];
            for (int i = 1; i < strings.Length; i++)
            {
                string current = strings[i];
                result += current.Substring(0, 1).ToUpper() + current.Substring(1, current.Length - 1);
            }
            return result;
        }
    }

    public static class Normalize
    {
        public static bool NoPunctuation(char input)
        {
            return !char.IsPunctuation(input);
        }

        public static bool NoPunctuationOrWhitespace(char input)
        {
            return !char.IsPunctuation(input) && !char.IsWhiteSpace(input);
        }

        public static bool DigitsOnly(char input)
        {
            return char.IsDigit(input);
        }
    }
}