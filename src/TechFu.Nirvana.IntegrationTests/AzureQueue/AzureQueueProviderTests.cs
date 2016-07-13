using System;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.IntegrationTests.AzureQueue
{
    public class TestCommand : Command<TestResult>
    {
        public string Test { get; set; }
        public bool ThrowError { get; set; }
    }

    public class TestResult
    {

    }

    public class TestCommandHandler : ICommandHandler<TestCommand, TestResult>
    {
        public CommandResponse<TestResult> Handle(TestCommand command)
        {
            if (command.ThrowError)
            {
                throw new NotImplementedException();
            }
            return CommandResponse.Succeeded(new TestResult());
        }
    }

}