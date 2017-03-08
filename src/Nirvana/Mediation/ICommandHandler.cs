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
        protected readonly IMediatorFactory Mediator;

        protected BaseCommandHandler(IChildMediatorFactory mediator)
        {
            Mediator = mediator;
        }

        public CommandResponse<U> Handle(T task)
        {
            var success = Validate(task);
            return success.Success() ? Execute(task) : success;
        }

        public virtual CommandResponse<U> Validate(T task)
        {
            return CommandResponse.Succeeded(default(U));
        }
        public CommandResponse<U> IsValidated(ValidationMessage[] messages)
        {
            return
                messages.Length==0?
                CommandResponse.Succeeded(default(U))
                :CommandResponse.Failed(default(U),messages) ;
        }

        public abstract CommandResponse<U> Execute(T task);
    }
}