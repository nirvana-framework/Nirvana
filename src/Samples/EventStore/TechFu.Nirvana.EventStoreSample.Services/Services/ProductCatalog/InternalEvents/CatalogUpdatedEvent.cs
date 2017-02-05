using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.InternalEvents
{
    [ProductCatalogRoot("CatalogUpdatedEvent")]
    public class CatalogUpdatedEvent : InternalEvent
    {
    }
}