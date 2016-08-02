using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.UINotifications
{
    [ProductCatalogRoot("HomePageUpdatedUiEvent")]
    public class HomePageUpdatedUiEvent : UiEvent<HomePageUpdatedUiEvent>
    {
    }
}