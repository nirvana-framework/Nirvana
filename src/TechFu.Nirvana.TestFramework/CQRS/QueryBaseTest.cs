using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Domain;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.TestFramework.CQRS
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