using System;
using Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.Commands
{
    [ProductCatalogRoot(typeof(AddProductToCartCommand))]
    public class AddProductToCartCommand : NopCommand
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}