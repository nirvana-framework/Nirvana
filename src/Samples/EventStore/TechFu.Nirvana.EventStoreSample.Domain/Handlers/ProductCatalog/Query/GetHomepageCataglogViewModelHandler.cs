using Nirvana.Configuration;
using Nirvana.CQRS;
using Nirvana.Data;
using Nirvana.Mediation;
using TechFu.Nirvana.EventStoreSample.Domain.Handlers.ProductCatalog.Common;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.Queries;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.ViewModels;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.ProductCatalog.Query
{
    public class GetHomepageCataglogViewModelHandler :
        ViewModelQueryBase<GetHomepageCataglogViewModelQuery, HomePageViewModel>
    {
        public GetHomepageCataglogViewModelHandler(IViewModelRepository repository, IChildMediatorFactory mediator)
            : base(repository, mediator)
        {
        }


        public override QueryResponse<HomePageViewModel> Handle(GetHomepageCataglogViewModelQuery query)
        {
            return GetFromViewModelRepository(query, q => NirvanaSetup.ApplicationLevelViewModelKey);
        }
    }
}