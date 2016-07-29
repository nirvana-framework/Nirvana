using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.InternalEvents
{
    [ProductCatalogRoot("CatalogUpdatedEvent")]
    public class CatalogUpdatedEvent : InternalEvent
    {
    }
}