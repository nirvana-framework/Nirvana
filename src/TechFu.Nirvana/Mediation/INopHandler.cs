using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.Mediation
{
    public interface INopHandler<T> : ICommandHandler<T, Nop>
        where T : Command<Nop>
    {
    }
}