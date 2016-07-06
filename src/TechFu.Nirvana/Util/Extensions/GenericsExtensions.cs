using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace TechFu.Nirvana.Util.Extensions
{
    public static class GenericsExtensions
    {
        /// <summary>
        ///     If the <see cref="target" /> value is null, returns the default value for type <see cref="TResult" />.
        ///     Otherwise, invokes the <see cref="lambda" /> expression supplying the <see cref="target" /> as a parameter.
        /// </summary>
        /// <typeparam name="TResult">Type of the result to return.</typeparam>
        /// <typeparam name="TInput">Type for the input.</typeparam>
        /// <param name="target">Target instance for the extension method.</param>
        /// <param name="lambda">Lambda to execute if target is not null.</param>
        /// <returns>
        ///     If target is null, returns default value for <see cref="TResult" />. Else, returns result of
        ///     <see cref="lambda" />
        /// </returns>
        public static TResult IfNotNull<TInput, TResult>(this TInput target, Func<TInput, TResult> lambda)
        {
            return (object) target == null ? default(TResult) : lambda(target);
        }

        /// <summary>
        ///     If the <see cref="target" /> value is null, returns the default value provided for type <see cref="TResult" />.
        ///     Otherwise, invokes the <see cref="lambda" /> expression supplying the <see cref="target" /> as a parameter.
        /// </summary>
        /// <typeparam name="TResult">Type of the result to return.</typeparam>
        /// <typeparam name="TInput">Type for the input.</typeparam>
        /// <param name="target">Target instance for the extension method.</param>
        /// <param name="lambda">Lambda to execute if target is not null.</param>
        /// <param name="defaultResult">The default result value if the target is null.</param>
        /// <returns>
        ///     If target is null, returns default value for <see cref="TResult" />. Else, returns result of
        ///     <see cref="lambda" />
        /// </returns>
        public static TResult IfNotNull<TInput, TResult>(this TInput target, Func<TInput, TResult> lambda,
            TResult defaultResult)
        {
            return (object) target == null ? defaultResult : lambda(target);
        }

        public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0) return min;
            return value.CompareTo(max) > 0 ? max : value;
        }

        /// <summary>Computes the Levenshtein Edit Distance between two enumerables.</summary>
        /// <typeparam name="T">The type of thce items in the enumerables.</typeparam>
        /// <param name="x">The first enumerable.</param>
        /// <param name="y">The second enumerable.</param>
        /// <returns>The edit distance.</returns>
        /// From: http://blogs.msdn.com/b/toub/archive/2006/05/05/590814.aspx
        public static int LevenshteinDistance<T>(this IEnumerable<T> x, IEnumerable<T> y)
            where T : IEquatable<T>
        {
            // Validate parameters
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");

            // Convert the parameters into IList instances
            // in order to obtain indexing capabilities
            var first = x as IList<T> ?? new List<T>(x);
            var second = y as IList<T> ?? new List<T>(y);

            // Get the length of both.  If either is 0, return
            // the length of the other, since that number of insertions
            // would be required.
            int n = first.Count, m = second.Count;
            if (n == 0) return m;
            if (m == 0) return n;

            // Rather than maintain an entire matrix (which would require O(n*m) space),
            // just store the current row and the next row, each of which has a length m+1,
            // so just O(m) space. Initialize the current row.
            int curRow = 0, nextRow = 1;
            var rows = new[] {new int[m + 1], new int[m + 1]};
            for (var j = 0; j <= m; ++j) rows[curRow][j] = j;

            // For each virtual row (since we only have physical storage for two)
            for (var i = 1; i <= n; ++i)
            {
                // Fill in the values in the row
                rows[nextRow][0] = i;
                for (var j = 1; j <= m; ++j)
                {
                    var dist1 = rows[curRow][j] + 1;
                    var dist2 = rows[nextRow][j - 1] + 1;
                    var dist3 = rows[curRow][j - 1] + (first[i - 1].Equals(second[j - 1]) ? 0 : 1);

                    rows[nextRow][j] = Math.Min(dist1, Math.Min(dist2, dist3));
                }

                // Swap the current and next rows
                if (curRow == 0)
                {
                    curRow = 1;
                    nextRow = 0;
                }
                else
                {
                    curRow = 0;
                    nextRow = 1;
                }
            }

            // Return the computed edit distance
            return rows[curRow][m];
        }

        public static string SerializeXml<T>(this T input, Type[] extraTypes = null, Encoding encoding = null,
            XmlSerializerNamespaces namespaces = null)
        {
            var serializer = new XmlSerializer(typeof(T), extraTypes);
            using (var stringWriter = new EncodingStringWriter(encoding ?? Encoding.UTF8))
            {
                serializer.Serialize(stringWriter, input, namespaces);
                return stringWriter.ToString();
            }
        }

        public class EncodingStringWriter : StringWriter
        {
            public EncodingStringWriter(Encoding encoding)
            {
                Encoding = encoding;
            }

            public override Encoding Encoding { get; }
        }
    }
}