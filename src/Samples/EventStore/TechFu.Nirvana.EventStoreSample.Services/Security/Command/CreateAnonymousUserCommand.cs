using System;
using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Security.Command
{
    public class CreateAnonymousUserCommand:NopCommand
    {
        public Guid SessionId { get; set; }
    }
}