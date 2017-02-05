using System;
using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Security.Events
{
    public class SessionViewModelCreatedEvent : InternalEvent
    {
        public Guid SessionId { get; set; }
    }
}