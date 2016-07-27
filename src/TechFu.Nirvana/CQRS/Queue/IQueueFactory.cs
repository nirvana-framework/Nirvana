using System;
using TechFu.Nirvana.Configuration;

namespace TechFu.Nirvana.CQRS.Queue
{
    public interface IQueueFactory
    {
        IQueue GetQueue(NirvanaTypeDefinition messageType);
    }
}