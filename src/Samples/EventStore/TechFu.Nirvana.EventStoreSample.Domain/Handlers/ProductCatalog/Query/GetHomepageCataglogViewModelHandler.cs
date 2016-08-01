using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Data;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.Queries;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.ViewModels;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.ProductCatalog.Query
{
    public class GetHomepageCataglogViewModelHandler :
        IQueryHandler<GetHomepageCataglogViewModelQuery, HomePageViewModel>
    {
        private readonly IViewModelRepository _repository;

        public GetHomepageCataglogViewModelHandler(IViewModelRepository repository)
        {
            _repository = repository;
        }

        public QueryResponse<HomePageViewModel> Handle(GetHomepageCataglogViewModelQuery query)
        {
            var model = _repository.Get<HomePageViewModel>(NirvanaSetup.ApplicationLevelViewModelKey);
            if (model == null)
            {
                return QueryResponse.Failed<HomePageViewModel>("Could not Find Model");
            }

            return QueryResponse.Succeeded(model);
        }
    }
}