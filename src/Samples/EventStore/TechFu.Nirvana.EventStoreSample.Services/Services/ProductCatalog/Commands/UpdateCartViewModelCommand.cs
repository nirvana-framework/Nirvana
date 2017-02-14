using System;
using Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.Commands
{
    public class UpdateCartViewModelCommand : NopCommand
    {
        public Guid UserId { get; set; }
    }
}