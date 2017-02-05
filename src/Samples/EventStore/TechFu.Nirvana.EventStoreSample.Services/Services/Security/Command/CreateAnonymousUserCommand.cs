using System;
using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Security.Command
{
    [SecurityRoot("CreateAnonymousUserCommand")]
    public class CreateAnonymousUserCommand:NopCommand
    {
        public Guid SessionId { get; set; }
    }
}