using System;
using System.Collections.Generic;
using Nirvana.Configuration;

namespace Nirvana.CQRS.Queue
{
    public interface IQueueController
    {
        QueueStatus Status { get; set; }

        bool InitializeAll();
        //bool StopAll();

//        bool StartRoot(string rootName);
//        bool StopRoot(string rootName);
//
//        bool StartQueue(Type messageType);
//        bool StopQueue(Type messageType);

        IDictionary<string, QueueReference[]> ByRoot();
        QueueReference[] ForRootType(string rootType);
        QueueReference[] AllQueues();

        QueueReference GetQueueReferenceFor(NirvanaTaskInformation typeRouting);

        bool Process();
    }
}