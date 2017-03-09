using System;
using Nirvana.CQRS;
using Nirvana.Mediation;

namespace Nirvana.TestFramework.CQRS
{
    public abstract class BddCommandTestBase<TSutType, TQueryType, TResponseType>
        : BddTestBase<TSutType, TQueryType, CommandResponse<TResponseType>>
        where TQueryType : Command<TResponseType>, new()
        where TSutType : ICommandHandler<TQueryType, TResponseType>

    {
        public override Action Because => () => Result = Sut.Handle(Input);
    }
}