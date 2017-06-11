using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nirvana.Configuration;
using Nirvana.CQRS.Queue;
using Nirvana.Logging;
using Nirvana.Util.Extensions;

namespace Nirvana.AzureQueues.Handlers
{
    public class AzureQueueController : IQueueController
    {
        public static bool ShowDebug = true;
        public QueueStatus Status { get; set; }

        private readonly NirvanaSetup _setup;
        private readonly ILogger _logger;
        public Dictionary<string, QueueReference[]> QueueTypesByRoot { get; set; }
        public int LoopTimeInMS { get; set; }

        public AzureQueueController(NirvanaSetup setup,ILogger logger)
        {
            LoopTimeInMS = 200;
            _setup = setup;
            _logger = logger;
            _logger.Debug("Configuring Queues - to disable debugging, set AzureQueueController.ShowDebug to false");
            var nirvanaTypeDefinitionses = GetTypes();
            QueueTypesByRoot = nirvanaTypeDefinitionses.ToDictionary(x => x.Key, x => x
                .Value.Select(q => new AzureQueueReference(logger)
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


        public bool Process()
        {
            var queueReferences = AllQueues();
            var running = false;
            

            var tasks = queueReferences.Select(x =>
            {
                if (x.Status == QueueStatus.Started)
                {
                    running = true;
                }
                return Task.Run(() =>
                {
                    x.RunQueue(Status);
                });
                
            }).ToArray();

            Task.WaitAll(tasks.ToArray());
            Thread.Sleep(LoopTimeInMS);

            
            return running;
        }

        public bool InitializeAll()
        {
            Status = QueueStatus.Started;
            var queueReferences = AllQueues();
            _logger.Debug($"Starting {queueReferences.Length} Queues");
            queueReferences.ForEach(x =>
            {
                
                _logger.Debug($"Starting {x.Name}");
                x.StartQueue((IQueueFactory) _setup.GetService(typeof(IQueueFactory)));
            });
            var success = queueReferences.All(x => x.Status == QueueStatus.Started);
            _logger.Debug($"Starting successful:{success}");

            return success;
        }

       

//        public bool StopAll()
//        {
//            this.st
//            AllQueues().ForEach(x => x.StopQueue(x.Queue));
//            _logger.Debug($"All queues shutting down");
//            WaitForShutDown();
//            var stopAll = AllQueues().All(x => x.Status == QueueStatus.Stopped);
//
//            _logger.Debug($"All queues stopped");
//            return stopAll;
//        }

       

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