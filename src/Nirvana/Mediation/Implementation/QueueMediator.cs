using System;
using Nirvana.Configuration;
using Nirvana.CQRS;
using Nirvana.CQRS.Queue;

namespace Nirvana.Mediation.Implementation
{
    public class QueueMediator:IQueueMediator
    {
        private readonly NirvanaSetup _setup;

        public QueueMediator(NirvanaSetup setup)
        {
            _setup = setup;
        }
        public CommandResponse<TResult> Command<TResult>(Command<TResult> command)
        {
            SendMessage(command);
            return CommandResponse.Succeeded(default(TResult), "Work queued.");
        }

        private void SendMessage(NirvanaTask task)
        {
            var messageType = _setup.FindTypeDefinition(task.GetType());
            var queueFactory = ((IQueueFactory)_setup.GetService(typeof(IQueueFactory)));

            var queue = queueFactory.GetQueue(messageType);
            queue.Send(task);
        }

        public QueryResponse<TResult> Query<TResult>(Query<TResult> query)
        {
            throw new NotImplementedException("This is purposly not implemented - queries should never be put on queue");
        }

        public UIEventResponse UiNotification<T>(UiNotification<T> uiEevent)
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