using System;
using Nirvana.CQRS;
using Nirvana.Domain;
using Nirvana.Mediation;

namespace Nirvana.AzureQueues.IntegrationTests.AzureQueue
{
    public class TestRoot : ps
    {
        public override string RootName => "Test";
    }

    public class TestRootAttribute : AggregateRootAttribute
    {
        
        public TestRootAttribute( Type parameterType, bool isPublic = false) : base(new TestRoot(), parameterType, isPublic)
        {
        }
    }
    [TestRoot(typeof(TestCommand))]
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
        public override CommandResponse<TestResult> Execute(TestCommand task)
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