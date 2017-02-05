using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.Commands
{
    [ProductCatalogRoot("CreateSampleCatalogCommand")]
    public class CreateSampleCatalogCommand : NopCommand
    {
    }
}