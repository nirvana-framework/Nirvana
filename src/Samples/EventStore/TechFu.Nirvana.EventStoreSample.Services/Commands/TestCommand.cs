using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Commands
{
    [InfrastructureRoot]
    public class TestCommand:Command<TestResult>
    {
    }

    public class TestResult
    {
        public string Message { get; set; }
    }


    public class TestNotification : UiNotification<TestNotification>
    {
        public string Message { get; set; }
    }
}