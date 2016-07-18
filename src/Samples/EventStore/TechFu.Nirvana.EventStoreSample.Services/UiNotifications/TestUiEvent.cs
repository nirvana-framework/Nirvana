using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.UiNotifications
{
    [InfrastructureRoot]
    public class TestUiEvent : UiEvent<TestUiEvent>
    {
        public string Message { get; set; }
    }
}