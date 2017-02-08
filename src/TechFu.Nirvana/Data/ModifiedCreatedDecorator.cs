using System;
using TechFu.Nirvana.Data.EntityTypes;
using TechFu.Nirvana.Util.Extensions;
using TechFu.Nirvana.Util.Tine;

namespace TechFu.Nirvana.Data
{
    public class ModifiedCreatedDecorator : ISaveChangesDecorator
    {
        public int Decorate(SaveChangesContext context) 
        {
            var dateTime = new SystemTime().UtcNow();

            var currentUserName = "unknown";

            context.Context.GetEntities(EntityChangeState.Added)
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

            context.Context.GetEntities(EntityChangeState.Modified)
                .ForEach(x =>
                {
                    x.Updated = dateTime;
                    x.UpdatedBy = currentUserName;
                });

            return context.SaveChanges();
        }
    }
}