using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.Mediation
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

        protected QueryHandlerBase(IMediatorFactory mediator)
        {
            Mediator = mediator;
            
        }

        public abstract QueryResponse<U> Handle(T query);
    }

}