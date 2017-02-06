using System;
using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Security.Command
{
    [SecurityRoot(typeof(CreateNewSessionViewModelCommand))]
    public class CreateNewSessionViewModelCommand : NopCommand
    {
        public Guid SessionId { get; set; }
    }
}