using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Data;
using TechFu.Nirvana.EventStoreSample.Domain.Handlers.ProductCatalog.Common;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.Queries;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.ViewModels;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.ProductCatalog.Query
{
    public class GetCartViewModelHandler : ViewModelQueryBase<GetCartViewModelQuery, CartViewModel>
    {
        public GetCartViewModelHandler(IViewModelRepository repository, IMediatorFactory mediator) : base(repository, mediator)
        {
        }

        public override QueryResponse<CartViewModel> Handle(GetCartViewModelQuery query)
        {
            return GetFromViewModelRepository(query, x => x.UserId);
        }
    }
}