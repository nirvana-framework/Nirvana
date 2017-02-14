using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Nirvana.Configuration;
using Nirvana.CQRS;
using Nirvana.CQRS.Util;
using Nirvana.Util.Extensions;

namespace Nirvana.Util
{
    public class Angular2CqrsGenerator
    {

        public string GetV2Items()
        {
            var builder = new StringBuilder();

            var emitedTypes = new List<Type>
            {
                //TODO - Paged Result didn't serialize well, moved to the seed project
                //See what went wrong and put it back here...
                typeof(PagedResult<>),
                typeof(ValidationMessage),
                typeof(PaginationQuery),
                typeof(MessageType)
            };

            builder.AppendLine("import {Command,Query,PagedResult,MessageType,ValidationMessage} from \"ng2-nirvana\"; ");

            //builder.AppendLine("//Common");
            //builder.AppendLine(WriteResponseType(typeof(ValidationMessage), new Stack<Type>(), true));
            //builder.AppendLine(WriteResponseType(typeof(PaginationQuery), new Stack<Type>(), true));
            //builder.AppendLine(WriteResponseType(typeof(MessageType), new Stack<Type>()));

            foreach (var controllerName in NirvanaSetup.RootNames)
            {
                WriteController(builder, controllerName, emitedTypes);
            }


            return builder.ToString();
        }

        private void WriteController(StringBuilder builder, string controllerName,
            List<Type> emitedTypes)
        {
            builder.Append($"//{controllerName}" + Environment.NewLine);

            var queryTypes = CqrsUtils. ActionTypes(typeof(Query<>), controllerName);
            var commandTypes = CqrsUtils.ActionTypes(typeof(Command<>), controllerName);
            var uiEventTypes = CqrsUtils.ActionTypes(typeof(UiEvent<>), controllerName);

            WriteTypes(builder, controllerName, emitedTypes, queryTypes, "Query", typeof(Query<>));
            WriteTypes(builder, controllerName, emitedTypes, commandTypes, "Command", typeof(Command<>));
            WriteEvents(builder, controllerName, emitedTypes, commandTypes, "UiEvent", typeof(UiEvent<>));
        }

        private void WriteEvents(StringBuilder builder, string controllerName, List<Type> emitedTypes,
            Type[] eventTypes, string prefix, Type type)
        {
            var subTypes = new Stack<Type>();
            foreach (var queryType in eventTypes)
            {
               //TODO - write event types to Angular
            }
        }

        private void WriteTypes(StringBuilder builder, string controllerName,
            List<Type> emitedTypes,
            Type[] queryTypes, string superType, Type closingType)
        {
            var subTypes = new Stack<Type>();
            foreach (var queryType in queryTypes)
            {
                var responseType = CqrsUtils.GetResponseType(queryType, closingType);
                if (!emitedTypes.Contains(queryType))
                {
                    builder.AppendLine(GetInputType(queryType, responseType, controllerName, superType, subTypes));
                    emitedTypes.Add(queryType);
                }
                if (!responseType.IsPrimitiveType())
                {
                    var checkEnum = responseType.IsEnumType();
                    if (checkEnum.Matches)
                    {
                        responseType = checkEnum.Arguments.First();
                    }

                    if (!emitedTypes.Contains(responseType))
                    {
                        builder.AppendLine(WriteResponseType(responseType, subTypes));
                        emitedTypes.Add(responseType);
                    }
                }


                while (subTypes.Any())
                {
                    var type = subTypes.Pop();
                    if (!emitedTypes.Contains(type))
                    {
                        emitedTypes.Add(type);
                        if (!responseType.IsPrimitiveType())
                        {
                            builder.AppendLine(WriteResponseType(type, subTypes));
                        }
                    }
                }
            }
        }

        private string WriteResponseType(Type queryResponseType, Stack<Type> subTypes,
            bool propertiesAsConstructorArguments = false)
        {
            var builder = new StringBuilder();
            var props = queryResponseType.GetProperties();
            if (queryResponseType.IsEnum)
            {
                builder.Append($"export enum {queryResponseType.Name}{{");
                var dictionary = EnumExtensions.GetAll(queryResponseType);
                var keyList = dictionary.Keys.ToList();
                for (var i = 0; i < ((ICollection) keyList).Count; i++)
                {
                    var key = keyList[i];
                    builder.Append($"{dictionary[key]}={key}");
                    if (i != dictionary.Keys.Count - 1)
                    {
                        builder.Append(",");
                    }
                }
            }
            else
            {
                WriteResponseClass(queryResponseType, subTypes, builder, props, propertiesAsConstructorArguments);
            }


            builder.Append("}");
            return builder.ToString();
        }

