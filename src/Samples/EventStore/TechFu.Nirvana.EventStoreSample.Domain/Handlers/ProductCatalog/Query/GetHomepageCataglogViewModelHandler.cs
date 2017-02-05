using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Data;
using TechFu.Nirvana.EventStoreSample.Domain.Handlers.ProductCatalog.Common;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.Queries;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.ViewModels;
using TechFu.Nirvana.Mediation;

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