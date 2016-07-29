﻿using System;
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
            var nirvanaTypeDefinitionses = GetTypes();
            QueueTypesByRoot = nirvanaTypeDefinitionses.ToDictionary(x => x.Key, x => x
                .Value.Select(q => new AzureQueueReference
                {
                    MessageTypeRouting = q,
                    MessageCount = 0,
                    Name = GetQueueName(x.Key, q),
                    NumberOfConsumers = 1,
                    Status = QueueStatus.Stopped,
                    SleepInMSBetweenTasks = 100
                }).Cast<QueueReference>().ToArray());
        }

        public QueueReference[] AllQueues()
        {
            return QueueTypesByRoot.SelectMany(x => x.Value).ToArray();
        }

        public QueueReference GetQueueReferenceFor(NirvanaTypeRoutingDefinition typeRouting)
        {
            return AllQueues().SingleOrDefault(x => x.MessageTypeRouting.TaskType == typeRouting.TaskType);
        }

        public bool StartAll()
        {
            AllQueues().ForEach(x => x.StartQueue((IQueueFactory) NirvanaSetup.GetService(typeof(IQueueFactory))));
            return AllQueues().All(x => x.Status == QueueStatus.Started);
        }

        public bool StopAll()
        {
            AllQueues().ForEach(x => x.StopQueue(x.Queue));
            Console.WriteLine($"All queues shutting down");
            WaitForShutDown();
            var stopAll = AllQueues().All(x => x.Status == QueueStatus.Stopped);

            Console.WriteLine($"All queues stopped");
            return stopAll;
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

        private static IDictionary<string, NirvanaTypeRoutingDefinition[]> GetTypes()
        {
            IList<NirvanaTypeRoutingDefinition> types = new List<NirvanaTypeRoutingDefinition>();
            foreach (var task in NirvanaSetup.TaskConfiguration)
            {
                if (task.Value.ReceiverMediationStrategy == MediationStrategy.ForwardToQueue)
                {
                    types.Add(task.Value);
                }
            }

            
            return types.GroupBy(x=>x.RootName).ToDictionary(x=>x.Key,x=>x.ToArray());
        }

        private void WaitForShutDown(string rootName = null)
        {
            //Do it here...
        }

        public static string GetQueueName(string rootType, NirvanaTypeRoutingDefinition typeRouting)
        {
            return typeRouting.UniqueName;
        }
    }
}