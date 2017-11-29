using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nirvana.CQRS;
using Nirvana.Mediation;
using Nirvana.SampleApplication.Services.Domain.Sample.Queries;
using Nirvana.Util.Io;
using Nirvana.Web.Controllers;

namespace Nirvana.SampleApplication.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : CommandQueryApiControllerBase
    {
        private readonly IServiceProvider _provider;

        public ValuesController(IMediatorFactory mediator, ISerializer serializer,IServiceProvider provider) : base(mediator, serializer)
        {
            _provider = provider;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] {"value1", "value2"};
        }

        [HttpGet("Test")]
        public SampleDataViewModel[] Test()
        {
            return new[]
            {
                new SampleDataViewModel {Message = "Hello World!"},
                new SampleDataViewModel {Message = "Hello World!"},
                new SampleDataViewModel {Message = "Hello World!"}
            };
        }

        [HttpGet("Test2")]
        public QueryResponse<SampleDataViewModel> Test2()
        {


            return QueryResponse.Success(new SampleDataViewModel {Message = "Hello World!"});
        }
        [HttpGet("Test3")]
        public QueryResponse<SampleDataViewModel> Test3()
        {
            var handler = _provider.GetService(typeof(IQueryHandler<GetSampleDataQuery,SampleDataViewModel>));
            return (handler as IQueryHandler<GetSampleDataQuery, SampleDataViewModel>).Handle(new GetSampleDataQuery());
        }
        [HttpGet("Test4")]
        public QueryResponse<SampleDataViewModel> Test4()
        {
            return Query(new GetSampleDataQuery());
        }
    }
}