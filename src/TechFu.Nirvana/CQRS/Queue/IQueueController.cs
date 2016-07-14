using System.Collections.Generic;

namespace TechFu.Nirvana.CQRS.Queue
{
    public interface IQueueController
    {
        bool Start();
        bool Stop();

        Dictionary<string,QueueReference[]> ByRoot();
        QueueReference[] AllQueues();

    }
}