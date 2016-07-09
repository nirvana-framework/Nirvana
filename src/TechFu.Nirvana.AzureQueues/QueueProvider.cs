using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.CQRS.Queue;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.AzureQueues
{
    public class QueueProvider: IQueueProvider
    {
        private readonly IMediator _mediator;

        public QueueProvider(IMediator mediator)
        {
            _mediator = mediator;
        }

        public CommandResponse<QueueCreationResponse> CreateQueue(CreateQueueCommand command)
        {
            return _mediator.Command(command);
        }

        public QueryResponse<QueueViewModel> GetQueue(GetQueueQuery query)
        {
            return _mediator.Query(query);
        }

        public QueryResponse<QueueViewModel[]> GetAllQueues(GetAllQueuesQuery query)
        {
            return _mediator.Query(query);
        }
    }
}