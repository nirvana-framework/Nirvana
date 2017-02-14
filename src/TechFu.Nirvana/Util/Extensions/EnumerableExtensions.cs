using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nirvana.Util.Extensions
{
    public static class EnumerableExtensions
    {
        public static U SafeMax<T, U>(this IEnumerable<T> source, Func<T, U> selector, U defaultValue = default(U))
        {
            return source.Select(selector).DefaultIfEmpty(defaultValue).Max();
        }

        public static decimal SafeSum<T>(this IEnumerable<T> source, Func<T, decimal> selector, int defaultValue = 0)
        {
            return source.Select(selector).DefaultIfEmpty(defaultValue).Sum();
        }

        /// <summary>
        ///     Returns an empty enumerable if the target enumerable is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> target)
        {
            return target ?? Enumerable.Empty<T>();
        }

        /// <summary>
        ///     Creates a new sequence of non-null instances from a sequence that might contain nulls,
        ///     or be null itself.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">an <see cref="IEnumerable{T}" /> to filter for nullity or null instances in the sequence.</param>
        /// <returns>an <see cref="IEnumerable{T}" /> that contains the non-null elements from the input sequence.</returns>
        public static IEnumerable<TSource> SkipNulls<TSource>(this IEnumerable<TSource> source)
            where TSource : class
        {
            return source.EmptyIfNull().Where(e => e != null);
        }

        /// <summary>
        ///     Executes the specified action on each element in the enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeration"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (var item in enumeration)
            {
                action(item);
            }
        }

        /// <summary>
        ///     Executes the specified action on each element in the enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeration"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T, int> action)
        {
            var i = 0;

            foreach (var item in enumeration)
            {
                action(item, i++);
            }
        }

        /// <summary>
        ///     DistinctBy implementation provided by Jon Skeet.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        /// <seealso
        ///     cref="http://stackoverflow.com/questions/520030/why-is-there-no-linq-method-to-return-distinct-values-by-a-predicate" />
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return source.DistinctBy(keySelector, EqualityComparer<TKey>.Default);
        }

        /// <summary>
        ///     DistinctBy implementation provided by Jon Skeet.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        /// <seealso
        ///     cref="http://stackoverflow.com/questions/520030/why-is-there-no-linq-method-to-return-distinct-values-by-a-predicate" />
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }

            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            return DistinctByImpl(source, keySelector, comparer);
        }

        private static IEnumerable<TSource> DistinctByImpl<TSource, TKey>
            (
            IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer)
        {
            var knownKeys = new HashSet<TKey>(comparer);
            return source.Where(element => knownKeys.Add(keySelector(element)));
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection != null && collection.Any();
        }

        public static TResult IfNotEmpty<TInput, TResult>(this IEnumerable<TInput> collection,
            Func<IEnumerable<TInput>, TResult> lambda)
        {
            return !collection.Any() ? default(TResult) : lambda(collection);
        }

        public static TResult IfNotEmpty<TInput, TResult>(this IEnumerable<TInput> collection,
            Func<IEnumerable<TInput>, TResult> lambda, TResult defaultResult)
        {
            return !collection.Any() ? defaultResult : lambda(collection);
        }

        public static IEnumerable<T> Concat<T>(this T item, IEnumerable<T> collection)
        {
            yield return item;

            foreach (var x in collection)
                yield return x;
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T item)
        {
            foreach (var x in source)
                yield return x;

            yield return item;
        }

        public static IEnumerable<dynamic> Flatten<T>(this IEnumerable<T> source)
        {
            return source.Select(x => x.Flatten());
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> getChildren)
        {
            var stack = new Stack<T>();
            foreach (var item in items)
                stack.Push(item);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;

                var children = getChildren(current);
                if (children == null) continue;

                foreach (var child in children)
                    stack.Push(child);
            }
        }

        public static IEnumerable<TResult> Map<T, TResult>(this IEnumerable<T> list, Func<T, TResult> func)
        {
            return list.Select(func);
        }

        public static T Reduce<T, TUpdated>(this IEnumerable<TUpdated> list, Func<TUpdated, T, T> func, T acc)
        {
            return list.Aggregate(acc, (current, i) => func(i, current));
        }

        public static IEnumerable<T> Tail<T>(this IEnumerable<T> enumerable, int nLastElements)
        {
            return enumerable.Skip(Math.Max(0, enumerable.Count() - nLastElements)).Take(nLastElements);
        }

        public static string AsString(this IEnumerable<char> characters)
        {
            return string.Concat(characters);
        }

        public static int IndexOf<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            var i = 0;

            foreach (var item in source)
            {
                if (predicate(item))
                    return i;

                ++i;
            }

            return -1;
        }

        /// <summary>
        ///     Level-order traversal
        /// </summary>
        public static IEnumerable<T> Traverse<T>(this T root, Func<T, IEnumerable<T>> children, bool closure = false)
        {
            var seen = new HashSet<T>();
            var queue = new Queue<T>();
            queue.Enqueue(root);

            while (queue.Count != 0)
            {
                var item = queue.Dequeue();

                if (closure)
                {
                    if (seen.Contains(item))
                        continue;

                    seen.Add(item);
                }

                yield return item;

                foreach (var child in children(item))
                    queue.Enqueue(child);
            }
        }

        public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            foreach (var item in source)
            {
                yield return item;

                if (predicate(item))
                    break;
            }
        }

        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> collection, int batchSize)
        {
            var nextbatch = new List<T>();

            foreach (var item in collection)
            {
                nextbatch.Add(item);
                if (nextbatch.Count != batchSize)
                    continue;

                yield return nextbatch;
                nextbatch = new List<T>(batchSize);
            }

            if (nextbatch.Count > 0)
                yield return nextbatch;
        }

        public static IEnumerable<string> FixedColumn<T>(this IEnumerable<T> source)
        {
            var t = typeof(T);
            var properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var lengths = new Dictionary<string, int>();
            {
                foreach (var property in properties)
                {
                    SetLength(property.Name, lengths, property);
                }
            }

            foreach (var item in source)
            {
                foreach (var property in properties)
                {
                    var value = property.GetValue(item);

                    if (value != null)
                    {
                        SetLength(value, lengths, property);
                    }
                    else
                    {
                        SetLength(string.Empty, lengths, property);
                    }
                }
            }
            var results = new List<string>();

            {
                var line = string.Empty;
                foreach (var property in properties)
                {
                    line += property.Name.PadRight(lengths[property.Name] + 1);
                }
                results.Add(line);
            }

            foreach (var item in source)
            {
                var line = string.Empty;
                foreach (var property in properties)
                {
                    var value = property.GetValue(item);
                    if (value != null)
                    {
                        line += value.ToString().PadRight(lengths[property.Name] + 1);
                    }
                    else
                    {
                        line += string.Empty.PadRight(lengths[property.Name] + 1);
                    }
                }
                results.Add(line);
            }
            return results;
        }

        private static void SetLength(object value, IDictionary<string, int> lengths, PropertyInfo property)
        {
            var len = value.ToString().Length;

            if (lengths.ContainsKey(property.Name))
            {
                if (lengths[property.Name] < len)
                {
                    lengths[property.Name] = len;
                }
            }
            else
            {
                lengths.Add(property.Name, len);
            }
        }

        public static IList ToList(this IEnumerable<object> source, Type listType)
        {
            var list = (IList) Activator.CreateInstance(typeof(List<>).MakeGenericType(listType));

            foreach (var item in source)
            {
                list.Add(item);
            }

            return list;
        }

        public static TSource[] ToArray<TSource>(this IQueryable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return source.Where(predicate).ToArray();
        }

        public static TResult[] SelectToArray<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, TResult> selector, Func<TSource, bool> predicate = null)
        {
            if (predicate == null)
            {
                return source.Select(selector).ToArray();
            }

            return source.Where(predicate).Select(selector).ToArray();
        }

        public static Array ToArray(this IEnumerable<object> source, Type listType)
        {
            var items = source.ToList();

            var array = Array.CreateInstance(listType, items.Count);

            for (var i = 0; i < array.Length; ++i)
                array.SetValue(items[i], i);

            return array;
        }

        public static IEnumerable<IEnumerable<T>> Batch1<T>(this IEnumerable<T> items, int maxItems)
        {
            return items
                .Select((item, inx) => new {item, inx})
                .GroupBy(x => x.inx/maxItems)
                .Select(g => g.Select(x => x.item));
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            var random = new Random();
            var list = source.ToList();

            for (var i = 0; i < list.Count; i++)
            {
                var randomIndex = i + (int) (random.NextDouble()*(list.Count - i));

                var item = list[randomIndex];
                list[randomIndex] = list[i];

                yield return item;
            }
        }
    }
}