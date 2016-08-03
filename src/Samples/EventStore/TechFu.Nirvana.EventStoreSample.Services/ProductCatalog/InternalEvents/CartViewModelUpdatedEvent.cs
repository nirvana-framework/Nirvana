using System;
using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.InternalEvents
{
    public class CartViewModelUpdatedEvent : InternalEvent
    {
        public Guid UserId { get; set; }
    }
}