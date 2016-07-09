using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Mediation;
using TechFu.Nirvana.WebApi.Sample.DomainSpecificData.Queries;

namespace TechFu.Nirvana.WebApi.Sample.DomainSpecificData.Handlers
{
    public class GetVersionHandler : IQueryHandler<GetVersionQuery, string>
    {
        public QueryResponse<string> Handle(GetVersionQuery query)
        {
            return QueryResponse.Succeeded("V 1.0");
        }
    }
}