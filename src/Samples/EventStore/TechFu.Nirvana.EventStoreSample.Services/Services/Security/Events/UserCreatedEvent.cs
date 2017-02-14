using System;
using Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Security.Events
{
    public class UserCreatedEvent:InternalEvent
    {
        public Guid UserId { get; set; }
    }
}