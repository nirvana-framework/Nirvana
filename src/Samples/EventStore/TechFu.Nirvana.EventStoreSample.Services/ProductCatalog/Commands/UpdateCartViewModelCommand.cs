using System;
using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.Commands
{
    public class UpdateCartViewModelCommand : NopCommand
    {
        public Guid UserId { get; set; }
    }
}