        private void WriteResponseClass(Type queryResponseType, Stack<Type> subTypes, StringBuilder builder,
            PropertyInfo[] props, bool propertiesAsConstructorArguments = false)
        {
            builder.Append($"export class {queryResponseType.Name}{{");

            if (propertiesAsConstructorArguments)
            {
                builder.Append("constructor(");
                var count = 0;
                foreach (var propertyInfo in props)
                {
                    if (!propertyInfo.IsHiddenProperty())
                    {
                        builder.Append(WriteConstructorArgument(subTypes, propertyInfo, count));
                        count++;
                    }
                }
                builder.Append("){}");
            }
            else
            {
                foreach (var propertyInfo in props)
                {
                    if (propertiesAsConstructorArguments)
                    {
                    }
                    var buildPublicProperty = BuildPublicProperty(propertyInfo, subTypes);
                    if (!string.IsNullOrWhiteSpace(buildPublicProperty))
                    {
                        builder.Append(buildPublicProperty);
                    }
                }
            }
        }


        private string BuildPublicProperty(PropertyInfo propertyInfo, Stack<Type> subTypes)
        {
            return $"public {propertyInfo.Name}: {GetTypeStriptType(propertyInfo.PropertyType, subTypes)};";
        }

        private string GetTypeStriptType(Type propertyType, Stack<Type> subTypes)
        {
            if (propertyType.IsPrimitiveType())
            {
                return propertyType.WritePrimitiveType();
            }

            var isEnumType = propertyType.IsEnumType();
            if (isEnumType.Matches)
            {
                AddComplexTypes(isEnumType.Arguments, subTypes);

                var type = isEnumType.Arguments.First();
                if (type.IsPrimitiveType())
                {
                    return $"{type.WritePrimitiveType()}[]";
                }
                return $"{type.Name}[]";
            }

            var pagedType = propertyType.IsPagedResult();
            if (pagedType.Matches)
            {
                AddComplexTypes(pagedType.Arguments, subTypes);
                var type = pagedType.Arguments.First();
                if (type.IsPrimitiveType())
                {
                    return $"PagedResult<{type.WritePrimitiveType()}>";
                }
                return $"PagedResult<{pagedType.Arguments.First().Name}>";
            }

            subTypes.Push(propertyType);
            return propertyType.Name;
        }

        private static void AddComplexTypes(Type[] propertyInfo, Stack<Type> subTypes)
        {
            foreach (var type in propertyInfo)
            {
                if (!type.IsPrimitiveType())
                {
                    subTypes.Push(type);
                }
            }
        }

        //queryType, responseType, controllerName, superType, subTypes
        private string GetInputType(Type queryType, Type queryResponseType, string controllerName, string superType,
            Stack<Type> subTypes)
        {
            var builder = new StringBuilder();
            var props = queryType.GetProperties();
            var typeName = queryType.Name;

            var tsTypeName = GetTypeStriptType(queryResponseType, subTypes);

            builder.Append($"export class {typeName} extends {superType}<{tsTypeName}>{{");
            WriteConstructor(controllerName, builder, props, typeName, superType, subTypes);

            builder.Append("}");
            return builder.ToString();
        }

        private void WriteConstructor(string controllerName, StringBuilder builder, PropertyInfo[] props,
            string typeName, string superTypeName, Stack<Type> subTypes)
        {
            builder.Append("constructor(");
            var count = 0;
            foreach (var propertyInfo in props)
            {
                if (!propertyInfo.IsHiddenProperty())
                {
                    builder.Append(WriteConstructorArgument(subTypes, propertyInfo, count));
                    count++;
                }
            }
            //This is a typescript/ javascreipt  hack 
            //can't have class inheritance with the same constructor arguments...
            if (count == 1 && props.First().IsString() && !props.First().IsHiddenProperty())
            {
                builder.Append(",public typescriptPlace: boolean");
            }

            var endpointName = CqrsUtils.GetApiEndpint(controllerName, typeName, superTypeName);

            builder.Append($"){{super('{endpointName}')}}");
        }

        private string WriteConstructorArgument(Stack<Type> subTypes, PropertyInfo propertyInfo, int count)
        {
            var temp = "";

            temp += $"public {propertyInfo.Name}: {GetTypeStriptType(propertyInfo.PropertyType, subTypes)}";
            if (count > 0)
            {
                temp = "," + temp;
            }

            return temp;
        }


    }
}