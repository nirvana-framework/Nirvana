﻿using Nirvana.CQRS;

namespace Nirvana.Mediation
{
    public interface IQueryHandler<T, U>
        where T : Query<U>
    {
        QueryResponse<U> Handle(T query);
    }


    public abstract class QueryHandlerBase<T, U> : IQueryHandler<T, U>
        where T : Query<U>
    {
        protected readonly IMediatorFactory Mediator;

        protected QueryHandlerBase(IChildMediatorFactory mediator)
        {
            Mediator = mediator;
            
        }

        public abstract QueryResponse<U> Handle(T query);
    }

}