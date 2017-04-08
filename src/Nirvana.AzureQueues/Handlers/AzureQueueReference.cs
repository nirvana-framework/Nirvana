using System;
using System.Threading;
using System.Threading.Tasks;
using Nirvana.CQRS.Queue;

namespace Nirvana.AzureQueues.Handlers
{
    public class AzureQueueReference : QueueReference
    {
        public override Func<IQueueFactory, IQueue> StartQueue => StartQueueInternal;
        public override Action<IQueue> StopQueue => StopQueueInternal;

        private IQueue StartQueueInternal(IQueueFactory factory)
        {
            Status = QueueStatus.Started;
            Queue = factory.GetQueue(TaskInformaion);


            RunQueue();


            return Queue;
        }

        private void RunQueue()
        {
            if (Status == QueueStatus.Started)
            {
                Task.Run(() =>
                {
                    if (Status == QueueStatus.Started)
                    {
                        var token = new CancellationToken();
                        var work = DoWork(token);
                        work.ContinueWith(x => RunQueue(), token);
                    }
                    if (Status == QueueStatus.ShuttingDown)
                    {
                        AzureQueueController.Debug($"shutting down {Name}");
                        Status = QueueStatus.Stopped;
                    }
                }).ContinueWith(x => x.Wait(this.SleepInMSBetweenTasks));
            }
            if (Status == QueueStatus.ShuttingDown)
            {
                AzureQueueController.Debug($"{this.Name} stopped");
                Status = QueueStatus.Stopped;
            }
        }


        private void StopQueueInternal(IQueue queue)
        {
            AzureQueueController.Debug($"Shutting down {this.Name}");
            this.Status = QueueStatus.ShuttingDown;
        }



        private async Task DoWork(CancellationToken ct)
        {
            await Task.Run(() =>
            {
                try
                {
                    InternalQueueController.GetAndExecute(Queue, NumberOfConsumers);

                }
                catch (Exception ex)
                {
                    AzureQueueController.Debug(ex.Message);
                    throw;
                }
                //TODO - handle multiple items here...})
            }, ct);



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