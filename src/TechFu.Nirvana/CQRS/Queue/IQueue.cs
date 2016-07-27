using System;
using System.Collections.Generic;
using TechFu.Nirvana.Configuration;

namespace TechFu.Nirvana.CQRS.Queue
{
    public interface IQueue
    {
        NirvanaTypeDefinition MessageType { get; }
        void Send<TCommandType>(TCommandType message);
        int GetMessageCount();
        void DoWork<T>(Func<object, bool> work, bool failOnException = false, bool requeueOnFailure = true);
        void GetAndExecute(int numberOfConsumers);
    }

    public interface IQueue<out TQueueMessageType>: IQueue
    {
        Func<TQueueMessageType> GetMessage { get; }
       
    }
}