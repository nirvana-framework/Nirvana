﻿using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Nirvana.Configuration;
using Nirvana.CQRS;
using Nirvana.CQRS.Queue;
using Nirvana.Logging;
using Nirvana.Mediation;
using Nirvana.Util.Compression;
using Nirvana.Util.Io;
using Nirvana.Util.Tine;

namespace Nirvana.AzureQueues.Handlers
{
    public class AzureQueueFactory : IQueueFactory
    {
        private readonly Lazy<CloudQueueClient> _client;
        private readonly ICompression _compression;
        private readonly IMediatorFactory _mediator;
        private readonly IQueueController _queueController;
        private readonly ISerializer _serializer;
        private readonly ISystemTime _systemTime;
        private readonly ILogger _logger;

        protected Func<Type, string> GetQueueName { get; set; }

        public AzureQueueFactory(IAzureQueueConfiguration configuration, IQueueController queueController,
            ISerializer serializer, ISystemTime systemTime, ICompression compression, IMediatorFactory mediator, ILogger logger)
        {
            _queueController = queueController;
            _serializer = serializer;
            _systemTime = systemTime;
            _compression = compression;
            _mediator = mediator;
            _logger = logger;
            _client = new Lazy<CloudQueueClient>(() =>
            {
                var account = CloudStorageAccount.Parse(configuration.ConnectionString);
                return account.CreateCloudQueueClient();
            });
           
        }

        public virtual IQueue GetQueue(NirvanaTaskInformation messageTypeRouting)
        {
            var queueReference = _queueController.GetQueueReferenceFor(messageTypeRouting);
            return new AzureStorageQueue(_client.Value, queueReference.Name, messageTypeRouting)
                .SetTime(_systemTime)
                .SetSerializer(_serializer)
                .SetCompression(_compression)
                .SetLogger(_logger)
                .SetMediator(_mediator);
        }
    }
}