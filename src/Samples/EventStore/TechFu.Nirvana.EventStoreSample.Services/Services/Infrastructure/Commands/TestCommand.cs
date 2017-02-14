


using Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Infrastructure.Commands
{
    [InfrastructureRoot(typeof(TestCommand))]
    public class TestCommand:Command<TestResult>
    {
    }
}
