using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.CQRS.Queue;
using TechFu.Nirvana.Mediation;
using TechFu.Nirvana.Util.Compression;
using TechFu.Nirvana.Util.Io;
using TechFu.Nirvana.Util.Tine;

namespace TechFu.Nirvana.AzureQueues.Handlers
{
    public class AzureQueueFactory : IQueueFactory
    {
        private readonly Lazy<CloudQueueClient> _client;
        private readonly ICompression _compression;
        private readonly IMediatorFactory _mediator;
        private readonly IQueueController _queueController;
        private readonly ISerializer _serializer;
        private readonly ISystemTime _systemTime;

        protected Func<Type, string> GetQueueName { get; set; }

        public AzureQueueFactory(IAzureQueueConfiguration configuration,IQueueController queueController,ISerializer serializer, ISystemTime systemTime, ICompression compression, IMediatorFactory mediator)
        {
            _queueController = queueController;
            _serializer = serializer;
            _systemTime = systemTime;
            _compression = compression;
            _mediator = mediator;
            _client = new Lazy<CloudQueueClient>(() =>
            {
                var account = CloudStorageAccount.Parse(configuration.ConnectionString);
                return account.CreateCloudQueueClient();
            });
           
        }

        public virtual IQueue GetQueue(NirvanaTypeRoutingDefinition messageTypeRouting)
        {
            var queueReference = _queueController.GetQueueReferenceFor(messageTypeRouting);
            return new AzureStorageQueue(_client.Value, queueReference.Name, messageTypeRouting)
                .SetTime(_systemTime)
                .SetSerializer(_serializer)
                .SetCompression(_compression)
                .SetMediator(_mediator);
        }
    }
}