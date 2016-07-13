using System;
using System.Collections.Generic;

namespace TechFu.Nirvana.CQRS.Queue
{
    public interface IQueue
    {
        TimeSpan VisibilityTimeout { get; set; }
        void Send<T>(T message);
        Type MessageType { get; }
        int GetMessageCount();
        void Clear();
        void DoWork<T>(Func<T,bool> work, bool failOnException=false,bool requeueOnFailure=true);
    }
}