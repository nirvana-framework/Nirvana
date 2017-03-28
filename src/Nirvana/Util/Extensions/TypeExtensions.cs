using System;
using System.Collections.Generic;
using System.Linq;
using Nirvana.CQRS;
using Nirvana.Mediation;

namespace Nirvana.Util.Extensions
{
    public static class TypeExtensions
    {
        public static string GetDisplayName(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return string.Format("{0}?", GetDisplayName(type.GetGenericArguments()[0]));
            if (type.IsGenericType)
                return string.Format("{0}<{1}>",
                    type.Name.Remove(type.Name.IndexOf('`')),
                    string.Join(",", type.GetGenericArguments().Select(at => at.GetDisplayName())));
            if (type.IsArray)
                return string.Format("{0}[{1}]",
                    GetDisplayName(type.GetElementType()),
                    new string(',', type.GetArrayRank() - 1));
            return type.Name;
        }

        public static bool ClosesOrImplements(this Type type, Type genericTypeDefinition)
        {

            if (!genericTypeDefinition.IsGenericTypeDefinition)
            {
                return genericTypeDefinition.IsAssignableFrom(type);
            }

            Type[] genericTypeArguments;
            return ClosesOrImplements(type, genericTypeDefinition, out genericTypeArguments);
        }

        public static bool ClosesOrImplements(this Type type, Type genericTypeDefinition, out Type[] genericTypeArguments)
        {
            if (!genericTypeDefinition.IsGenericTypeDefinition)
                throw new ArgumentException("Type must be a generic type definition", "genericTypeDefinition");

            Predicate<Type> closes = x => x.IsGenericType
                                          && x.GetGenericTypeDefinition() == genericTypeDefinition
                                          &&
                                          x.GenericTypeArguments.Length ==
                                          genericTypeDefinition.GetGenericArguments().Length
                                          && !x.GenericTypeArguments.Any(y => y.IsGenericParameter);

            var typesToConsider = type.Concat(type.GetBaseTypes());

            if (genericTypeDefinition.IsInterface)
                typesToConsider = typesToConsider.Concat(type.GetInterfaces());

            foreach (var baseType in typesToConsider)
            {
                if (closes(baseType))
                {
                    genericTypeArguments = baseType.GenericTypeArguments;
                    return true;
                }
            }

            genericTypeArguments = null;
            return false;
        }

        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            var baseType = type.BaseType;

            while (baseType != null)
            {
                yield return baseType;

                baseType = baseType.BaseType;
            }
        }

        public static IEnumerable<Type> GetAssignableTypes(this Type type)
        {
            return type.Concat(type.GetBaseTypes()).Concat(type.GetInterfaces());
        }



        public static bool IsCommand(this Type type)
        {
            return !type.IsAbstract && type.ClosesOrImplements(typeof(Command<>));
        }
        public static bool IsQuery(this Type type)
        {
            return !type.IsAbstract && type.ClosesOrImplements(typeof(Query<>));
        }
        public static bool IsUiNotification(this Type type)
        {
            return !type.IsAbstract && type.ClosesOrImplements(typeof(UiNotification<>));
        }
        public static bool IsInternalEvent(this Type type)
        {
            return !type.IsAbstract && typeof(InternalEvent).IsAssignableFrom(type);
        }


        public static Type GetCommandResultType(this Type commandType)
        {
            Type[] genericTypeArguments;
            return commandType.ClosesOrImplements(typeof(Command<>), out genericTypeArguments)
                ? genericTypeArguments[0]
                : null;
        }

        public static Type GetCommandType(this Type commandHandlerType)
        {
            Type[] genericTypeArguments;
            return commandHandlerType.ClosesOrImplements(typeof(ICommandHandler<,>), out genericTypeArguments)
                ? genericTypeArguments[0]
                : null;
        }



        public static Type GetQueryResultType(this Type queryType)
        {
            Type[] genericTypeArguments;
            return queryType.ClosesOrImplements(typeof(Query<>), out genericTypeArguments)
                ? genericTypeArguments[0]
                : null;
        }

        public static Type GetQueryType(this Type queryHandlerType)
        {
            Type[] genericTypeArguments;
            return queryHandlerType.ClosesOrImplements(typeof(IQueryHandler<,>), out genericTypeArguments)
                ? genericTypeArguments[0]
                : null;
        }


        public static Type GetListType(this Type listType)
        {
            Type[] genericTypeArguments;
            return listType.ClosesOrImplements(typeof(IEnumerable<>), out genericTypeArguments)
                ? genericTypeArguments[0]
                : null;
        }

        public static Type GetNonNullableType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }
    }
}