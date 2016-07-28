using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.Commands;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.InternalEvents;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.ProductCatalog.Event
{
    public class HandleCatalogUpdatedEvent: IEventHandler<CatalogUpdatedEvent>
    {
        private readonly IMediatorFactory _mediator;

        public HandleCatalogUpdatedEvent(IMediatorFactory mediator)
        {
            _mediator = mediator;
        }

        public InternalEventResponse Handle(CatalogUpdatedEvent command)
        {
            _mediator.Command(new UpdateHomePageViewModelCommand());
            return InternalEventResponse.Succeeded();
        }
    }
}
