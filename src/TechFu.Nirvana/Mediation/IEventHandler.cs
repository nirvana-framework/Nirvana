using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.Mediation
{
    public interface IEventHandler<in T>
        where T : InternalEvent
    {
        InternalEventResponse Handle(T command);
    }
}