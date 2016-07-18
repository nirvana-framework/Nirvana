using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Commands;
using TechFu.Nirvana.EventStoreSample.Services.Shared.UiNotifications;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.Command
{



    public class TestHandler : ICommandHandler<TestCommand, TestResult>
    {
        private readonly IMediatorFactory _mediator;

        public TestHandler(IMediatorFactory mediator)
        {
            _mediator = mediator;
        }

        public CommandResponse<TestResult> Handle(TestCommand command)
        {

            _mediator.Notification(new TestUiEvent());

            return CommandResponse.Succeeded(new TestResult
            {
                Message = "Worked"
            });
        }
    }
}