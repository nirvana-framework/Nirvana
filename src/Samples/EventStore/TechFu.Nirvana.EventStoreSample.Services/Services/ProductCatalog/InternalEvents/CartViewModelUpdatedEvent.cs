using System;
using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.InternalEvents
{
    [ProductCatalogRoot("CartViewModelUpdatedEvent")]
    public class CartViewModelUpdatedEvent : InternalEvent
    {
        public Guid UserId { get; set; }
    }
}