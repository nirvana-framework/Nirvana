using System;
using Nirvana.CQRS;
using Nirvana.TestFramework;
using Should;
using Xunit;

namespace Nirvana.JsonSerializer.Tests
{
    public class ExceptionSerializationTests:BddTestBase<Serializer, object, string>
    {

        public class when_serializing_exceptions:ExceptionSerializationTests
        {
            
            public override Action Establish => () =>
            {
                Input = new Exception[]
                {
                    new NotImplementedException("test2"),
                    new NotImplementedException("test1"),
                    new ArgumentNullException()
                };
            };
            [Fact]
            public void should_serialize()
            {
                Result.ShouldEqual("[{\"ClassName\":\"System.NotImplementedException\",\"Message\":\"test2\",\"Data\":null,\"InnerException\":null,\"HelpURL\":null,\"StackTraceString\":null,\"RemoteStackTraceString\":null,\"RemoteStackIndex\":0,\"ExceptionMethod\":null,\"HResult\":-2147467263,\"Source\":null,\"WatsonBuckets\":null},{\"ClassName\":\"System.NotImplementedException\",\"Message\":\"test1\",\"Data\":null,\"InnerException\":null,\"HelpURL\":null,\"StackTraceString\":null,\"RemoteStackTraceString\":null,\"RemoteStackIndex\":0,\"ExceptionMethod\":null,\"HResult\":-2147467263,\"Source\":null,\"WatsonBuckets\":null},{\"ClassName\":\"System.ArgumentNullException\",\"Message\":\"Value cannot be null.\",\"Data\":null,\"InnerException\":null,\"HelpURL\":null,\"StackTraceString\":null,\"RemoteStackTraceString\":null,\"RemoteStackIndex\":0,\"ExceptionMethod\":null,\"HResult\":-2147467261,\"Source\":null,\"WatsonBuckets\":null,\"ParamName\":null}]");
                //var exceptions = new Exception[]
                //{
                //    new NotImplementedException("test2"),
                //    new NotImplementedException("test1"),
                //    new ArgumentNullException()
                //};

          
                //var results = s.Serialize(exceptions);
            }
        }
        public class when_serializing_commandResponse:ExceptionSerializationTests
        {
            
            public override Action Establish => () =>
            {
                Input = new CommandResponse<Nop>();
            };
            [Fact]
            public void should_serialize()
            {
                Result.ShouldEqual("{\"Result\":0,\"ValidationMessages\":[],\"IsValid\":false}");
                //var exceptions = new Exception[]
                //{
                //    new NotImplementedException("test2"),
                //    new NotImplementedException("test1"),
                //    new ArgumentNullException()
                //};

          
                //var results = s.Serialize(exceptions);
            }
        }

        
        
        public override Action Inject => ()=>{};
        public override Action Because=> ()=> { Result = Sut.Serialize(Input, IncludeNulls); };
        public bool IncludeNulls { get; set; }
    }
    public abstract class ExceptionDeSerializationTests:BddTestBase<Serializer, string,object>
    {

        public class when_serializing_exceptions:ExceptionDeSerializationTests
        {
            
            public override Action Because=> ()=> { Result = Sut.Deserialize<Exception[]>(Input); };
            public override Action Establish => () =>
            {

                Input =
                    "[{\"ClassName\":\"System.NotImplementedException\",\"Message\":\"test2\",\"Data\":null,\"InnerException\":null,\"HelpURL\":null,\"StackTraceString\":null,\"RemoteStackTraceString\":null,\"RemoteStackIndex\":0,\"ExceptionMethod\":null,\"HResult\":-2147467263,\"Source\":null,\"WatsonBuckets\":null},{\"ClassName\":\"System.NotImplementedException\",\"Message\":\"test1\",\"Data\":null,\"InnerException\":null,\"HelpURL\":null,\"StackTraceString\":null,\"RemoteStackTraceString\":null,\"RemoteStackIndex\":0,\"ExceptionMethod\":null,\"HResult\":-2147467263,\"Source\":null,\"WatsonBuckets\":null},{\"ClassName\":\"System.ArgumentNullException\",\"Message\":\"Value cannot be null.\",\"Data\":null,\"InnerException\":null,\"HelpURL\":null,\"StackTraceString\":null,\"RemoteStackTraceString\":null,\"RemoteStackIndex\":0,\"ExceptionMethod\":null,\"HResult\":-2147467261,\"Source\":null,\"WatsonBuckets\":null,\"ParamName\":null}]";

                
            };
            [Fact]
            public void should_serialize()
            {
                Result.ShouldNotBeNull();
                //var exceptions = new Exception[]
                //{
                //    new NotImplementedException("test2"),
                //    new NotImplementedException("test1"),
                //    new ArgumentNullException()
                //};

          
                //var results = s.Serialize(exceptions);
            }
        }
        public class when_serializing_commandResponse:ExceptionDeSerializationTests
        {
            
            public override Action Because=> ()=> { Result = Sut.Deserialize<CommandResponse<Nop>>(Input); };
            public override Action Establish => () =>
            {

                Input ="{\"Result\":0,\"ValidationMessages\":[],\"IsValid\":false}";

                
            };
            [Fact]
            public void should_serialize()
            {
                Result.ShouldNotBeNull();
                //var exceptions = new Exception[]
                //{
                //    new NotImplementedException("test2"),
                //    new NotImplementedException("test1"),
                //    new ArgumentNullException()
                //};

          
                //var results = s.Serialize(exceptions);
            }
        }

        
        
        public override Action Inject => ()=>{};
        public bool IncludeNulls { get; set; }
    }
}