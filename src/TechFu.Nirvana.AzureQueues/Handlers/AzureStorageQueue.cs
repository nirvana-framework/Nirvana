using System;
using System.Reflection;
using System.Text;
using Microsoft.WindowsAzure.Storage.Queue;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.CQRS.Queue;
using TechFu.Nirvana.CQRS.Util;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.AzureQueues.Handlers
{
    public class AzureStorageQueue : BaseQueue<CloudQueueMessage>
    {
        private static readonly MethodInfo DoWorkmethodInfo;
        private static readonly MethodInfo QueryMethodInfo;
        private static readonly MethodInfo CommandMethodInfo;

        private readonly CloudQueueClient _client;
        private readonly CloudQueue _queue;
        private readonly string _queueName;
        public override Func<CloudQueueMessage> GetMessage => () => _queue.GetMessage();


        public override Type MessageType { get; }

        public TimeSpan VisibilityTimeout { get; set; }

        static AzureStorageQueue()
        {
            CommandMethodInfo = typeof(MediatorFactory).GetMethod("Command");
            QueryMethodInfo = typeof(MediatorFactory).GetMethod("Query");
            DoWorkmethodInfo = typeof(AzureStorageQueue).GetMethod("DoWork");
        }


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
            //TODO - handle more than one message at a time...
            //Get DoWork
            var handler = GetDoWorkHandler(MessageType);

            Func<object, bool> workMethod = InvokeCommand;
            handler.Invoke(this, new object[] {workMethod, false, false});
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


        public override void DoWork<T>(Func<object, bool> work, bool failOnException, bool failOnActionFailure)
        {
            var cloudMessage = GetAzureMessage();

            var message = cloudMessage != null ? new AzureQueueMessage(Compression, cloudMessage, MessageType) : null;
            if (message == null)
            {
                return;
            }

            var success = false;
            var typed = DeserializeMessage<Message<T>, T>(message.Text);
            try
            {
                success = work(typed.Body);
            }
            catch (Exception ex)
            {
                if (failOnException)
                {
                    throw;
                }
            }

            if (success || !failOnActionFailure)
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


        private MethodInfo GetDoWorkHandler(Type messageType)
        {
            return DoWorkmethodInfo.MakeGenericMethod(messageType);
        }


        public bool InvokeCommand(object x)
        {
            var responseType = CqrsUtils.GetResponseType(MessageType, typeof(Command<>));
            var method = CommandMethodInfo.MakeGenericMethod(responseType);
            var result = method.Invoke(Mediator, new[] {x});
            return ((Response) result).Success();
        }

        public bool InvokeQuery(object x)
        {
            var responseType = CqrsUtils.GetResponseType(MessageType, typeof(Query<>));
            var method = QueryMethodInfo.MakeGenericMethod(responseType);
            var result = method.Invoke(Mediator, new[] {x});
            return ((Response) result).Success();
        }
    }
}