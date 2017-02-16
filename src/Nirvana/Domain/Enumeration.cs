using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Nirvana.Domain
{
    public interface IEnumeration
    {
        object Value { get; }
        string DisplayName { get; }
    }

    [Serializable]
    [DebuggerDisplay("{DisplayName} - {Value}")]
    public abstract class Enumeration<TEnumeration> : Enumeration<TEnumeration, int>
        where TEnumeration : Enumeration<TEnumeration>
    {
        protected Enumeration(int value, string displayName)
            : base(value, displayName)
        {
        }

        public static TEnumeration FromInt32(int value)
        {
            return FromValue(value);
        }

        public static TEnumeration TryFromInt32(int value, TEnumeration defaultValue)
        {
            try
            {
                return FromValue(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static bool TryFromInt32(int listItemValue, out TEnumeration result)
        {
            return TryParse(listItemValue, out result);
        }
    }

    [Serializable]
    [DebuggerDisplay("{DisplayName} - {Value}")]
    public abstract class Enumeration<TEnumeration, TValue> : IComparable<TEnumeration>, IEquatable<TEnumeration>,
        IEnumeration
        where TEnumeration : Enumeration<TEnumeration, TValue>
        where TValue : IComparable
    {
        private static readonly Lazy<TEnumeration[]> Enumerations = new Lazy<TEnumeration[]>(GetEnumerations);

        protected Enumeration(TValue value, string displayName)
        {
            Value = value;
            DisplayName = displayName;
        }

        public TValue Value { get; }

        public int CompareTo(TEnumeration other)
        {
            return Value.CompareTo(other.Value);
        }

        object IEnumeration.Value => Value;
        public string DisplayName { get; }

        public bool Equals(TEnumeration other)
        {
            return other != null && Value.Equals(other.Value);
        }

        public sealed override string ToString()
        {
            return DisplayName;
        }

        public static TEnumeration[] GetAll()
        {
            return Enumerations.Value;
        }

        public static Dictionary<int, string> ToSerializableDictionary<T>(Func<T, bool> restrict = null)
            where T : Enumeration<T>
        {
            var all = Enumeration<T>.GetAll();
            if (restrict != null)
            {
                all = all.Where(restrict).ToArray();
            }
            return all.ToDictionary(x => x.Value, x => x.DisplayName);
        }

        private static TEnumeration[] GetEnumerations()
        {
            var enumerationType = typeof(TEnumeration);
            return enumerationType
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(info => enumerationType.IsAssignableFrom(info.FieldType))
                .Select(info => info.GetValue(null))
                .Cast<TEnumeration>()
                .ToArray();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TEnumeration);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right)
        {
            return !Equals(left, right);
        }

        public static TEnumeration FromValue(TValue value)
        {
            return Parse(value, "value", item => item.Value.Equals(value));
        }

        public static TEnumeration Parse(string displayName)
        {
            return Parse(displayName, "display name", item => item.DisplayName == displayName);
        }

        private static bool TryParse(Func<TEnumeration, bool> predicate, out TEnumeration result)
        {
            result = GetAll().FirstOrDefault(predicate);
            return result != null;
        }

        protected static TEnumeration Parse(object value, string description, Func<TEnumeration, bool> predicate)
        {
            TEnumeration result;

            if (!TryParse(predicate, out result))
            {
                string message = $"'{value}' is not a valid {description} in {typeof(TEnumeration)}";
                throw new ArgumentException(message, "value");
            }

            return result;
        }

        public static bool TryParse(TValue value, out TEnumeration result)
        {
            return TryParse(e => e.Value.Equals(value), out result);
        }

        public static bool TryParse(string displayName, out TEnumeration result)
        {
            return TryParse(e => e.DisplayName == displayName, out result);
        }
    }

    public static class Enumeration
    {
        public static IEnumerable<IEnumeration> GetAll(Type enumerationType)
        {
            var getAllMethod = enumerationType.GetMethod("GetAll",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            return ((IEnumerable) getAllMethod.Invoke(null, new object[0])).Cast<IEnumeration>();
        }

        public static IEnumeration FromValue(Type enumerationType, object value)
        {
            var fromValueMethod = enumerationType.GetMethod("FromValue",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            return (IEnumeration) fromValueMethod.Invoke(null, new[] {value});
        }

        public static IEnumeration Parse(Type enumerationType, string displayName)
        {
            var parseMethod = enumerationType.GetMethod("Parse",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy, null, new[] {typeof(string)},
                null);

            return (IEnumeration) parseMethod.Invoke(null, new object[] {displayName});
        }

        public static bool TryParse(Type enumerationType, string displayName, out IEnumeration enumeration)
        {
            var tryParseMethod = enumerationType.GetMethod("TryParse", BindingFlags.Static | BindingFlags.Public, null,
                new[] {typeof(string), enumerationType.MakeByRefType()}, null);

            var parameters = new object[] {displayName, null};

            var result = (bool) tryParseMethod.Invoke(null, parameters);

            enumeration = (IEnumeration) parameters[1];

            return result;
        }

        public static FieldInfo GetField(Type enumerationType, object enumeration)
        {
            return
                enumerationType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                    .First(x => enumeration == x.GetValue(null));
        }

        public static IEnumeration FromValueOrDisplayName(Type enumerationType, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            int listItemValue;
            return int.TryParse(value, out listItemValue)
                ? FromValue(enumerationType, listItemValue)
                : Parse(enumerationType, value);
        }
    }
}