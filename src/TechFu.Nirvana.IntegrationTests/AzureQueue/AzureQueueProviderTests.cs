using System;
using Should;
using TechFu.Nirvana.AzureQueues.Handlers;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.CQRS.Queue;
using TechFu.Nirvana.EventStoreSample.Infrastructure.Io;
using TechFu.Nirvana.TestFramework;
using TechFu.Nirvana.Util.Compression;
using TechFu.Nirvana.Util.Tine;
using Xunit;

namespace TechFu.Nirvana.IntegrationTests.AzureQueue
{
    public class AzureQueueFactoryTests : TestBase<AzureQueueFactory, TestCommand>
    {
        public override Func<AzureQueueFactory> Build
            => () => new AzureQueueFactory(new Serializer(), new SystemTime(), new Compression());

        protected TestCommand Result;

        public override void RunTest()
        {
            var queue = Sut.GetQueue(typeof(TestCommand));
            queue
                .Send(new TestCommand { Test = "this is a test" });
            var result = queue.DeserializeMessage<Message<TestCommand>,TestCommand>(queue.GetMessage().Text);
            Result = result.Body;
        }

        public class when_sending_to_azure_queues : AzureQueueFactoryTests
        {
            [Fact]
            public void should_send_and_recieve_message()
            {
                Result.Test.ShouldEqual("this is a test");

            }
        }
    }

    public class TestCommand : Command<TestResult>
    {
        public string Test { get; set; }
    }

    public class TestResult
    {

    }
}