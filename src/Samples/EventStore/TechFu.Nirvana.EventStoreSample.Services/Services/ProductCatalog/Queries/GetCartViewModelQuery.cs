using System;
using Nirvana.CQRS;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.ViewModels;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.Queries
{
    [ProductCatalogRoot(typeof(GetCartViewModelQuery))]
    public class GetCartViewModelQuery : Query<CartViewModel>
    {
        public Guid UserId { get; set; }
    }
}