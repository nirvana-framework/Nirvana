using System;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.CQRS.Queue;

namespace TechFu.Nirvana.Mediation.Implementation
{
    public class QueueMediator:IQueueMediator
    {

        public CommandResponse<TResult> Command<TResult>(Command<TResult> command)
        {
            var messageType = command.GetType();
            var queueFactory = ((IQueueFactory)NirvanaSetup.GetService(typeof(IQueueFactory)));

            var queue = queueFactory.GetQueue(messageType);
            queue.Send(command);
            return CommandResponse.Succeeded(default(TResult), "Work queued.");
        }

        public QueryResponse<TResult> Query<TResult>(Query<TResult> query)
        {
            throw new NotImplementedException("This is purposly not implements - queries should never be put on queue");
        }

        public UIEventResponse UiNotification<T>(UiEvent<T> uiEevent)
        {
            throw new NotImplementedException();
        }

        public InternalEventResponse InternalEvent<T>(InternalEvent<T> internalEvent)
        {
            throw new NotImplementedException();
        }

    }
}