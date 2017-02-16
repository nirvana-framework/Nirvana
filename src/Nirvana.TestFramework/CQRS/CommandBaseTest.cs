using Nirvana.CQRS;
using Nirvana.Domain;
using Nirvana.Mediation;

namespace Nirvana.TestFramework.CQRS
{
    namespace UnitTests.Commands
    {
        public abstract class CommandBaseTest<TSutType, TCommandType, TOppType,TRootType> : CqrsTestBase<TSutType, TCommandType, TRootType>
            where TSutType : ICommandHandler<TCommandType, TOppType>
            where TCommandType : Command<TOppType>, new() where TRootType : RootType
        {
            protected CommandResponse<TOppType> Result;

            public override void RunTest()
            {
                Result = Sut.Handle(Task);
            }
        }
    }
}