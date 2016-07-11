using System;
using System.Linq.Expressions;
using NSubstitute;

namespace TechFu.Nirvana.TestFramework
{
    public class Arg<T>
    {
        public static Arg<T> Is => new Arg<T>();
        public T Anything => Arg.Any<T>();
        public T Null => Arg.Is<T>(x => x == null);

        public static T Matches(Expression<Predicate<T>> predicate)
        {
            return Arg.Is(predicate);
        }

        public T Equal(T value)
        {
            return Arg.Is(value);
        }
    }
}