using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.ViewModels;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.Queries
{
    [ProductCatalogRoot("GetHomepageCataglogViewModelQuery")]
    public class GetHomepageCataglogViewModelQuery : Query<HomePageViewModel>
    {
    }
}