using System;
using System.Text;
using Microsoft.WindowsAzure.Storage.Queue;
using TechFu.Nirvana.CQRS.Queue;
using TechFu.Nirvana.Util.Compression;
using TechFu.Nirvana.Util.Io;
using TechFu.Nirvana.Util.Tine;

namespace TechFu.Nirvana.AzureQueues.Handlers
{
    public class AzureStorageQueue : IQueue
    {
        private static readonly TimeSpan SevenDays = TimeSpan.FromDays(7);
        private static readonly TimeSpan DefaultVisibilityTimeout = TimeSpan.FromSeconds(30);
        private readonly CloudQueueClient _client;
        private readonly CloudQueue _queue;
        private readonly string _queueName;

        private ICompression _compression;

        private ISerializer _serializer;
        private ISystemTime _systemTime;

        public AzureStorageQueue(CloudQueueClient client, Type messageType, string queueName, int? timeout = null)
        {
            _queue = client.GetQueueReference(queueName.ToLower());

            _queueName = queueName;
            MessageType = messageType;
            VisibilityTimeout = timeout != null
                ? timeout > SevenDays.TotalMilliseconds
                    ? SevenDays
                    : TimeSpan.FromMilliseconds(timeout.Value)
                : DefaultVisibilityTimeout;
            _queue.CreateIfNotExists();
            _client = client;
        }

        public TimeSpan VisibilityTimeout { get; set; }

        public Type MessageType { get; set; }


      

        public int GetMessageCount()
        {
            _queue.FetchAttributes();

            return _queue.ApproximateMessageCount ?? 0;
        }

        public void Clear()
        {
            _queue.Clear();
        }

        public void Send<T>(T message)
        {
            var queue = _client.GetQueueReference(_queueName.ToLower());

            queue.CreateIfNotExists();


            var json = _serializer.Serialize(new Message<T>
            {
                Created = _systemTime.UtcNow(),
                CreatedBy = "",
                Body = message
            });

            var cloudQueueMessage = json.Length > 500
                ? new CloudQueueMessage(_compression.Compress(Encoding.UTF8.GetBytes(json)))
                : new CloudQueueMessage(json);

            queue.AddMessage(cloudQueueMessage);
        }

        public void DoWork<T>(Func<T, bool> work, bool failOnException, bool failOnActionFailure)
        {

            var cloudMessage = GetAzureMessage();

            var message  = cloudMessage != null ? new AzureQueueMessage(_compression, cloudMessage, MessageType) : null;
            if (message == null)
            {
                return;
            }

            var result = false;
            var typed = DeserializeMessage<Message<T>, T>(message.Text);
            try
            {
                result = work(typed.Body);
            }
            catch (Exception ex)
            {
                if (failOnException)
                {
                    throw;
                }
            }

            if (result || !failOnActionFailure)
            {
                Delete(cloudMessage);
            }
        }

        public CloudQueueMessage GetAzureMessage()
        {
            return _queue.GetMessage(VisibilityTimeout);
        }


        public virtual T DeserializeMessage<T, U>(string input) where T : Message<U>
        {
            var messageContainerType = typeof(Message<>).MakeGenericType(MessageType);
            var message = (T) _serializer.Deserialize(messageContainerType, input);
            message.Created = message.Created;
            message.CreatedBy = message.CreatedBy;
            message.CorrelationId = message.CorrelationId;
            message.Body = message.Body;
            return message;
        }


        public virtual string SerializeMessage<T, U>(T message)
            where T : Message<U>
        {
            return message != null ? _serializer.Serialize(message.Body) : null;
        }

        public void ReEnqueue<T>(T message)
        {
            Send(message);
        }

        public void Delete(CloudQueueMessage message)
        {
            _queue.DeleteMessage(message);
        }

       

        public AzureStorageQueue SetTime(ISystemTime systemTime)
        {
            _systemTime = systemTime;
            return this;
        }

        public AzureStorageQueue SetSerializer(ISerializer serializer)
        {
            _serializer = serializer;
            return this;
        }

        public AzureStorageQueue SetCompression(ICompression compression)
        {
            _compression = compression;
            return this;
        }
    }
}