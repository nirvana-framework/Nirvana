using System;
using System.Collections.Generic;
using Nirvana.CQRS;

namespace Nirvana.Mediation
{
    public interface ICommandHandler<T, U>
    {
        CommandResponse<U> Handle(T task);
        CommandResponse<U> Validate(T task);
    }

    public abstract class BaseNoOpCommandHandler<T> : BaseCommandHandler<T, Nop>
        where T : Command<Nop>
    {
        protected BaseNoOpCommandHandler(IChildMediatorFactory mediator) : base(mediator)
        {
        }
    }

    public abstract class BaseCommandHandler<T, U> : ICommandHandler<T, U>
        where T : Command<U>
    {
        protected List<ValidationMessage> Messsages;
        protected readonly IMediatorFactory Mediator;

        protected BaseCommandHandler(IChildMediatorFactory mediator)
        {
            Messsages = new List<ValidationMessage>();
            Mediator = mediator;
        }

        public CommandResponse<U> Handle(T task)
        {
          
            var validationResult = Validate(task);
            return validationResult.Success() 
                ? Execute(task) 
                : validationResult;
        }

        public CommandResponse<U> IsValidated()
        {
            return
                Messsages.Count==0?
                CommandResponse.Succeeded(default(U))
                :CommandResponse.Failed(default(U), Messsages.ToArray()) ;
        }

        public abstract CommandResponse<U> Execute(T task);

        public virtual CommandResponse<U> Validate(T task)
        {
            return IsValidated();
        }
    }
}