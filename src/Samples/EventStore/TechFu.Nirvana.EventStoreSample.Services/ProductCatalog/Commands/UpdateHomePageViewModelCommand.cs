using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.Commands
{
    [ProductCatalogRoot("UpdateHomePageViewModelCommand")]
    public class UpdateHomePageViewModelCommand : NopCommand
    {
    }
}