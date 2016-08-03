using System;
using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.UINotifications
{
    [ProductCatalogRoot("CartNeedsUpdateUiEvent")]
    public class CartNeedsUpdateUiEvent:UiEvent<CartNeedsUpdateUiEvent>
    {
        public Guid UserId { get; set; }
    }
}
