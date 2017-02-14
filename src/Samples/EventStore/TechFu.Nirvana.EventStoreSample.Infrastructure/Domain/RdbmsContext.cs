using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using EntityFramework.DynamicFilters;
using Nirvana.Data;
using TechFu.Nirvana.EventStoreSample.Domain.Domain.LeadPrototype;
using TechFu.Nirvana.EventStoreSample.Domain.Domain.ProductCatalog;
using TechFu.Nirvana.EventStoreSample.Services.Shared;
using TechFu.Nirvana.SqlProvider;

namespace TechFu.Nirvana.EventStoreSample.Infrastructure.Domain
{


    public class InfrastructureContext : RdbmsContext<InfrastructureRoot>
    {
        public InfrastructureContext(SaveChangesDecoratorType type) : base(type)
        {
        }

        public override IEnumerable<Type> GetAllEntityTypes()
        {
            return EntityTypesByNamespace("TechFu.Nirvana.EventStoreSample.Domain.Domain.Infrastructure", GetType().Assembly);
        }
    }

    public class ProductCatalogContext: RdbmsContext<ProductCatalogRoot>
    {
        public ProductCatalogContext(SaveChangesDecoratorType type) : base(type)
        {
        }

        public override IEnumerable<Type> GetAllEntityTypes()
        {
            return EntityTypesByNamespace("TechFu.Nirvana.EventStoreSample.Domain.Domain.ProductCatalog", GetType().Assembly);
        }
    }

    public class UsersContext: RdbmsContext<UsersRoot>
    {
        public UsersContext(SaveChangesDecoratorType type) : base(type)
        {
        }


        public override IEnumerable<Type> GetAllEntityTypes()
        {
            return EntityTypesByNamespace("TechFu.Nirvana.EventStoreSample.Domain.Domain.Users", GetType().Assembly);
        }
    }

    public class SecurityContext: RdbmsContext<SecurityRoot>
    {
        public SecurityContext(SaveChangesDecoratorType type) : base(type)
        {
        }

        public override IEnumerable<Type> GetAllEntityTypes()
        {
            return EntityTypesByNamespace("TechFu.Nirvana.EventStoreSample.Domain.Domain.Security", GetType().Assembly);
        }
    }

    public class LeadContext: RdbmsContext<LeadRoot>
    {
        public LeadContext(SaveChangesDecoratorType type) : base(type)
        {
        }


        public override IEnumerable<Type> GetAllEntityTypes()
        {
            return EntityTypesByNamespace("TechFu.Nirvana.EventStoreSample.Domain.Domain.LeadPrototype", GetType().Assembly);
        }
    }
}
