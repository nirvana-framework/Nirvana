using System.Collections.Generic;
using TechFu.Nirvana.Data.EntityTypes;

namespace TechFu.Nirvana.Data
{
    public interface IDataContext
    {
        IEnumerable<Entity> GetEntities(EntityChangeState state);
    }

    public enum EntityChangeState
    {
        Unchanged = 1,
        Added = 2,
        Deleted = 4,
        Modified = 8
    }
}