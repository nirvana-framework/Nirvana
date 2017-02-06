using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.Commands
{
    [ProductCatalogRoot(typeof(CreateSampleCatalogCommand))]
    public class CreateSampleCatalogCommand : NopCommand
    {
    }
}