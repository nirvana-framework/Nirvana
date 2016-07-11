using System;

namespace TechFu.Nirvana.CQRS.Queue
{
    public interface IQueueFactory
    {
        IQueue GetQueue(Type messageType);
    }
}