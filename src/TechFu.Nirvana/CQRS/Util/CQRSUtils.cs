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

            if (NirvanaSetup.ControllerTypes.Contains(ControllerType.Command))
            {
                types.AddRange(ActionTypes(typeof(Command<>), rootType));
            }
            if (NirvanaSetup.ControllerTypes.Contains(ControllerType.Query))
            {
                types.AddRange(ActionTypes(typeof(Query<>), rootType));
            }
            if (NirvanaSetup.ControllerTypes.Contains(ControllerType.UiNotification))
            {
                types.AddRange(ActionTypes(typeof(UiEvent<>), rootType));
            }

            return types;
        }

        public static Type GetQueryResultType(Type queryType)
        {
            Type[] genericTypeArguments;
            return queryType.Closes(typeof(Query<>), out genericTypeArguments)
                ? genericTypeArguments[0]
                : null;
        }

        public static Type[] ActionTypes(Type types, string rootType)
        {
            var seedTypes = new[] {typeof(Command<>), NirvanaSetup.RootType};


            return
                ObjectExtensions.AddAllTypesFromAssembliesContainingTheseSeedTypes(x => x.Closes(types), seedTypes)
                    .Where(x => MatchesRootType(rootType, x))
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
            return queryType.Closes(closingType, out genericTypeArguments)
                ? genericTypeArguments[0]
                : null;
        }

        public static string GetApiEndpint(string rootTypeName, string actionName, string superTypeName)
        {
            return $"{rootTypeName}/{actionName.Replace(superTypeName, "")}";
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