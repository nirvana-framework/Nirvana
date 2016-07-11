using System;

namespace TechFu.Nirvana.Data
{
    public interface ISoftDelete
    {
        DateTime? Deleted { get; set; }
        string DeletedBy { get; set; }
    }
}
