using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.Mediation
{
    public interface IQueryHandler<T, U>
        where T : Query<U>
    {
        QueryResponse<U> Handle(T query);
    }
}