using System.Security.Cryptography.X509Certificates;

namespace TechFu.Nirvana.CQRS.Queue
{
    public interface IQueueProvider
    {
        CommandResponse<QueueCreationResponse> CreateQueue(CreateQueueCommand command);

        QueryResponse<QueueViewModel> GetQueue(GetQueueQuery query);
        QueryResponse<QueueViewModel[]> GetAllQueues(GetAllQueuesQuery query);
    }
}