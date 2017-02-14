using System;
using Nirvana.Configuration;
using Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Infrastructure.UiNotifications
{
    [InfrastructureRoot(typeof(TestUiEvent))]
    public class TestUiEvent : UiEvent<TestUiEvent>
    {
        public string Message { get; set; }
        public override Guid AggregateRoot => NirvanaSetup.ApplicationLevelViewModelKey;
    }
}