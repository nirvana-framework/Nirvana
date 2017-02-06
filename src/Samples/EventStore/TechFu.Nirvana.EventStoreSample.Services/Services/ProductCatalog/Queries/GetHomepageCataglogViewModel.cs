using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.ViewModels;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.Queries
{
    [ProductCatalogRoot(typeof(GetHomepageCataglogViewModelQuery))]
    public class GetHomepageCataglogViewModelQuery : Query<HomePageViewModel>
    {
    }
}