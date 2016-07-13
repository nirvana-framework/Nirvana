using System;
using Should;
using TechFu.Nirvana.AzureQueues.Handlers;
using TechFu.Nirvana.CQRS.Queue;
using TechFu.Nirvana.EventStoreSample.Infrastructure.Io;
using TechFu.Nirvana.TestFramework;
using TechFu.Nirvana.Util.Compression;
using TechFu.Nirvana.Util.Tine;
using Xunit;

namespace TechFu.Nirvana.IntegrationTests.AzureQueue
{
    public class AzureQueueTests
    {
        public int messageCount;

        private bool ProcessCommand(TestCommand arg)
        {
            return new TestCommandHandler().Handle(arg).Success();
        }

        public class when_a_handler_fails : AzureQueueTests
        {
            public when_a_handler_fails()
            {
                var command = new TestCommand {Test = "test", ThrowError = true};

                var queue =
                    new AzureQueueFactory(new AzureQueueConfiguration(), new Serializer(), new SystemTime(),
                        new Compression()).GetQueue(command.GetType());
                queue.Clear();
                queue.Send(command);

                queue.DoWork<TestCommand>(x => ProcessCommand(command), false, true);
                messageCount = queue.GetMessageCount();
            }
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
                var command = new TestCommand { Test = "test", ThrowError = false };

                var queue =
                    new AzureQueueFactory(new AzureQueueConfiguration(), new Serializer(), new SystemTime(),
                        new Compression()).GetQueue(command.GetType());
                queue.Clear();
                queue.Send(command);

                queue.DoWork<TestCommand>(x => ProcessCommand(command), false, true);
                messageCount = queue.GetMessageCount();
            }
            [Fact]
            public void should_not_remove_message()
            {
                messageCount.ShouldEqual(0);
            }
        }

    }

    public class AzureQueueFactoryTests : TestBase<AzureQueueFactory, TestCommand>
    {
        private int MessageCount;

        protected TestCommand Result;
        private readonly ICompression _compression;
        private readonly Serializer _serializer;
        private readonly AzureQueueConfiguration _azureQueueConfiguration;
        private readonly SystemTime _systemTime;

        public AzureQueueFactoryTests()
        {
            _systemTime = new SystemTime();
            _compression = new Compression();
            _serializer = new Serializer();
            _azureQueueConfiguration = new AzureQueueConfiguration();
            SetupBuildAndRun();
        }

        public override Func<AzureQueueFactory> Build=>() =>new AzureQueueFactory(_azureQueueConfiguration, _serializer, _systemTime, _compression);

        public override void RunTest()
        {
            
            AzureStorageQueue queue = Sut.GetQueue(typeof(TestCommand)) as AzureStorageQueue;


            if (queue != null)
            {
                queue.Clear();

                queue.Send(new TestCommand {Test = "this is a test"});

                var cloudMessage = queue.GetAzureMessage();

                var message = cloudMessage != null ? new AzureQueueMessage(_compression, cloudMessage, typeof(TestCommand)) : null;
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
        }
    }
}