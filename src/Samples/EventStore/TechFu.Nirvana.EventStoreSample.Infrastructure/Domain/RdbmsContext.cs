using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using EntityFramework.DynamicFilters;
using TechFu.Nirvana.Data;
using TechFu.Nirvana.Data.EntityTypes;
using TechFu.Nirvana.EventStoreSample.Domain.Domain.LeadProtoType;
using TechFu.Nirvana.EventStoreSample.Domain.Domain.ProductCatalog;
using TechFu.Nirvana.Util.Extensions;
using TechFu.Nirvana.Util.Tine;

namespace TechFu.Nirvana.EventStoreSample.Infrastructure.Domain
{

    public class SaveChangesDecoratorFactory
    {
        public ISaveChangesDecorator[] Build(SaveChangesDecoratorType type)
        {
            switch (type)
            {
                case SaveChangesDecoratorType.Live:
                    return new ISaveChangesDecorator[]
                    {
                        new ModifiedCreatedDecorator()
                    };
                case SaveChangesDecoratorType.IntegrationTest:
                case SaveChangesDecoratorType.Empty:
                    return new ISaveChangesDecorator[0];
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
    public class ModifiedCreatedDecorator : ISaveChangesDecorator
    {
        public int Decorate(SaveChangesContext context)
        {
            var dateTime = new SystemTime().UtcNow();

            var currentUserName = "unknown";

            context.Context.ChangeTracker.Entries<Entity>()
                .Where(x => x.State == EntityState.Added)
                .Select(x => x.Entity)
                .ForEach(x =>
                {
                    x.Created = dateTime;
                    x.Updated = dateTime;
                    x.CreatedBy = currentUserName;
                    x.UpdatedBy = currentUserName;

                    var e = x as ISoftDelete;
                    if (e != null)
                    {
                        e.Deleted = null;
                        e.DeletedBy = null;
                    }
                    var eG = x as Entity<Guid>;
                    if (eG != null && eG.Id == Guid.Empty)
                    {
                        eG.Id = Guid.NewGuid();
                    }

                });

            context.Context.ChangeTracker.Entries<Entity>()
                .Where(x => x.State == EntityState.Modified)
                .Select(x => x.Entity)
                .ForEach(x =>
                {
                    x.Updated = dateTime;
                    x.UpdatedBy = currentUserName;
                });

            return context.SaveChanges();
        }
    }
    public enum SaveChangesDecoratorType
    {
        Live,
        IntegrationTest,
        Empty
    }

    public interface ISaveChangesDecorator
    {
        int Decorate(SaveChangesContext context);
    }


    public class SaveChangesContext
    {
        public RdbmsContext Context { get; }
        public Func<int> SaveChanges { get; }

        public SaveChangesContext(RdbmsContext context, Func<int> saveChanges)
        {
            Context = context;
            SaveChanges = saveChanges;
        }
    }

    public class RdbmsContext : DbContext
    {
        private readonly ISaveChangesDecorator[] _saveChangesDecorators;


        public ObjectContext ObjectContext => ((IObjectContextAdapter) this).ObjectContext;

        public RdbmsContext() : this(SaveChangesDecoratorType.Live, "DataStoreConnectionString")
        {
        }

        public RdbmsContext(SaveChangesDecoratorType type, string connectionType) : base(connectionType)
        {
            _saveChangesDecorators = new SaveChangesDecoratorFactory().Build(type);
        }

        public override int SaveChanges()
        {
            Func<int> saveChanges = () => base.SaveChanges();

            foreach (var decorator in _saveChangesDecorators)
            {
                var newContext = new SaveChangesContext(this, saveChanges);

                var localDecorator = decorator;
                saveChanges = () => localDecorator.Decorate(newContext);
            }

            return saveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ConfigureModel(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void ConfigureModel(DbModelBuilder modelBuilder)
        {
            var entityMethod = typeof(DbModelBuilder).GetMethod("Entity");

            foreach (var type in GetAllEntityTypes())
            {
                entityMethod.MakeGenericMethod(type).Invoke(modelBuilder, new object[] {});
            }
            
            modelBuilder.Entity<Lead>().HasOptional(l => l.Measure).WithRequired(b=>b.Lead);
       

            modelBuilder.Filter("SoftDelete", (ISoftDelete e) => e.Deleted == null || !e.Deleted.HasValue);

        }
        public static IEnumerable<Type> GetAllEntityTypes()
        {
            return typeof(Product).Assembly.GetTypes()
                .Where(x => !x.IsAbstract && typeof(Entity).IsAssignableFrom(x));
        }
    }


   
}
