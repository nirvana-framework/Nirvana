using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.Commands;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.InternalEvents;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.ProductCatalog.Event
{
    public class HandleUserCartUpdatedEvent : BaseEventHandler<UserCartUpdatedEvent>
    {
        public HandleUserCartUpdatedEvent(IChildMediatorFactory mediator) : base(mediator)
        {
        }

        public override InternalEventResponse Handle(UserCartUpdatedEvent command)
        {
            Mediator.Command(new UpdateCartViewModelCommand {UserId = command.UserId});
            return InternalEventResponse.Succeeded();
        }
    }
}