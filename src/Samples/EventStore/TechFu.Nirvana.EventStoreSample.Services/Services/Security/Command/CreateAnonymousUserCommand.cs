using System;
using Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Security.Command
{
    [SecurityRoot(typeof(CreateAnonymousUserCommand))]
    public class CreateAnonymousUserCommand:NopCommand
    {
        public Guid SessionId { get; set; }
    }
}