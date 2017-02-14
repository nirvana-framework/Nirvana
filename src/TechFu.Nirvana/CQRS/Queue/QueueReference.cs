using System;
using Nirvana.Configuration;

namespace Nirvana.CQRS.Queue
{
    public abstract class QueueReference
    {
        public QueueStatus Status { get; set; }
        public string Name { get; set; }
        public bool CanCancel{ get; set; }
        public NirvanaTaskInformation TaskInformaion{ get; set; }
        public int  MessageCount{ get; set; }
        public int  NumberOfConsumers{ get; set; }
        public int SleepInMSBetweenTasks { get; set; }
        public abstract Func<IQueueFactory,IQueue> StartQueue { get;  }
        public abstract Action<IQueue> StopQueue { get;  }
        public IQueue Queue { get; protected set; }
    }
}