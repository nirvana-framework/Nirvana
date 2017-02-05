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
        public override string RootName => nameof(Infrastructure);
        public InfrastructureRoot(string identifier) : base(new Infrastructure(), identifier)
        {
        }
    }
    public class ProductCatalogRoot : AggregateRootAttribute
    {
        public override string RootName => nameof(ProductCatalog);
        public ProductCatalogRoot(string identifier) : base(new ProductCatalog(), identifier)
        {
        }
    }
    public class SecurityRoot : AggregateRootAttribute
    {
        public override string RootName => nameof(Security);
        public SecurityRoot(string identifier) : base(new Security(), identifier)
        {
        }
    }
    public class LeadRoot : AggregateRootAttribute
    {
        public override string RootName => nameof(Lead);
        public LeadRoot(string identifier) : base(new Lead(), identifier)
        {
        }
    }

}