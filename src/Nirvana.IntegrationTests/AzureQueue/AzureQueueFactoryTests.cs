using System;
using System.IO;
using Nirvana.AzureQueues.Handlers;
using Nirvana.Configuration;
using Nirvana.CQRS.Queue;
using Nirvana.Domain;
using Nirvana.JsonSerializer;
using Nirvana.Logging;
using Nirvana.Mediation;
using Nirvana.TestFramework;
using Nirvana.Util.Compression;
using Nirvana.Util.Io;
using Nirvana.Util.Tine;
using Should;
using Xunit;
using Xunit.Abstractions;

namespace Nirvana.AzureQueues.IntegrationTests.AzureQueue
{
    public abstract class AzureQueueFactoryTests 
    {
        public  AzureQueueFactory Sut { get; set; }
        public  TestCommand Input { get; set; }
        public  TestCommand Result { get; set; }

        
        private readonly ITestOutputHelper _output;


        protected int MessageCount;
        protected ICompression Compression;
        protected ILogger Logger;
        protected ISerializer Serializer;
        protected IAzureQueueConfiguration AzureQueueConfiguration;
        protected ISystemTime SystemTime;
        protected IQueueController Controller;
        protected IMediatorFactory Mediator;


        protected NirvanaSetup Setup;

        protected AzureQueueFactoryTests(ITestOutputHelper output)
        {
            _output = output;

            Setup = NirvanaSetup.Configure()
                .UsingControllerName("ControlelrName", "Root")
                .WithAssembliesFromFolder(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"))
                .SetAdditionalAssemblyNameReferences(new string[0])
                .SetRootTypeAssembly(this.GetType().Assembly)
                .SetAttributeMatchingFunction(AttributeMatchingFunctionStub)
                .SetDependencyResolver(x=>null, x=>new object[0])
                .ForQueries(MediationStrategy.InProcess, MediationStrategy.InProcess)
                .ForCommands(MediationStrategy.InProcess, MediationStrategy.InProcess, MediationStrategy.None)
                .ForInternalEvents(MediationStrategy.InProcess, MediationStrategy.InProcess, MediationStrategy.None)
                .ForUiNotifications(MediationStrategy.InProcess, MediationStrategy.InProcess, MediationStrategy.None)
                .BuildConfiguration();

            Logger = new TestDebugLogger(output);
            Mediator = new MediatorFactory(Setup);
            Controller = new AzureQueueController(Setup,Logger);
            SystemTime = new SystemTime();
            Compression = new Compression();
            Serializer = new Serializer();
            AzureQueueConfiguration = new AzureQueueConfiguration();

            
            Sut = new AzureQueueFactory(AzureQueueConfiguration,Controller,Serializer,SystemTime,Compression,Mediator,Logger);

            Because();
        }


        public Func<string, object, bool> AttributeMatchingFunctionStub
            => (x, y) => x == ((AggregateRootAttribute)y).RootName;


        

        public void Because  () 
        {
            var typeDef = Setup.FindTypeDefinition(typeof(TestCommand));
            AzureStorageQueue queue = Sut.GetQueue(typeDef) as AzureStorageQueue;
            

            if (queue != null)
            {
                queue.Clear();

                queue.Send(new TestCommand { Test = "this is a test" });

                var cloudMessage = queue.GetAzureMessage();

                var message = cloudMessage != null
                    ? new AzureQueueMessage( cloudMessage, typeDef,Logger,Compression)
                    : null;
                queue.Delete(cloudMessage);
                var result = queue.DeserializeMessage<Message<TestCommand>, TestCommand>(message.Text);
                MessageCount = queue.GetMessageCount();
                Result = result.Body;
            }
        }

        public class when_sending_to_azure_queues : AzureQueueFactoryTests
        {

            [Fact]
            public void should_have_no_messages()
            {
                MessageCount.ShouldEqual(0);
            }
            
            [Fact]
            public void should_send_and_recieve_message()
            {
                Result.Test.ShouldEqual("this is a test");
            }

       

            public when_sending_to_azure_queues(ITestOutputHelper output):base(output)
            {
            }
        }
    }
}