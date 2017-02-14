using Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.Commands
{
    [ProductCatalogRoot(typeof(UpdateHomePageViewModelCommand))]
    public class UpdateHomePageViewModelCommand : NopCommand
    {
    }
}