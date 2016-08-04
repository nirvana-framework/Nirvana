using System;

namespace TechFu.Nirvana.CQRS
{

    public abstract class UiEvent<T> : UiEvent{}

    public abstract class UiEvent : NirvanaTask
    {
        public abstract Guid AggregateRoot { get; }
    }
    
}