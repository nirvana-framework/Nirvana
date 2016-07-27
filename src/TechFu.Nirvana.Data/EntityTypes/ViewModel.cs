using System;

namespace TechFu.Nirvana.Data.EntityTypes
{
    public abstract class ViewModel<T> : ViewModel
    {
        public Guid RootEntityKey { get; set; }
        public T Id { get; set; }

        protected bool Equals(Entity<T> other)
        {
            return Id.Equals(other.Id);
        }
    }

    public abstract class ViewModel
    {
    }
}