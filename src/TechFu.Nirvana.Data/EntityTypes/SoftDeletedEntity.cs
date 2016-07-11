using System;

namespace TechFu.Nirvana.Data.EntityTypes
{
    public abstract class SoftDeletedEntity<T> : Entity<T>, ISoftDelete
    {
        public DateTime? Deleted { get; set; }
        public string DeletedBy { get; set; }
    }
}