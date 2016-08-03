using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Data;
using TechFu.Nirvana.EventStoreSample.Domain.Handlers.ProductCatalog.Common;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.Queries;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.ViewModels;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.ProductCatalog.Query
{
    public class GetHomepageCataglogViewModelHandler :
        ViewModelQueryBase<GetHomepageCataglogViewModelQuery, HomePageViewModel>
    {
        public GetHomepageCataglogViewModelHandler(IViewModelRepository repository) : base(repository)
        {
        }

        public override QueryResponse<HomePageViewModel> Handle(GetHomepageCataglogViewModelQuery query)
        {
            return GetFromViewModelRepository(query, q => NirvanaSetup.ApplicationLevelViewModelKey);
        }
    }
   
}