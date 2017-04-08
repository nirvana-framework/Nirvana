using System;
using System.IO;
using Nirvana.AzureQueues.Handlers;
using Nirvana.Configuration;
using Nirvana.Domain;
using Nirvana.JsonSerializer;
using Nirvana.Mediation;
using Nirvana.Util.Compression;
using Nirvana.Util.Tine;
using Should;
using Xunit;

namespace Nirvana.AzureQueues.IntegrationTests.AzureQueue
{
    public abstract class AzureQueueTests
    {
        public Func<string, object, bool> AttributeMatchingFunctionStub
            => (x, y) => x == ((AggregateRootAttribute)y).RootName;
        protected NirvanaSetup Setup;

        protected AzureQueueTests()
        {
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
        }

        public int messageCount;

        private bool ProcessCommand(TestCommand arg)
        {
            return new TestCommandHandler(new ChildMediatorFactory(Setup)).Handle(arg).Success();
        }

        public class when_a_handler_fails : AzureQueueTests
        {
            public when_a_handler_fails()
            {
                var command = new TestCommand {Test = "test", ThrowError = true};

                var queue =
                    new AzureQueueFactory(new AzureQueueConfiguration(), new AzureQueueController(Setup), new Serializer(),
                        new SystemTime(),
                        new Compression(), new MediatorFactory(Setup)).GetQueue(
                        Setup.FindTypeDefinition(command.GetType()));
                ((AzureStorageQueue) queue).Clear();
                queue.Send(command);

                queue.DoWork<TestCommand>(x => ProcessCommand(command), false, true);
                messageCount = queue.GetMessageCount();
            }

//            [Fact(Skip = "explicit")]
            [Fact]
            public void should_not_remove_message()
            {
                messageCount.ShouldEqual(1);
            }
        }

        public class when_a_handler_succeeds : AzureQueueTests
        {
            

            public when_a_handler_succeeds()
            {
                
                var command = new TestCommand {Test = "test", ThrowError = false};

                var nirvanaTypeDefinition = Setup.FindTypeDefinition(command.GetType());
                var queue =
                    new AzureQueueFactory(new AzureQueueConfiguration(), new AzureQueueController(Setup), new Serializer(), 
                        new SystemTime(),
                        new Compression(), new MediatorFactory(Setup)).GetQueue(nirvanaTypeDefinition);
                ((AzureStorageQueue) queue).Clear();
                queue.Send(command);

                queue.DoWork<TestCommand>(x => ProcessCommand(command), false, true);
                messageCount = queue.GetMessageCount();
            }

            [Fact(Skip = "explicit")]
            public void should_not_remove_message()
            {
                messageCount.ShouldEqual(0);
            }
        }
    }
}