using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.Mediation
{
    public interface ICommandHandler<T, U>
        where T : Command<U>
    {
        CommandResponse<U> Handle(T command);
    }
}