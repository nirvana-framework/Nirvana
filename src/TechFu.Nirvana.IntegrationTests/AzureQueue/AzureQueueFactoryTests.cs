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

        public override Func<AzureQueueFactory> Build
            =>
            () =>
                new AzureQueueFactory(new AzureQueueConfiguration(), new Serializer(), new SystemTime(),
                    new Compression());

        public override void RunTest()
        {
            var queue = Sut.GetQueue(typeof(TestCommand));
            queue.Clear();

            queue
                .Send(new TestCommand {Test = "this is a test"});
            var message = queue.GetMessage();
            queue.Delete(message);
            var result = queue.DeserializeMessage<Message<TestCommand>, TestCommand>(message.Text);
            MessageCount = queue.GetMessageCount();
            Result = result.Body;
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