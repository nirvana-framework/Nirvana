using System;
using System.Collections.Generic;
using System.Reflection;
using Nirvana.CQRS;

namespace Nirvana.Util.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static string WritePrimitiveType(this PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType;
            return WritePrimitiveType(type);
        }

        public static string WritePrimitiveType(this Type type)
        {
            if (type.IsString())
            {
                {
                    return "string";
                }
            }
            if (type.IsType())
            {
                {
                    return "any";
                }
            }
            if (type.IsNumber())
            {
                {
                    return "number";
                }
            }
            if (type.IsDate())
            {
                {
                    return "Date";
                }
            }
            if (IsBoolean(type))
            {
                {
                    return "boolean";
                }
            }


            if (type.IsObject())
            {
                {
                    return "any";
                }
            }

            return string.Empty;
        }


        public static bool IsPrimitiveType(this PropertyInfo propertyInfo)
        {
            var propertyType = propertyInfo.PropertyType;
            return IsPrimitiveType(propertyType);
        }

        public static bool IsPrimitiveType(this Type propertyType)
        {
            return propertyType.IsString()
                   || propertyType.IsNumber()
                   || IsBoolean(propertyType)
                   || propertyType.IsDate()
                   || propertyType.IsType()
                   || IsObject(propertyType);
        }

        public static GenericMatch IsEnumType(this PropertyInfo propertyInfo)
        {
            var propertyType = propertyInfo.PropertyType;
            return IsEnumType(propertyType);
        }

        public static GenericMatch IsEnumType(this Type propertyType)
        {
            Type[] types;
            var isEnumType = propertyType.ClosesOrImplements(typeof(IEnumerable<>), out types);

            var isArrayType = propertyType.IsArray;
            if (isArrayType)
            {
            }

            return new GenericMatch
            {
                Matches = isEnumType,
                Arguments = types
            };
        }

        public static GenericMatch IsPagedResult(this PropertyInfo propertyInfo)
        {
            var propertyType = propertyInfo.PropertyType;
            return IsPagedResult(propertyType);
        }

        public static GenericMatch IsPagedResult(this Type propertyType)
        {
            Type[] types;
            var isEnumType = propertyType.ClosesOrImplements(typeof(PagedResult<>), out types);
            return new GenericMatch
            {
                Matches = isEnumType,
                Arguments = types
            };
        }


        public static bool IsHiddenProperty(this PropertyInfo propertyInfo)
        {
            return propertyInfo.Name == "AuthCode";
        }

        public static bool IsDate(this PropertyInfo prop)
        {
            var propType = prop.PropertyType;
            return IsDate(propType);
        }

        public static bool IsDate(this Type propType)
        {
            return propType == typeof(DateTime)
                   || propType == typeof(DateTime?);
        }
        public static bool IsType(this Type propType)
        {
            return propType == typeof(Type);
        }

        public static bool IsObject(this Type propType)
        {
            return propType == typeof(object)
                   || propType == typeof(object);
        }

        public static bool IsBoolean(this Type propType)
        {
            return propType == typeof(bool)
                   || propType == typeof(bool)
                   || propType == typeof(bool?);
        }

      

        public static bool IsNumber(this PropertyInfo prop)
        {
            var propType = prop.PropertyType;
            return IsNumber(propType);
        }

        public static bool IsNumber(this Type propType)
        {
            return propType == typeof(int)
                   || propType == typeof(int?)
                   || propType == typeof(short)
                   || propType == typeof(short?)
                   || propType == typeof(long)
                   || propType == typeof(long?)
                   || propType == typeof(decimal)
                   || propType == typeof(decimal?)
                   || propType == typeof(double)
                   || propType == typeof(double?)
                   || propType == typeof(float)
                   || propType == typeof(float?);
        }

        public static bool IsString(this PropertyInfo prop)
        {
            var propType = prop.PropertyType;
            return IsString(propType);
        }

        public static bool IsString(this Type propType)
        {
            return propType == typeof(string)
                   || propType == typeof(string)
                   || propType == typeof(Guid)
                   || propType == typeof(Guid?);
        }

        public class GenericMatch
        {
            public bool Matches { get; set; }
            public Type[] Arguments { get; set; }
        }
    }
}