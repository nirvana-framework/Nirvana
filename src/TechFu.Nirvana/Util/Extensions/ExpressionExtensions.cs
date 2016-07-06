using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TechFu.Nirvana.Util.Extensions
{
    public static class ExpressionExtensions
    {
        public static HashSet<string> GetPropertyNames<T>(params Expression<Func<T, object>>[] properties)
        {
            return new HashSet<string>(properties.Select(x => x.GetProperty().Name));
        }

        public static PropertyInfo GetProperty<T, TValue>(this Expression<Func<T, TValue>> expression)
        {
            return GetProperty(expression.Body);
        }

        public static PropertyInfo GetProperty<TValue>(this Expression<Func<TValue>> expression)
        {
            return GetProperty(expression.Body);
        }

        private static PropertyInfo GetProperty(Expression body)
        {
            MemberExpression memberExpression;
            switch (body.NodeType)
            {
                case ExpressionType.Convert:
                    memberExpression = (MemberExpression) ((UnaryExpression) body).Operand;
                    break;
                case ExpressionType.MemberAccess:
                    memberExpression = (MemberExpression) body;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return (PropertyInfo) memberExpression.Member;
        }

        public static MethodInfo GetMethod<T, TValue>(Expression<Func<T, TValue>> expression)
        {
            var body = expression.Body;

            MethodInfo method;
            switch (body.NodeType)
            {
                case ExpressionType.Call:
                    method = ((MethodCallExpression) body).Method;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return method;
        }
    }
}