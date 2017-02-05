using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using EntityFramework.DynamicFilters;
using TechFu.Nirvana.Data;
using TechFu.Nirvana.Domain;

namespace TechFu.Nirvana.SqlProvider.Decorators
{
    public abstract class RdbmsContext<T> : DbContext where T: AggregateRootAttribute
    {
        protected readonly ISaveChangesDecorator[] _saveChangesDecorators;


        public ObjectContext ObjectContext => ((IObjectContextAdapter) this).ObjectContext;

        protected RdbmsContext() : this(SaveChangesDecoratorType.Live){}
        protected RdbmsContext(SaveChangesDecoratorType type) : this(type, nameof(T) + "ConnectionString"){}
        protected RdbmsContext(SaveChangesDecoratorType type, string connectionType) : base(connectionType)
        {
            _saveChangesDecorators = new SaveChangesDecoratorFactory().Build(type);
        }

        public override int SaveChanges()
        {
            Func<int> saveChanges = () => base.SaveChanges();

            foreach (var decorator in _saveChangesDecorators)
            {
                var newContext = new SaveChangesContext<T>(this, saveChanges);

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
