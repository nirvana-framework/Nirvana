using Nirvana.Domain;

namespace Nirvana.JsonSerializer.Tests
{
    public enum TestEnumValue
    {
        One=1,
        Two=2
    }

    public class TestEnum:Enumeration<TestEnum>
    {
        public static TestEnum One = new TestEnum(TestEnumValue.One,"One");
        public static TestEnum Two = new TestEnum(TestEnumValue.Two,"Two");
        public TestEnum(TestEnumValue value, string displayName) : base((int)value, displayName)
        {
        }

        public static TestEnum FromInnerValue(TestEnumValue value)
        {
            return TestEnum.FromValue((int) value);
        }

        public TestEnumValue InnerValue => (TestEnumValue) Value;
    }
}