using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using TechFu.Core.Util.DateTimeHelpers;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.CQRS.Queue;
using TechFu.Nirvana.Mediation;
using TechFu.Nirvana.Util.Compression;
using TechFu.Nirvana.Util.Io;

namespace TechFu.Nirvana.AzureQueues.Handlers
{
    public class CreateQueueHandler : ICommandHandler<CreateQueueCommand, QueueCreationResponse>
    {
        public CommandResponse<QueueCreationResponse> Handle(CreateQueueCommand command)
        {
            return CommandResponse.Succeeded(new QueueCreationResponse());
        }
    }

    public class Message<T>
    {
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public Guid? CorrelationId { get; set; }
        public ICollection<Guid> Batches { get; set; } = new List<Guid>();
        public T Body { get; set; }
    }

    public class AzureQueueService : IAzureQueueService

    {
        private readonly Lazy<CloudQueueClient> _client;
        private readonly ICompression _compresion;
        private readonly ISerializerIO _serializer;
        private readonly ISystemTime _systemTime;

        public AzureQueueService(ISerializerIO serializer, ISystemTime systemTime, ICompression compresion)
        {
            _serializer = serializer;
            _systemTime = systemTime;
            _compresion = compresion;
        }


        public AzureQueueService()
        {
            //var connectionString = applicationConfiguration.QueueStorageConnectionString;
            var connectionString = string.Empty;
            _client = new Lazy<CloudQueueClient>(() =>
            {
                
                var account = CloudStorageAccount.Parse(connectionString);

                return account.CreateCloudQueueClient();
            });

            GetQueueName = x => x.Name;
        }

        protected Func<Type, string> GetQueueName { get; set; }

        public void Send<T>(T message)
        {
            Send(message, "");
        }

        private void Send<T>(T message, string user)
        {
            var name = GetQueueName(message.GetType());

            var queue = _client.Value.GetQueueReference(name.ToLower());

            queue.CreateIfNotExists();


            var json = _serializer.Serialize(new Message<T>
            {
                Created = _systemTime.UtcNow(),
                CreatedBy = user,
                Body = message,
                CorrelationId = Guid.NewGuid()
            });

            var cloudQueueMessage = json.Length > 500
                ? new CloudQueueMessage(_compresion.Compress(Encoding.UTF8.GetBytes(json)))
                : new CloudQueueMessage(json);

            queue.AddMessage(cloudQueueMessage);
        }
    }

    public interface IAzureQueueService
    {
    }
}