using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Data;
using TechFu.Nirvana.EventStoreSample.Domain.Domain.ShoppingCart;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.Commands;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.InternalEvents;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.ViewModels;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.ProductCatalog.Command
{
    public class UpdateCartViewModelHandler : BaseNoOpCommandHandler<UpdateCartViewModelCommand>
    {
        private readonly IRepository<object> _repository;
        private readonly IViewModelRepository _viewModelRepository;

        public UpdateCartViewModelHandler(IRepository<object> repository, IViewModelRepository viewModelRepository,
            IChildMediatorFactory mediator) : base(mediator)
        {
            _repository = repository;
            _viewModelRepository = viewModelRepository;
        }

        public override CommandResponse<Nop> Handle(UpdateCartViewModelCommand task)
        {
            var cart = _repository.GetAllAndInclude(new List<Expression<Func<Cart, object>>>
                {
                    x => x.Items,
                    x => x.Coupons
                })
                .OrderByDescending(x => x.Created)
                .FirstOrDefault(x => x.UserId == task.UserId);

            var items = GetItems(cart);
            var viewModel = new CartViewModel
            {
                Id = task.UserId,
                RootEntityKey = task.UserId,
                Items = items,
                SubTotal = items.Sum(x => x.Price)
            };

            _viewModelRepository.Save(viewModel);

            Mediator.InternalEvent(new CartViewModelUpdatedEvent {UserId = task.UserId});

            return CommandResponse.Succeeded(Nop.NoValue);
        }

        private CartItemViewModel[] GetItems(Cart cart)
        {
            return cart.Items.Select(x => new CartItemViewModel
            {
                Id = x.Id,
                RootEntityKey = cart.Id,
                Price = x.Price,
                Name = x.Name,
                Description = x.ShortDescription
            }).ToArray();
        }
    }
}