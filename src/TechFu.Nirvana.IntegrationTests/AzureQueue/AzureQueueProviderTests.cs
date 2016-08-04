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

    public class TestCommandHandler : BaseCommandHandler<TestCommand, TestResult>
    {
        public override CommandResponse<TestResult> Handle(TestCommand task)
        {
            if (task.ThrowError)
            {
                throw new NotImplementedException();
            }
            return CommandResponse.Succeeded(new TestResult());
        }

        public TestCommandHandler(IMediatorFactory mediator) : base(mediator)
        {
        }
    }

}