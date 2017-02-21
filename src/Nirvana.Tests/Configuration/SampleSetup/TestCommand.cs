using Nirvana.CQRS;

namespace Nirvana.Tests.Configuration.SampleSetup
{
    [TestRoot(typeof(TestCommand))]
    public class TestCommand : Command<Nop>
    {
    }
}