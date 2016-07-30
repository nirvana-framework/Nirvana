using System;
using System.Collections.Generic;
using TechFu.Nirvana.Configuration;

namespace TechFu.Nirvana.CQRS.Queue
{
    public interface IQueueController
    {
        bool StartAll();
        bool StopAll();

        bool StartRoot(string rootName);
        bool StopRoot(string rootName);

        bool StartQueue(Type messageType);
        bool StopQueue(Type messageType);

        IDictionary<string, QueueReference[]> ByRoot();
        QueueReference[] ForRootType(string rootType);
        QueueReference[] AllQueues();

        QueueReference GetQueueReferenceFor(NirvanaTaskInformation typeRouting);

    }
}