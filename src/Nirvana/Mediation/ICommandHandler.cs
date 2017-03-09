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
        protected List<ValidationMessage> Messages;
        protected readonly IMediatorFactory Mediator;

        protected BaseCommandHandler(IChildMediatorFactory mediator)
        {
            Messages = new List<ValidationMessage>();
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
                Messages.Count==0?
                CommandResponse.Succeeded(default(U),Messages)
                :CommandResponse.Failed(default(U), Messages.ToArray()) ;
        }

        public abstract CommandResponse<U> Execute(T task);

        public virtual CommandResponse<U> Validate(T task)
        {
            return IsValidated();
        }
    }
}