using Nirvana.CQRS;
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
        public QueryResponse<SampleDataViewModel> Handle(GetSampleDataQuery query)
        {
            return QueryResponse.Success(new SampleDataViewModel
            {
                Message = "Hello World!"
            });
        }
    }

}