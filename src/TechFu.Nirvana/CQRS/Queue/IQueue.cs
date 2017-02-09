using System;
using System.Collections.Generic;
using TechFu.Nirvana.Configuration;

namespace TechFu.Nirvana.CQRS.Queue
{
    public interface IQueue
    {
        NirvanaTaskInformation MessageTypeRouting { get; }
        void Send<TCommandType>(TCommandType message);
        int GetMessageCount();
        void GetAndExecute(int numberOfConsumers);
        void DoWork<TInput>(Func<object, bool> work, bool failOnException = false, bool requeueOnFailure = true);
    }

    public interface IQueue<out TQueueMessageType>: IQueue
    {
        Func<TQueueMessageType> GetMessage { get; }
       
    }
}