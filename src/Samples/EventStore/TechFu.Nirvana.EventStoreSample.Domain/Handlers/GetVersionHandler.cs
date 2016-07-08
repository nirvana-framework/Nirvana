using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Queries;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers
{
    public class GetVersionHandler : IQueryHandler<GetVersionQuery, VersionModel>
    {
        public QueryResponse<VersionModel> Handle(GetVersionQuery query)
        {
            return QueryResponse.Succeeded(new VersionModel { Version = "1.0" });
        }
    }
}