using System;
using TechFu.Nirvana.Domain;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.ViewModels
{
    [ProductCatalogRoot("CartViewModel")]
    public class CartViewModel : ViewModel<Guid>
    {
    }
}