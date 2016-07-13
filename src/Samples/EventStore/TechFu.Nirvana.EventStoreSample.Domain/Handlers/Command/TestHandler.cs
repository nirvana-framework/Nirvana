using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Commands;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.Command
{
    internal class TestHandler : ICommandHandler<TestCommand, TestResult>
    {
        public CommandResponse<TestResult> Handle(TestCommand command)
        {
            return CommandResponse.Succeeded(new TestResult
            {
                Message="Worked"
            });
        }
    }
}