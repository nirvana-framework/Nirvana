using System;
using TechFu.Nirvana.Domain;

namespace TechFu.Nirvana.SqlProvider.Decorators
{
    public class SaveChangesContext<T> where T: AggregateRootAttribute
    {
        public RdbmsContext<T> Context { get; }
        public Func<int> SaveChanges { get; }

        public SaveChangesContext(RdbmsContext<T> context, Func<int> saveChanges)
        {
            Context = context;
            SaveChanges = saveChanges;
        }
    }
}