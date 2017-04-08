using System;
using System.Collections.Generic;
using System.Linq;
using Nirvana.Configuration;
using Nirvana.CQRS.Queue;
using Nirvana.Util.Extensions;

namespace Nirvana.AzureQueues.Handlers
{
    public class AzureQueueController : IQueueController
    {
        public static bool ShowDebug = true;

        private readonly NirvanaSetup _setup;
        public Dictionary<string, QueueReference[]> QueueTypesByRoot { get; set; }

        public AzureQueueController(NirvanaSetup setup)
        {
            _setup = setup;
            Debug("Configuring Queues - to disable debugging, set AzureQueueController.ShowDebug to false");
            var nirvanaTypeDefinitionses = GetTypes();
            QueueTypesByRoot = nirvanaTypeDefinitionses.ToDictionary(x => x.Key, x => x
                .Value.Select(q => new AzureQueueReference
                {
                    TaskInformaion = q,
                    MessageCount = 0,
                    Name = GetQueueName(x.Key, q),
                    NumberOfConsumers = 1,
                    Status = QueueStatus.Stopped,
                    SleepInMSBetweenTasks = 200
                })
                .Cast<QueueReference>()
                .ToArray());
        }

        public QueueReference[] AllQueues()
        {
            return QueueTypesByRoot.SelectMany(x => x.Value).ToArray();
        }

        public QueueReference GetQueueReferenceFor(NirvanaTaskInformation typeRouting)
        {
            return AllQueues().SingleOrDefault(x => x.TaskInformaion.TaskType == typeRouting.TaskType);
        }

        public bool StartAll()
        {
            var queueReferences = AllQueues();
            Debug($"Starting {queueReferences.Length} Queues");
            queueReferences.ForEach(x => x.StartQueue((IQueueFactory) _setup.GetService(typeof(IQueueFactory))));
            var success = queueReferences.All(x => x.Status == QueueStatus.Started);
            Debug($"Starting successful:{success}");

            return success;
        }

        public static void Debug(string message)
        {
            if (ShowDebug)
            {
                Console.WriteLine(message);
            }
        }

        public bool StopAll()
        {
            AllQueues().ForEach(x => x.StopQueue(x.Queue));
            Debug($"All queues shutting down");
            WaitForShutDown();
            var stopAll = AllQueues().All(x => x.Status == QueueStatus.Stopped);

            Debug($"All queues stopped");
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

        public IDictionary<string, NirvanaTaskInformation[]> GetTypes()
        {
            return
                _setup.TaskConfiguration.Where(x => x.Value.CanHandle)
                    .SelectMany(x => x.Value.Tasks)
                    .GroupBy(x => x.RootName)
                    .ToDictionary(x => x.Key, x => x.ToArray());
        }


        private void WaitForShutDown(string rootName = null)
        {
            //Do it here...
        }

        public static string GetQueueName(string rootType, NirvanaTaskInformation typeRouting)
        {
            return typeRouting.UniqueName;
        }
    }
}