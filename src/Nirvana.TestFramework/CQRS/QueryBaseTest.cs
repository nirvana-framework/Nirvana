using Nirvana.CQRS;
using Nirvana.Domain;
using Nirvana.Mediation;

namespace Nirvana.TestFramework.CQRS
{
    public abstract class QueryBaseTest<TSutType, TCommandType, TOppType,TRootType> : CqrsTestBase<TSutType, TCommandType,TRootType>
        where TSutType : IQueryHandler<TCommandType, TOppType>
        where TCommandType : Query<TOppType>, new() where TRootType : RootType
    {
        protected QueryResponse<TOppType> Result;

        public override void RunTest()
        {
            Result = Sut.Handle(Task);
        }
    }
}