using Nirvana.CQRS;

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
}