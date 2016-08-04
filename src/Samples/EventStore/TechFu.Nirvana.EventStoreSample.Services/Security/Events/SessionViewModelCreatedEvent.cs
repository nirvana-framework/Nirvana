using System;
using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Security.Events
{
    public class SessionViewModelCreatedEvent : InternalEvent
    {
        public Guid SessionId { get; set; }
    }
}