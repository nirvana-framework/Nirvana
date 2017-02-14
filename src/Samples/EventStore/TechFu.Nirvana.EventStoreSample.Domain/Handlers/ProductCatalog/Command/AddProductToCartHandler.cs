using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Nirvana.CQRS;
using Nirvana.Data;
using Nirvana.Mediation;
using TechFu.Nirvana.EventStoreSample.Domain.Domain.ProductCatalog;
using TechFu.Nirvana.EventStoreSample.Domain.Domain.Security;
using TechFu.Nirvana.EventStoreSample.Domain.Domain.ShoppingCart;
using TechFu.Nirvana.EventStoreSample.Services.Shared;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.Commands;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.InternalEvents;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Security.Command;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.ProductCatalog.Command
{
    public class AddProductToCartHandler : BaseNoOpCommandHandler<AddProductToCartCommand>
    {
        private readonly IRepository<ProductCatalogRoot> _repository;

        public AddProductToCartHandler(IRepository<ProductCatalogRoot> repository, IChildMediatorFactory mediator) : base(mediator)
        {
            _repository = repository;
        }

        public override CommandResponse<Nop> Handle(AddProductToCartCommand task)
        {
            var user = _repository.Get<SiteUser>(task.UserId);

            if (user == null)
                Mediator.Command(new CreateAnonymousUserCommand
                {
                    SessionId = task.UserId
                });

            var cart = _repository
                           .GetAllAndInclude(new List<Expression<Func<Cart, object>>> {x => x.Items})
                           .FirstOrDefault(x => x.UserId == task.UserId) ?? new Cart
                       {
                           UserId = task.UserId,
                           Id = Guid.NewGuid()
                       };
            var product = _repository.Get<Product>(task.ProductId);

            AddToCart(cart, product, task.Quantity);


            _repository.SaveOrUpdate(cart);
            Mediator.InternalEvent(new UserCartUpdatedEvent {UserId = task.UserId});
            return CommandResponse.Succeeded(Nop.NoValue);
        }

        private void AddToCart(Cart cart, Product product, int quantity)
        {
            var current = cart.Items.FirstOrDefault(x => x.ProductId == product.Id);


            if (current != null)
            {
                current.Quantity += quantity;
                return;
            }
            cart.Items.Add(new CartItem
            {
                ProductId = product.Id,
                Id = Guid.NewGuid(),
                Cart = cart,
                Price = product.BasePrice,
                Quantity = quantity,
                ShortDescription = product.ShortDescription
            });
        }
    }
}