using System;
using TechFu.Nirvana.Domain;

namespace TechFu.Nirvana.Util.Extensions
{
    public static class DomainEntityExtensions
    {
        public static bool IsEnumeration(this Type type)
        {
            return !type.IsAbstract && type.Closes(typeof(Enumeration<,>));
        }

        public static Type GetEnumerationValueType(this Type type)
        {
            Type[] genericTypeArguments;
            return type.Closes(typeof(Enumeration<,>), out genericTypeArguments)
                ? genericTypeArguments[1]
                : null;
        }
    }
}