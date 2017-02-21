using System.Reflection;
using Nirvana.Configuration;
using Nirvana.CQRS;

namespace Nirvana.Mediation
{
    public interface IEventHandler<in T>
        where T : InternalEvent
    {
        InternalEventResponse Handle(T internalEvent);
    }

    public abstract class BaseEventHandler<T> : IEventHandler<T>

        where T : InternalEvent
    {
        protected readonly IMediatorFactory Mediator;

        protected BaseEventHandler(IChildMediatorFactory mediator)
        {
            Mediator = mediator;
        }

        public abstract InternalEventResponse Handle(T internalEvent);
    }


    public class InternalEventProcessor
    {
        private readonly NirvanaSetup _setup;

        public InternalEventProcessor(NirvanaSetup setup)
        {
            _setup = setup;
        }

        public InternalEventResponse Process(InternalEvent @event)
        {
            var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
            var handlermethod= handlerType.GetMethod("Handle");

            InternalEventResponse result=null;
            foreach (var service in _setup.GetServices(handlerType))
            {
                var newresult = InvokeEvent(@event, handlermethod, service);
                if (result == null || !newresult.Success())
                {
                    result = newresult;
                }
            }
            return result;
        }

        private static InternalEventResponse InvokeEvent(InternalEvent @event, MethodInfo handlermethod, object service)
        {
            var newresult = handlermethod.Invoke(service, new object[] {@event}) as InternalEventResponse;
            return newresult;
        }
    }
}