using System;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared
{
    public enum RootType
    {
        Infrastructure,
        ProductCatalog,
        Users,
        Security,
        Lead,
    }

    public class InfrastructureRoot : AggregateRootAttribute
    {
        public static string RootName = RootType.Infrastructure.ToString();
        public InfrastructureRoot(string identifier) : base(RootType.Infrastructure,identifier)
        {
        }
    }
    public class ProductCatalogRoot : AggregateRootAttribute
    {
        public static string RootName = RootType.ProductCatalog.ToString();
        public ProductCatalogRoot(string identifier) : base(RootType.ProductCatalog, identifier)
        {
        }
    }
    public class SecurityRoot : AggregateRootAttribute
    {
        public static string RootName = RootType.Security.ToString();
        public SecurityRoot(string identifier) : base(RootType.Security, identifier)
        {
        }
    }
    public class LeadRoot : AggregateRootAttribute
    {
        public static string RootName = RootType.Lead.ToString();
        public LeadRoot(string identifier) : base(RootType.Lead, identifier)
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