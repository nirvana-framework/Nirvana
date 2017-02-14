using System;

namespace Nirvana.Util.Extensions
{
    public static class IntegerExtentions
    {
        // Stolen from: http://stackoverflow.com/questions/2729752/converting-numbers-in-to-words-c-sharp
        private static readonly string[] UnitsMap =
        {
            "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven",
            "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen",
            "Eighteen", "Nineteen"
        };

        private static readonly string[] TensMap =
        {
            "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty",
            "Seventy", "Eighty", "Ninety"
        };

        public static string ToWords(this int number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + Math.Abs(number).ToWords();

            var words = string.Empty;

            if (number/1000000 > 0)
            {
                words += (number/1000000).ToWords() + " Million ";
                number %= 1000000;
            }

            if (number/1000 > 0)
            {
                words += (number/1000).ToWords() + " Thousand ";
                number %= 1000;
            }

            if (number/100 > 0)
            {
                words += (number/100).ToWords() + " Hundred ";
                number %= 100;
            }

            if (number <= 0)
                return words;

            if (words != "")
                words += "and ";


            if (number < 20)
                words += UnitsMap[number];
            else
            {
                words += TensMap[number/10];
                if (number%10 > 0)
                    words += "-" + UnitsMap[number%10];
            }

            return words;
        }

        public static int GreatestCommonFactor(this int a, int b)
        {
            while (b != 0)
            {
                var temp = b;
                b = a%b;
                a = temp;
            }
            return a;
        }

        public static int LeastCommonMultiple(this int a, int b)
        {
            return a/GreatestCommonFactor(a, b)*b;
        }

        public static long ToMsFromDays(this int days)
        {
            return (long) TimeSpan.FromDays(days).TotalMilliseconds;
        }

        // Stolen from: http://stackoverflow.com/questions/20156/is-there-an-easy-way-to-create-ordinals-in-c#
        public static string Ordinal(this int num)
        {
            if (num <= 0) return string.Empty;

            if ((num%100 == 11) || (num%100 == 12) || (num%100 == 13))
            {
                return "th";
            }

            switch (num%10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }
    }
}