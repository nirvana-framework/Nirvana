using System;
using Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.InternalEvents
{
    [ProductCatalogRoot(typeof(CartViewModelUpdatedEvent))]
    public class CartViewModelUpdatedEvent : InternalEvent
    {
        public Guid UserId { get; set; }
    }
}