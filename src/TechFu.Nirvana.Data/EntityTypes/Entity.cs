using System;

namespace TechFu.Nirvana.Data.EntityTypes
{
    public abstract class Entity
    {
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }

    public abstract class Entity<T> : Entity
    {
        public T Id { get; set; }

        protected bool Equals(Entity<T> other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Entity<T>) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}