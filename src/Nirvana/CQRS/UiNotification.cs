using System;

namespace Nirvana.CQRS
{

    public abstract class UiNotification<T> : UiNotification{}

    public abstract class UiNotification : NirvanaTask
    {
        public abstract Guid AggregateRoot { get; }
    }
    
}