using System;
using TechFu.Nirvana.CQRS.Queue;

namespace TechFu.Nirvana.AzureQueues.Handlers
{
    public class AzureQueueReference : QueueReference
    {
        public override Func<IQueueFactory, IQueue> StartQueue => StartQueueInternal;
        public override Action<IQueue> StopQueue => StopQueueInternal;

        private IQueue StartQueueInternal(IQueueFactory factory)
        {
            Status = QueueStatus.Started;
            Queue = factory.GetQueue(MessageType);
            return Queue;
        }


        private void StopQueueInternal(IQueue queue)
        {
            this.Status=QueueStatus.ShuttingDown;
        }
    }
}