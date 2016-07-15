using System;
using System.Text;
using Microsoft.WindowsAzure.Storage.Queue;
using TechFu.Nirvana.CQRS.Queue;

namespace TechFu.Nirvana.AzureQueues.Handlers
{
    public class AzureStorageQueue : BaseQueue<CloudQueueMessage>
    {
        private readonly CloudQueueClient _client;
        private readonly CloudQueue _queue;
        private readonly string _queueName;
        public override Func<CloudQueueMessage> GetMessage => () => _queue.GetMessage();


        public override Type MessageType { get; }

        public TimeSpan VisibilityTimeout { get; set; }


        public AzureStorageQueue(CloudQueueClient client, string rootType, Type messageType, int? timeout = null)
        {
            _queueName = AzureQueueController.GetQueueName(rootType, messageType);
            _queue = client.GetQueueReference(_queueName.ToLower());
            MessageType = messageType;
            VisibilityTimeout = timeout != null
                ? timeout > SevenDays.TotalMilliseconds
                    ? SevenDays
                    : TimeSpan.FromMilliseconds(timeout.Value)
                : DefaultVisibilityTimeout;
            _queue.CreateIfNotExists();
            _client = client;
        }


        public override int GetMessageCount()
        {
            _queue.FetchAttributes();

            return _queue.ApproximateMessageCount ?? 0;
        }

        public override void Clear()
        {
            _queue.Clear();
        }

        public override void GetAndExecute(int numberOfConsumers)
        {
            var message = GetMessage();
           var result =  DoWork(x=>HandleMessage(MessageType,x),false,true);
        }

        public override void Send<T>(T message)
        {
            var queue = _client.GetQueueReference(_queueName.ToLower());

            queue.CreateIfNotExists();


            var json = Serializer.Serialize(new Message<T>
            {
                Created = SystemTime.UtcNow(),
                CreatedBy = "",
                Body = message
            });

            var cloudQueueMessage = json.Length > 500
                ? new CloudQueueMessage(Compression.Compress(Encoding.UTF8.GetBytes(json)))
                : new CloudQueueMessage(json);

            queue.AddMessage(cloudQueueMessage);
        }


        public override void DoWork(Func<Type,object, bool> work, bool failOnException, bool failOnActionFailure)
        {
            var cloudMessage = GetAzureMessage();

            var message = cloudMessage != null ? new AzureQueueMessage(Compression, cloudMessage, MessageType) : null;
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
            var message = (T) Serializer.Deserialize(messageContainerType, input);
            message.Created = message.Created;
            message.CreatedBy = message.CreatedBy;
            message.CorrelationId = message.CorrelationId;
            message.Body = message.Body;
            return message;
        }


        public virtual string SerializeMessage<T, U>(T message)
            where T : Message<U>
        {
            return message != null ? Serializer.Serialize(message.Body) : null;
        }

        public void ReEnqueue<T>(T message)
        {
            Send(message);
        }

        public void Delete(CloudQueueMessage message)
        {
            _queue.DeleteMessage(message);
        }
    }
}