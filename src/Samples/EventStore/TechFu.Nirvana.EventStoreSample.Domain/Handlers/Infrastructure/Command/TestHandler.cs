using System;
using Nirvana.CQRS;
using Nirvana.Mediation;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Infrastructure.Commands;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Infrastructure.UiNotifications;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.Infrastructure.Command
{
    public class TestHandler : BaseCommandHandler<TestCommand, TestResult>
    {
        public TestHandler(IChildMediatorFactory mediator) : base(mediator)
        {
        }

        public override CommandResponse<TestResult> Handle(TestCommand task)
        {
            Mediator.Notification(new TestUiEvent {Message = $"UI NOtification from {Guid.NewGuid()}"});

            return CommandResponse.Succeeded(new TestResult
            {
                Message = "Worked"
            });
        }
    }
}