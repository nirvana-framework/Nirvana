using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Nirvana.Util.Extensions
{
    public static class ObjectExtensions
    {


     
        public static T[] AsArray<T>(this T input)
        {
            return new[] {input};
        }

        public static IEnumerable<T> AsIEnumerable<T>(this T input)
        {
            return new List<T> {input};
        }

        public static IQueryable<T> AsIQueryable<T>(this T input)
        {
            return AsIEnumerable(input).AsQueryable();
        }


        public static string ToStringExtension(this object obj)
        {
            var sb = new StringBuilder();

            foreach (var property in obj.GetType().GetProperties())
            {
                sb.Append(property.Name);
                sb.Append(": ");

                if (property.GetIndexParameters().Length > 0)
                {
                    sb.Append("Indexed Property cannot be used");
                }
                else
                {
                    sb.Append(property.GetValue(obj, null));
                }

                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

     

        public static void SetProperty(this object target, string name, object value)
        {
            object[] values = {value};
            var targetProperty = target.GetType().GetProperty(name);

            if (targetProperty == null)
            {
                throw new Exception("Object " + target + "   does not have Target Property " + name);
            }

            targetProperty.GetSetMethod().Invoke(target, values);
        }

        public static object GetProperty(this object target, string name)
        {
            return GetProperty(target, name, false);
        }

        public static object GetProperty(this object target, string name, bool throwError)
        {
            var targetProperty = target.GetType().GetProperty(name);

            if (targetProperty == null)
            {
                if (throwError)
                {
                    throw new Exception("Object " + target + "   does not have Target Property " + name);
                }
                return null;
            }

            return targetProperty.GetGetMethod().Invoke(target, null);
        }

        public static dynamic Flatten(this object instance)
        {
            var builder = ReflectionExtensions.GetTypeBuilder();
            var propertyList = new Dictionary<string, object>();

            // make the type
            instance.Flatten(builder, propertyList);
            var t = builder.CreateTypeInfo();
            var obj = Activator.CreateInstance(t);
            propertyList.ForEach(x => obj.SetProperty(x.Key, x.Value));

            return obj;
        }

        private static void Flatten(this object instance, TypeBuilder builder, Dictionary<string, object> propertyList)
        {
            var vals = instance as IEnumerable;

            if (propertyList == null)
                propertyList = new Dictionary<string, object>();

            if (vals != null && !vals.GetType().IsValueType && !(vals is string))
            {
                foreach (var val in (IEnumerable) instance)
                {
                    val.Flatten(builder, propertyList);
                }
            }
            else
            {
                var props =
                    instance.GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var pi in props)
                {
                    var value = pi.GetIndexParameters().IsNullOrEmpty()
                        ? pi.GetValue(instance)
                        : pi.GetValue(instance,
                            Enumerable.Range(0, pi.GetIndexParameters().Count()).Select(x => (object) x).ToArray());
                    if (!pi.GetIndexParameters().IsNullOrEmpty() && !(value is string))
                    {
                        value.Flatten(builder, propertyList);
                    }
                    else if (pi.PropertyType.GetInterface("IEnumerable").IfNotNull(x => true, false) &&
                             !(value is string))
                    {
                        if (value is IEnumerable)
                        {
                            value.Flatten(builder, propertyList);
                        }
                    }
                    else if (!propertyList.ContainsKey(pi.Name) && (value.GetType().IsValueType || value is string))
                    {
                        builder.AddProperty(pi.Name, pi.PropertyType);
                        propertyList[pi.Name] = value;
                    }
                    else
                    {
                        value.Flatten(builder, propertyList);
                    }
                }
            }
        }

       
        public static List<Type> AddAllTypesFromAssembliesContainingTheseSeedTypes(
            Func<Type, bool> exclusionExpression, params Type[] types)
        {
            return types.SelectMany(t => t.Assembly.GetTypes())
                .Where(exclusionExpression).ToList();
        }
       
        public static List<Type> AddAllTypesFromAssembliesContainingTheseSeedTypes(
            Func<Type, bool> exclusionExpression, params Assembly[] assemblies)
        {
            return assemblies.SelectMany(t => t.GetTypes())
                .Where(exclusionExpression).ToList();
        }
    }
}