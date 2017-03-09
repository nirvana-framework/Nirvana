using System;
using Nirvana.CQRS;
using Nirvana.Mediation;

namespace Nirvana.TestFramework.CQRS
{
    public abstract class BddQueryTestBase<TSutType, TQueryType, TResponseType>
        : BddTestBase<TSutType, TQueryType, QueryResponse<TResponseType>>
        where TQueryType : Query<TResponseType>, new()
        where TSutType : IQueryHandler<TQueryType, TResponseType>

    {
        public override Action Because => () => Result = Sut.Handle(Input);
    }
}