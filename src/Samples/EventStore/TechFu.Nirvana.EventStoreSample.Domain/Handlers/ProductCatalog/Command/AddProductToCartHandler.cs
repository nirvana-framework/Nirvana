using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Data;
using TechFu.Nirvana.EventStoreSample.Domain.Domain.ProductCatalog;
using TechFu.Nirvana.EventStoreSample.Domain.Domain.Security;
using TechFu.Nirvana.EventStoreSample.Domain.Domain.ShoppingCart;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.Commands;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.InternalEvents;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.ProductCatalog.Command
{
    public class AddProductToCartHandler : INopHandler<AddProductToCartCommand>
    {
        private readonly IRepository _repository;
        private readonly IMediatorFactory _mediator;

        public AddProductToCartHandler(IRepository repository, IMediatorFactory mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public CommandResponse<Nop> Handle(AddProductToCartCommand command)
        {

            var user = _repository.Get<SiteUser>(command.UserId);
            var includes = new List<Expression<Func<Cart, object>>>{x=>x.Items};
            var cart =  _repository.Include(_repository.GetAll<Cart>(), includes).FirstOrDefault(x=>x.UserId==command.UserId) ?? new Cart
                        {
                            UserId = user.Id,
                            Id = Guid.NewGuid()
                            
                        };
            var product = _repository.Get<Product>(command.ProductId);

            AddToCart(user, cart, product, command.Quantity);


            _repository.SaveOrUpdate(cart);
            _mediator.InternalEvent(new UserCartUpdatedEvent {UserId = command.UserId});
            return CommandResponse.Succeeded(Nop.NoValue);
        }

        private void AddToCart(SiteUser user, Cart cart, Product product, int quantity)
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