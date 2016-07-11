using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.TestFramework.CQRS
{
    namespace UnitTests.Commands
    {
        public abstract class CommandBaseTest<TSutType, TCommandType, TOppType> : CqrsTestBase<TSutType, TCommandType>
            where TSutType : ICommandHandler<TCommandType, TOppType>
            where TCommandType : Command<TOppType>, new()
        {
            private readonly CqrsTestBase<TSutType, TCommandType> _cqrsTestBase;
            protected CommandResponse<TOppType> Result;

            public override void RunTest()
            {
                Result = Sut.Handle(Task);
            }
        }
    }
}