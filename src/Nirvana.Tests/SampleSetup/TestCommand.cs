using System;
using Nirvana.CQRS;
using Nirvana.CQRS.UiNotifications;

namespace Nirvana.Tests.SampleSetup
{
    [TestRoot(typeof(TestCommand))]
    public class TestCommand : Command<Nop>
    {
    }

    [TestRoot(typeof(TestLongRunningCommand),LongRunning = true)]
    public class TestLongRunningCommand : Command<Nop>
    {
    }

    [TestRoot(typeof(TestQuery),LongRunning = true)]
    public class TestQuery: Query<object>
    {
    }
    [TestRoot(typeof(TestEvent),LongRunning = true)]
    public class TestEvent: InternalEvent
    {
    }
    [TestRoot(typeof(TestEvent),LongRunning = true)]
    public class TestUiNotification: UiNotification<ClientEventType>
    {
        public override Guid AggregateRoot =>Guid.Parse("bbb1cac5-224f-470c-9a88-35b97e86bbe1");
    }

    public class ClientEventType
    {
        public string TestResponseValue { get; set; }
    }
}