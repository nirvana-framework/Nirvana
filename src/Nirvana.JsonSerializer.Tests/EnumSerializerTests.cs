using System;
using System.Linq;
using Nirvana.TestFramework;
using Nirvana.TestFramework.FluentAssertions;
using Xunit;

namespace Nirvana.JsonSerializer.Tests
{
    public abstract class NullValueTests : BddTestBase<Serializer, object, string>
    {
        public override Action Inject => ()=>{};
        public override Action Because=> ()=> { Result = Sut.Serialize(Input, IncludeNulls); };
        protected bool IncludeNulls;

        public class SubClass 
        {
            public string test { get; set; }
        }

        public class TestInput {
            public  string test { get; set; }
            public  SubClass Sub { get; set; }
        }

        public class when_disabled:NullValueTests
        {
            public override Action Establish=> () =>
            {
                Input = new TestInput();
            };

            [Fact]
            public void should_serialize() => Assert.Equal("{}", Result);

        }
        public class when_enabled:NullValueTests
        {
            public override Action Establish=> () =>
            {
                Input = new TestInput();
                IncludeNulls = true;
            };
            [Fact]
            public void should_serialize() => Result.ShouldEqual("{\"test\":null,\"Sub\":null}");
        }
    }

    public class EnumSerializerTests : BddTestBase<Serializer, object, string>
    {

        [Fact]
        public void should_serialize()=> Result.ShouldEqual( "[{\"DisplayName\":\"One\",\"Value\":1},{\"DisplayName\":\"Two\",\"Value\":2}]");
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