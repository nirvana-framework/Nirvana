using System;
using System.Collections.Generic;
using TechFu.Nirvana.CQRS.Queue;

namespace TechFu.Nirvana.AzureQueues.Handlers
{
    public class AzureQueueController : IQueueController
    {
        public bool Start()
        {
            throw new NotImplementedException();
        }

        public bool Stop()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, QueueReference[]> ByRoot()
        {
            throw new NotImplementedException();
        }

        public QueueReference[] AllQueues()
        {
            throw new NotImplementedException();
        }
    }
}