using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.TestFramework.CQRS
{
    public abstract class QueryBaseTest<TSutType, TCommandType, TOppType> : CqrsTestBase<TSutType, TCommandType>
        where TSutType : IQueryHandler<TCommandType, TOppType>
        where TCommandType : Query<TOppType>, new()
    {
        private readonly CqrsTestBase<TSutType, TCommandType> _cqrsTestBase;
        protected QueryResponse<TOppType> Result;

        public override void RunTest()
        {
            Result = Sut.Handle(Task);
        }
    }
}