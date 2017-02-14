using System;
using Nirvana.Domain;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.ViewModels
{
    [ProductCatalogRoot(typeof(CartViewModel))]
    public class CartViewModel : ViewModel<Guid>
    {
        public CartItemViewModel[] Items { get; set; }
        public decimal SubTotal{ get; set; }
    }

    public class CartItemViewModel : ViewModel<Guid>
    {
        public string Description { get; set; }
        public string Name{ get; set; }
        public decimal Price{ get; set; }
    }
}