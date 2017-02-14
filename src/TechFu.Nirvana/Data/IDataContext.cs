using System.Collections.Generic;
using Nirvana.Data.EntityTypes;

namespace Nirvana.Data
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
        Modified = 8,
    }
}