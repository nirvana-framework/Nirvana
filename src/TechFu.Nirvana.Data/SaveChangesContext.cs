using System;

namespace TechFu.Nirvana.Data
{
    public class SaveChangesContext 
    {
        public IDataContext Context { get; }
        public Func<int> SaveChanges { get; }

        public SaveChangesContext(IDataContext context, Func<int> saveChanges)
        {
            Context = context;
            SaveChanges = saveChanges;
        }
    }
}