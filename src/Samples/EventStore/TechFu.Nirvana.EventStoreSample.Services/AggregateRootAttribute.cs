using System;
using Nirvana.Domain;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared
{


    public class InfrastructureRoot : RootType
    {
        public override string RootName => "Infrastructure";
    }
    public class ProductCatalogRoot : RootType
    {
        public override string RootName => "ProductCatalog";
    }
    public class UsersRoot : RootType
    {
        public override string RootName => "Users";
    }
    public class SecurityRoot : RootType
    {
        public override string RootName => "Security";
    }
    public class LeadRoot : RootType
    {
        public override string RootName => "Lead";
    }

    public class InfrastructureRootAttribute : AggregateRootAttribute
    {
        public InfrastructureRootAttribute(Type identifierType) : base(new InfrastructureRoot(), identifierType)
        {
        }
    }
    public class ProductCatalogRootAttribute : AggregateRootAttribute
    {
        public ProductCatalogRootAttribute(Type identifierType) : base(new ProductCatalogRoot(), identifierType)
        {
        }
    }
    public class UsersRootAttribute : AggregateRootAttribute
    {
        public UsersRootAttribute(Type identifierType) : base(new UsersRoot(), identifierType)
        {
        }
    }
    public class SecurityRootAttribute : AggregateRootAttribute
    {
        public SecurityRootAttribute(Type identifierType) : base(new SecurityRoot(), identifierType)
        {
        }
    }
    public class LeadRootAttribute : AggregateRootAttribute
    {
        public LeadRootAttribute(Type identifierType) : base(new LeadRoot(), identifierType)
        {
        }
    }

}