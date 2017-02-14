using System;

namespace Nirvana.Data
{
    public interface ISoftDelete
    {
        DateTime? Deleted { get; set; }
        string DeletedBy { get; set; }
    }
}
