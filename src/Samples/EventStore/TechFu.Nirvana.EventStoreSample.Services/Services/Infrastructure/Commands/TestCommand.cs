


using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Infrastructure.Commands
{
    [InfrastructureRoot("TestCommand")]
    public class TestCommand:Command<TestResult>
    {
    }
}
