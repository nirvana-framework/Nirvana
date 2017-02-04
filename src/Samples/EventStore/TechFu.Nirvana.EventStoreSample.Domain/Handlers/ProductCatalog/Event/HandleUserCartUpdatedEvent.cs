using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.Commands;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.InternalEvents;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.UINotifications;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.ProductCatalog.Event
{
    public class HandleUserCartUpdatedEvent : BaseEventHandler<UserCartUpdatedEvent>
    {
        public HandleUserCartUpdatedEvent(IChildMediatorFactory mediator) : base(mediator)
        {
        }

        public override InternalEventResponse Handle(UserCartUpdatedEvent internalEvent)
        {
            Mediator.Command(new UpdateCartViewModelCommand {UserId = internalEvent.UserId});
            return InternalEventResponse.Succeeded();
        }
    }
    
}