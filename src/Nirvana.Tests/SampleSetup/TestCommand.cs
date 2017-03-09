using Nirvana.CQRS;

namespace Nirvana.Tests.SampleSetup
{
    [TestRoot(typeof(TestCommand))]
    public class TestCommand : Command<Nop>
    {
    }
}