using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.WebApi.Sample.DomainSpecificData.Queries
{
    [AggregateRoot(RootType.Infrastructure)]
    public class GetVersionQuery:Query<string>
    {

    }
}