using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.Commands
{
    [ProductCatalogRoot("UpdateHomePageViewModelCommand")]
    public class UpdateHomePageViewModelCommand : NopCommand
    {
    }
}