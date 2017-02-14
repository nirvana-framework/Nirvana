using Nirvana.CQRS;
using Nirvana.Mediation;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.Commands;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.InternalEvents;

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