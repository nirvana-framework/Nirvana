using System;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Infrastructure.UiNotifications
{
    [InfrastructureRoot("TestUiEvent")]
    public class TestUiEvent : UiEvent<TestUiEvent>
    {
        public string Message { get; set; }
        public override Guid AggregateRoot => NirvanaSetup.ApplicationLevelViewModelKey;
    }
}