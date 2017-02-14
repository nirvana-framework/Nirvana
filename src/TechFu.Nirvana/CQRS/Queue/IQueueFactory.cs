using Nirvana.Configuration;

namespace Nirvana.CQRS.Queue
{
    public interface IQueueFactory
    {
        IQueue GetQueue(NirvanaTaskInformation messageTypeRouting);
    }
}