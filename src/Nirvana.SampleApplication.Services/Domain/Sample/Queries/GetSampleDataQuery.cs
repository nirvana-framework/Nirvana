using Nirvana.CQRS;
using Nirvana.Data;
using Nirvana.Mediation;
using Nirvana.SampleApplication.Services.Services;

namespace Nirvana.SampleApplication.Services.Domain.Sample.Queries
{
    [SampleServiceRoot(typeof(GetSampleDataQuery))]
    public class GetSampleDataQuery : Query<SampleDataViewModel>
    {
    }

    public class SampleDataViewModel
    {
        public string Message { get; set; }
    }

    public class GetSampleDataHandler : IQueryHandler<GetSampleDataQuery, SampleDataViewModel>
    {
        private readonly DataConfiguration _config;

        public GetSampleDataHandler(DataConfiguration config)
        {
            _config = config;
        }

        public QueryResponse<SampleDataViewModel> Handle(GetSampleDataQuery query)
        {
            return QueryResponse.Success(new SampleDataViewModel
            {
                Message = _config.GetConnectionString("testConnectionString")
            });
        }
    }

}