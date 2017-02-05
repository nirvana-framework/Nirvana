using System;
using System.Data.Entity;
using System.Linq;
using TechFu.Nirvana.Data;
using TechFu.Nirvana.Data.EntityTypes;
using TechFu.Nirvana.Domain;
using TechFu.Nirvana.Util.Extensions;
using TechFu.Nirvana.Util.Tine;

namespace TechFu.Nirvana.SqlProvider.Decorators
{
    public class ModifiedCreatedDecorator : ISaveChangesDecorator
    {
        public int Decorate<T>(SaveChangesContext<T> context) where T: AggregateRootAttribute
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
}