using System;
using System.Linq;
using Nirvana.TestFramework;
using Should;
using Xunit;

namespace Nirvana.JsonSerializer.Tests
{
    public class EnumSerializerTests : BddTestBase<Serializer, object, string>
    {

        [Fact]
        public void should_serialize()
        {
            Result.ShouldEqual("[{\"DisplayName\":\"One\",\"Value\":1},{\"DisplayName\":\"Two\",\"Value\":2}]");
        }
        [Fact]
        public void should_deserialize()
        {
            var value = Sut.Deserialize<TestEnum[]>(Result);
            value.Length.ShouldEqual(2);
            value.First().InnerValue.ShouldEqual(TestEnumValue.One);
            value.Last().InnerValue.ShouldEqual(TestEnumValue.Two);
        }
        [Fact]
        public void should_deserialize_bad()
        {
            var value = Sut.Deserialize<TestEnum[]>("[{\"DisplayName\":\"One\",\"Value\":\"\"}]");
            value.Length.ShouldEqual(1);
            value[0].ShouldBeNull();
        }

        public override Action Inject => ()=>{};
        public override Action Because => () =>
        {
            Result = Sut.Serialize(TestEnum.GetAll());
        };
    }
}