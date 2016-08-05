using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.InternalEvents;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.UINotifications;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.ProductCatalog.Event
{
    public class HandleCartViewModelUpdatedEvent : BaseEventHandler<CartViewModelUpdatedEvent>
    {
        public HandleCartViewModelUpdatedEvent(IChildMediatorFactory mediator) : base(mediator)
        {
        }

        public override InternalEventResponse Handle(CartViewModelUpdatedEvent command)
        {
            Mediator.Notification(new CartNeedsUpdateUiEvent {UserId = command.UserId});
            return InternalEventResponse.Succeeded();
        }
    }
}