using Xunit;

namespace Nirvana.TestFramework.FluentAssertions
{
    public static class EqualsExtensions
    {
        public static void ShouldEqual<T>(this T input, T assertion)
        {
            Assert.Equal(assertion, input);
        }
        public static void ShouldBeNull<T>(this T input)
        {
            Assert.Null(input);
        }

        public static void ShouldBeTrue(this bool input)
        {
            Assert.True(input);
        }
    }
}