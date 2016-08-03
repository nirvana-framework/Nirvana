using System;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.ViewModels;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.Queries
{
    public class GetCartViewModelQuery : Query<CartViewModel>
    {
        public Guid UserId { get; set; }
    }
}