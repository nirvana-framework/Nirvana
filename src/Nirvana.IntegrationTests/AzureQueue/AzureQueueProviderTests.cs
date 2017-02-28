using System;
using Nirvana.CQRS;
using Nirvana.Mediation;

namespace Nirvana.AzureQueues.IntegrationTests.AzureQueue
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
        public override CommandResponse<TestResult> Exexute(TestCommand task)
        {
            if (task.ThrowError)
            {
                throw new NotImplementedException();
            }
            return CommandResponse.Succeeded(new TestResult());
        }

        public TestCommandHandler(IChildMediatorFactory mediator) : base(mediator)
        {
        }
    }

}