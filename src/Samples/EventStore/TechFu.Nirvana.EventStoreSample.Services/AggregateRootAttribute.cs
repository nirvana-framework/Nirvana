using System;
using TechFu.Nirvana.Domain;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared
{


    public class Infrastructure : RootType{}
    public class ProductCatalog : RootType { }
    public class Users : RootType { }
    public class Security : RootType { }
    public class Lead : RootType { }

    public class InfrastructureRoot : AggregateRootAttribute
    {
        public static string RootName = nameof(Infrastructure);
        public InfrastructureRoot(string identifier) : base(new Infrastructure(), identifier)
        {
        }
    }
    public class ProductCatalogRoot : AggregateRootAttribute
    {
        public static string RootName = nameof(ProductCatalog);
        public ProductCatalogRoot(string identifier) : base(new ProductCatalog(), identifier)
        {
        }
    }
    public class SecurityRoot : AggregateRootAttribute
    {
        public static string RootName = nameof(Security);
        public SecurityRoot(string identifier) : base(new Security(), identifier)
        {
        }
    }
    public class LeadRoot : AggregateRootAttribute
    {
        public static string RootName = nameof(Lead);
        public LeadRoot(string identifier) : base(new Lead(), identifier)
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