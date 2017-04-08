using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Nirvana.Configuration;
using Nirvana.Domain;
using Nirvana.Util.Extensions;

namespace Nirvana.CQRS.Util
{
    public static class CqrsUtils
    {
        public static Type[] QueryTypes(this NirvanaSetup setup, string rootType)
        {
            return FindImplementingTaskTypes(setup, typeof(Query<>), rootType);
        }

        public static Type[] CommandTypes(this NirvanaSetup setup, string rootType)
        {
            return FindImplementingTaskTypes(setup,typeof(Command<>), rootType);
        }

        public static Type[] UiNotificationTypes(this NirvanaSetup setup, string rootType)
        {
            return FindImplementingTaskTypes(setup, typeof(UiNotification<>), rootType);
        }

        public static IEnumerable<Type> GetAllTypes(this NirvanaSetup setup, string rootType)
        {
            var types = new List<Type>();
            types.AddRange(FindImplementingTaskTypes(setup,typeof(Command<>), rootType));
            types.AddRange(FindImplementingTaskTypes(setup,typeof(Query<>), rootType));
            types.AddRange(FindImplementingTaskTypes(setup,typeof(UiNotification<>), rootType));
            types.AddRange(FindImplementingTaskTypes(setup,typeof(InternalEvent), rootType));
            

            return types;
        }

        public static Type GetQueryResultType(Type queryType)
        {
            Type[] genericTypeArguments;
            return queryType.ClosesOrImplements(typeof(Query<>), out genericTypeArguments)
                ? genericTypeArguments[0]
                : null;
        }

        public static Type[] FindImplementingTaskTypes(this NirvanaSetup setup, Type types, string rootType)
        {
            var seedTypes = new[]
            {
                typeof(Command<>).Assembly,
                setup.RootTypeAssembly
            };


            var matchesType = ObjectExtensions.AddAllTypesFromAssembliesContainingTheseSeedTypes(x => x.ClosesOrImplements(types), seedTypes);
            return
                matchesType.Where(x => MatchesRootType(setup,rootType, x))
                    .ToArray();
        }

        public static bool MatchesRootType(this NirvanaSetup setup, string rootType, Type x)
        {
            var customAttribute = CustomAttribute(x);
            return customAttribute != null && setup.AttributeMatchingFunction(rootType, customAttribute);
        }

        public static AggregateRootAttribute CustomAttribute(Type x)
        {
            return Attribute.GetCustomAttribute(x, typeof(AggregateRootAttribute)) as AggregateRootAttribute;
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

        public static string GetApiEndpint(this NirvanaSetup setup, Type type,  string superTypeName)
        {

            return GetApiEndpint(GetRootTypeName(setup,type), type.Name, superTypeName);
        }

        private static string GetRootTypeName(this NirvanaSetup setup, Type type)
        {
            //TODO - this is a bit slow, create a dictionary by type in hte configuration
            foreach (var rootName in setup.RootNames)
            {

                if (MatchesRootType(setup, rootName, type))
                {
                    return rootName;
                }
            }
            throw new InvalidEnumArgumentException("Type does not contain aggregate attribute");
        }
    }
}