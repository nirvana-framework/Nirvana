using System.Collections.Generic;
using Xunit;

namespace Nirvana.TestFramework.FluentAssertions
{
    public static class IEnumberableAssertExtensions
    {
        public static void ShouldContain<T>(this IEnumerable<T>input, T assertion)
        {
            Assert.Contains(assertion, input);
        }
    }
}