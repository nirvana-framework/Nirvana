using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.Util.Extensions;

namespace TechFu.Nirvana.CQRS.Util
{
    public class CqrsUtils
    {
        public static Type[] QueryTypes(string rootType)
        {
            return ActionTypes(typeof(Query<>), rootType);
        }

        public static Type[] CommandTypes(string rootType)
        {
            return ActionTypes(typeof(Query<>), rootType);
        }

        public static Type[] UiNotificationTypes(string rootType)
        {
            return ActionTypes(typeof(UiEvent<>), rootType);
        }

        public static IEnumerable<Type> GetAllTypes(string rootType)
        {
            var types = new List<Type>();
            types.AddRange(ActionTypes(typeof(Command<>), rootType));
            types.AddRange(ActionTypes(typeof(Query<>), rootType));
            types.AddRange(ActionTypes(typeof(UiEvent<>), rootType));
            types.AddRange(ActionTypes(typeof(InternalEvent), rootType));
            

            return types;
        }

        public static Type GetQueryResultType(Type queryType)
        {
            Type[] genericTypeArguments;
            return queryType.ClosesOrImplements(typeof(Query<>), out genericTypeArguments)
                ? genericTypeArguments[0]
                : null;
        }

        public static Type[] ActionTypes(Type types, string rootType)
        {
            var seedTypes = new[]
            {
                typeof(Command<>).Assembly,
                NirvanaSetup.RootType.Assembly,
                NirvanaSetup.RootTypeAssembly
            };


            var matchesType = ObjectExtensions.AddAllTypesFromAssembliesContainingTheseSeedTypes(x => x.ClosesOrImplements(types), seedTypes);
            return
                matchesType.Where(x => MatchesRootType(rootType, x))
                    .ToArray();
        }

        private static bool MatchesRootType(string rootType, Type x)
        {
            var customAttribute = CustomAttribute(x);
            return customAttribute != null && NirvanaSetup.AttributeMatchingFunction(rootType, customAttribute);
        }

        public static Attribute CustomAttribute(Type x)
        {
            return Attribute.GetCustomAttribute(x, NirvanaSetup.AggregateAttributeType);
        }


        public static Type GetResponseType(Type queryType, Type closingType)
        {
            Type[] genericTypeArguments;
            return queryType.ClosesOrImplements(closingType, out genericTypeArguments)
                ? genericTypeArguments[0]
                : null;
        }

        public static string GetApiEndpint(string rootTypeName, string actionName, string superTypeName)
        {

            if (!superTypeName.IsNullOrEmpty())
            {
                actionName = actionName.Replace(superTypeName, "");
            }
            return $"{rootTypeName}/{actionName}";
        }

        public static string GetApiEndpint(Type type,  string superTypeName)
        {

            return GetApiEndpint(GetRootTypeName(type), type.Name, superTypeName);
        }

        private static string GetRootTypeName(Type type)
        {
            //TODO - this is a bit slow, create a dictionary by type in hte configuration
            foreach (var rootName in NirvanaSetup.RootNames)
            {

                if (MatchesRootType(rootName, type))
                {
                    return rootName;
                }
            }
            throw new InvalidEnumArgumentException("Type does not contain aggregate attribute");
        }
    }
}