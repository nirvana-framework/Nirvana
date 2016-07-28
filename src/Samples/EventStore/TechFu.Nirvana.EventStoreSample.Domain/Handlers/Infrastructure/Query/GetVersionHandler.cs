using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Queries;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.Query
{
    public class GetVersionHandler : IQueryHandler<GetVersionQuery, VersionModel>
    {
        public QueryResponse<VersionModel> Handle(GetVersionQuery query)
        {
            return QueryResponse.Succeeded(new VersionModel { Version = "1.0" });
        }
    }
}