using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage.Queue;
using TechFu.Core.Util.DateTimeHelpers;
using TechFu.Nirvana.CQRS.Queue;
using TechFu.Nirvana.Util.Compression;
using TechFu.Nirvana.Util.Io;

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

        public Type MessageType { get; set; }
        public TimeSpan VisibilityTimeout { get; set; }


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


        public IQueueMessage GetMessage()
        {
            var message = _queue.GetMessage(VisibilityTimeout);

            return message != null ? new AzureQueueMessage(_compression, message, MessageType) : null;
        }

        public int GetMessageCount()
        {
            _queue.FetchAttributes();

            return _queue.ApproximateMessageCount ?? 0;
        }

        IList<IQueueMessage> IQueue.PeekAtMessages()
        {
            throw new NotImplementedException();
        }

//        public void Delete<T>(T message)
//        {
//            _queue.Clear();
//        }

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

        public bool DeleteMessage(string messageId)
        {
            var deletedMessage = false;

            GetAllMessages()
                .TakeWhile(x =>
                {
                    if (x.Id == messageId)
                    {
                        _queue.Delete(x);
                        deletedMessage = true;
                        return false;
                    }

                    return true;
                })
                .ToList()
                .ForEach(ReEnqueue);

            return deletedMessage;
        }

        public void DoWork<T>(Func<T, bool> work, bool failOnException, bool failOnActionFailure)
        {
            var message = GetMessage();


            bool result=false;
            var typed= DeserializeMessage<Message<T>, T>(message.Text);
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
                Delete(message);
            }

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

        public IEnumerable<IQueueMessage> PeekAtMessages()
        {
            return _queue.PeekMessages(32).Select(x => new AzureQueueMessage(x, MessageType));
        }

        private IEnumerable<IQueueMessage> GetAllMessages()
        {
            IQueueMessage message;

            do
            {
                message = GetMessage();

                if (message != null)
                    yield return message;
            } while (message != null);
        }
    }
}