using System;
using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Security.Events
{
    public class UserCreatedEvent:InternalEvent
    {
        public Guid UserId { get; set; }
    }
}