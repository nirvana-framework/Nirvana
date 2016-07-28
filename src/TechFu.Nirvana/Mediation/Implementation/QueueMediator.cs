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
            SendMessage(command);
            return CommandResponse.Succeeded(default(TResult), "Work queued.");
        }

        private static void SendMessage(NirvanaTask task)
        {
            var messageType = NirvanaSetup.FindTypeDefinition(task.GetType());
            var queueFactory = ((IQueueFactory) NirvanaSetup.GetService(typeof(IQueueFactory)));

            var queue = queueFactory.GetQueue(messageType);
            queue.Send(task);
        }

        public QueryResponse<TResult> Query<TResult>(Query<TResult> query)
        {
            throw new NotImplementedException("This is purposly not implemented - queries should never be put on queue");
        }

        public UIEventResponse UiNotification<T>(UiEvent<T> uiEevent)
        {
            SendMessage(uiEevent);
            return UIEventResponse.Succeeded( "Work queued.");
        }

        public InternalEventResponse InternalEvent(InternalEvent internalEvent)
        {
            SendMessage(internalEvent);
            return InternalEventResponse.Succeeded("Work queued.");
        }

    }
}