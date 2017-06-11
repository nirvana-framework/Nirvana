using System;
using Nirvana.CQRS.Queue;
using Nirvana.Logging;

namespace Nirvana.AzureQueues.Handlers
{
    public class AzureQueueReference : QueueReference
    {
        private readonly ILogger _logger;
        public override Func<IQueueFactory, IQueue> StartQueue => StartQueueInternal;

        public long NextRun { get; set; }
        public bool IsRunning { get; set; }

        public AzureQueueReference(ILogger logger)
        {
            _logger = logger;
        }

        private IQueue StartQueueInternal(IQueueFactory factory)
        {
            Status = QueueStatus.Started;
            Queue = factory.GetQueue(TaskInformaion);
            return Queue;
        }

        public override void RunQueue(QueueStatus status)
        {
            if (IsRunning)
            {
                return;
            }
            IsRunning = true;
            Status = status;

            if (Status == QueueStatus.Started)
            {
                if (DateTime.Now.Ticks < NextRun)
                {
                    return;
                }

                if (Status == QueueStatus.Started)
                {
                    _logger.DetailedDebug($"Checking message {this.Queue.MessageTypeRouting.UniqueName}");
                    try
                    {
                        
                        DoWork();
                    }
                    catch (Exception e)
                    {
                        
                        _logger.Error($"Error processing queue {e.Message}");
                    }
                    
                    this.NextRun = DateTime.Now.AddMilliseconds(SleepInMSBetweenTasks).Ticks;
                }
                   
            }
            if (Status == QueueStatus.ShuttingDown)
            {
                _logger.Debug($"{this.Name} stopped");
                Status = QueueStatus.Stopped;
            }
            IsRunning = false;
        }






        private void DoWork()
        {
            
            //TODO - handle multiple items here...})
                try
                {
                    InternalQueueController.GetAndExecute(Queue, NumberOfConsumers);

                }
                catch (Exception ex)
                {
                    _logger.Debug(ex.Message);
                    throw;
                }
            



        }
    }

    internal class InternalQueueController
    {
        public static void GetAndExecute(IQueue queue, int numberOfConsumers)
        {
            queue.GetAndExecute(numberOfConsumers);
        }
    }
}