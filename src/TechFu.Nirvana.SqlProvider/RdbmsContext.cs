using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using EntityFramework.DynamicFilters;
using TechFu.Nirvana.Data;
using TechFu.Nirvana.Data.EntityTypes;
using TechFu.Nirvana.Domain;

namespace TechFu.Nirvana.SqlProvider
{


    public class RdbmsContext : DbContext, IDataContext
    {
        protected readonly ISaveChangesDecorator[] _saveChangesDecorators;

        protected RdbmsContext(string connectionName) : this(SaveChangesDecoratorType.Live, connectionName)
        {
        }

        protected RdbmsContext(SaveChangesDecoratorType type, string connectionType) : base(connectionType)
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


        public IEnumerable<Entity> GetEntities(EntityChangeState state)
        {

            var efState = GetState(state);
            return this.ChangeTracker.Entries<Entity>()
                .Where(x => x.State == efState)
                .Select(x => x.Entity);
        }

        private EntityState GetState(EntityChangeState state)
        {
            switch (state)
            {
                case EntityChangeState.Unchanged:
                    return EntityState.Unchanged;
                case EntityChangeState.Added:
                    return EntityState.Added;
                case EntityChangeState.Deleted:
                    return EntityState.Deleted;
                case EntityChangeState.Modified:
                    return EntityState.Modified;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }



    public abstract class RdbmsContext<T> : RdbmsContext 
        where T: AggregateRootAttribute
    {


        public ObjectContext ObjectContext => ((IObjectContextAdapter) this).ObjectContext;

        protected RdbmsContext(SaveChangesDecoratorType type) : base(type, nameof(T) + "ConnectionString"){}


        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ConfigureModel(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureModel(DbModelBuilder modelBuilder)
        {
            var entityMethod = typeof(DbModelBuilder).GetMethod("Entity");

            foreach (var type in GetAllEntityTypes())
            {
                entityMethod.MakeGenericMethod(type).Invoke(modelBuilder, new object[] {});
            }
            
            
            modelBuilder.Filter("SoftDelete", (ISoftDelete e) => e.Deleted == null || !e.Deleted.HasValue);

        }

        public abstract IEnumerable<Type> GetAllEntityTypes();

    }


   
}
