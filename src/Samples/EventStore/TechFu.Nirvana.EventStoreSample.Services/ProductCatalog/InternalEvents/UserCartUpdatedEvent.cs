using System;
using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.InternalEvents
{
    public class UserCartUpdatedEvent : InternalEvent
    {
        public Guid UserId { get; set; }
    }
}