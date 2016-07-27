using System;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared
{
    public enum RootType
    {
        Infrastructure,
        ProductCatalog,
        Users
    }

    public class InfrastructureRoot : AggregateRootAttribute
    {
        public InfrastructureRoot(string identifier) : base(RootType.Infrastructure,identifier)
        {
        }
    }
    public class ProductCatalogRoot : AggregateRootAttribute
    {
        public ProductCatalogRoot(string identifier) : base(RootType.ProductCatalog, identifier)
        {
        }
    }

    public class AggregateRootAttribute : Attribute
    {
        public AggregateRootAttribute(RootType rootType, string identifier)
        {
            RootType = rootType;
            Identifier = identifier;
        }

        public RootType RootType { get; private set; }
        public string Identifier{ get; private set; }
    }
}