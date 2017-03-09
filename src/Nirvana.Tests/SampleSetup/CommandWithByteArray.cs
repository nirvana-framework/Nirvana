using Nirvana.CQRS;

namespace Nirvana.Tests.SampleSetup
{
    [TestRoot(typeof(CommandWithByteArray))]
    public class CommandWithByteArray:Command<Nop>
    {
        public byte[] TestInput { get; set; }
    }
}