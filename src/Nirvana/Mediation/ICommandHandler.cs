using Nirvana.CQRS;

namespace Nirvana.Mediation
{
    public interface ICommandHandler<T, U>
    {
        CommandResponse<U> Handle(T task);
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

        public abstract CommandResponse<U> Handle(T task);
    }
}