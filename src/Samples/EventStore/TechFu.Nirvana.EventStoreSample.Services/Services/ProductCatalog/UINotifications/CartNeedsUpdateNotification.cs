using System;
using Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.UINotifications
{
    [ProductCatalogRoot(typeof(CartNeedsUpdateUiEvent))]
    public class CartNeedsUpdateUiEvent:UiEvent<CartNeedsUpdateUiEvent>
    {
        public Guid UserId { get; set; }
        public override Guid AggregateRoot => UserId;
    }
}
