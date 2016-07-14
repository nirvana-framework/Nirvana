using System;
using System.Collections.Generic;
using System.Linq;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.CQRS.Queue;
using TechFu.Nirvana.Util.Extensions;

namespace TechFu.Nirvana.AzureQueues.Handlers
{
    public class AzureQueueController : IQueueController
    {

        public Dictionary<string, QueueReference[]> QueueTypesByRoot { get; set; }

        public AzureQueueController()
        {
            QueueTypesByRoot = NirvanaSetup.CommandTypes.ToDictionary(x => x.Key, x => x
                .Value.Select(q => new AzureQueueReference
                {
                    MessageType = q,
                    MessageCount = 0,
                    Name = GetQueueName(x.Key,q),
                    NumberOfConsumers = 1,
                    Status = QueueStatus.Stopped
                }).Cast<QueueReference>().ToArray());

        }

        public QueueReference[] AllQueues()
        {
            return QueueTypesByRoot.SelectMany(x => x.Value).ToArray();
        }

        public bool StartAll()
        {
            AllQueues().ForEach(x=>x.StartQueue((IQueueFactory)NirvanaSetup.GetService(typeof(IQueueFactory))));
            return AllQueues().All(x => x.Status == QueueStatus.Started);
            
        }

        public bool StopAll()
        {
            AllQueues().ForEach(x => x.StopQueue(x.Queue));
            WaitForShutDown();
            return AllQueues().All(x => x.Status == QueueStatus.Stopped);
        }

        private void WaitForShutDown()
        {
            //Do it here...
        }

        public bool StartRoot(string rootName)
        {
            throw new NotImplementedException();
        }

        public bool StopRoot(string rootName)
        {
            throw new NotImplementedException();
        }

        public bool StartQueue(Type messageType)
        {
            throw new NotImplementedException();
        }

        public bool StopQueue(Type messageType)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, QueueReference[]> ByRoot()
        {
            return QueueTypesByRoot;
        }

        public QueueReference[] ForRootType(string rootType)
        {
            return QueueTypesByRoot[rootType];
        }

        public static string GetQueueName(string rootType, Type type)
        {
            return $"{rootType}_{type}";
        }


      
    }
}