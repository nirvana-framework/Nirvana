


using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Infrastructure.Commands
{
    [InfrastructureRoot("TestCommand")]
    public class TestCommand:Command<TestResult>
    {
    }
}
