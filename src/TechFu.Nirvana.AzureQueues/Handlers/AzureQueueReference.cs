using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TechFu.Nirvana.CQRS.Queue;

namespace TechFu.Nirvana.AzureQueues.Handlers
{
    public class AzureQueueReference : QueueReference
    {
        public override Func<IQueueFactory, IQueue> StartQueue => StartQueueInternal;
        public override Action<IQueue> StopQueue => StopQueueInternal;

        private IQueue StartQueueInternal(IQueueFactory factory)
        {
            Status = QueueStatus.Started;
            Queue = factory.GetQueue(MessageType);

           
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
                        Status = QueueStatus.Stopped;
                    }
                }).ContinueWith(x=>x.Wait(this.SleepInMSBetweenTasks));
            }
            if (Status == QueueStatus.ShuttingDown)
            {
                Console.WriteLine($"{this.Name} stopped");
                Status = QueueStatus.Stopped;
            }
        }


        private void StopQueueInternal(IQueue queue)
        {
            Console.WriteLine($"Shutting down {this.Name}");
            this.Status=QueueStatus.ShuttingDown;
        }



        private async Task DoWork(CancellationToken ct)
        {
            GetAndExecute(NumberOfConsumers);
        }

        private void GetAndExecute(int numberOfConsumers)
        {
            InternalQueueController.GetAndExecute(this.Queue, numberOfConsumers);
        }
    }

    internal class InternalQueueController
    {
        public static void GetAndExecute(IQueue queue, int numberOfConsumers)
        {
            throw new NotImplementedException();
        }
    }
}