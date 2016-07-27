using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Commands.ProductCatalog
{
    [ProductCatalogRoot("CreateSampleCatalogCommand")]
    public class CreateSampleCatalogCommand : NopCommand
    {
    }
}