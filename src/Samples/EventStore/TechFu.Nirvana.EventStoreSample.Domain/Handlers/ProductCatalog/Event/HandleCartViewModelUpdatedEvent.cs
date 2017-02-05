using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.InternalEvents;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.UINotifications;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.ProductCatalog.Event
{
    public class HandleCartViewModelUpdatedEvent : BaseEventHandler<CartViewModelUpdatedEvent>
    {
        public HandleCartViewModelUpdatedEvent(IChildMediatorFactory mediator) : base(mediator)
        {
        }

        public override InternalEventResponse Handle(CartViewModelUpdatedEvent internalEvent)
        {
            Mediator.Notification(new CartNeedsUpdateUiEvent {UserId = internalEvent.UserId});
            return InternalEventResponse.Succeeded();
        }
    }
}