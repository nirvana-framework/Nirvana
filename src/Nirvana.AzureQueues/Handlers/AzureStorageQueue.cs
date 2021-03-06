﻿using System;
using System.Reflection;
using System.Text;
using Microsoft.WindowsAzure.Storage.Queue;
using Nirvana.Configuration;
using Nirvana.CQRS;
using Nirvana.CQRS.Queue;
using Nirvana.CQRS.Util;
using Nirvana.Logging;
using Nirvana.Mediation;
using Nirvana.Util.Extensions;

namespace Nirvana.AzureQueues.Handlers
{
    public class AzureStorageQueue : BaseQueue<CloudQueueMessage>
    {
        private static readonly MethodInfo DoWorkmethodInfo;

        private static readonly MethodInfo InternalEventMethodInfo;
        private static readonly MethodInfo QueryMethodInfo;
        private static readonly MethodInfo CommandMethodInfo;
        private static readonly MethodInfo NotificationInfo;

        private readonly CloudQueueClient _client;
        private readonly CloudQueue _queue;
        private readonly string _queueName;
        public override Func<CloudQueueMessage> GetMessage => () => _queue.GetMessageAsync().Result;


        public override NirvanaTaskInformation MessageTypeRouting { get; }

        public TimeSpan VisibilityTimeout { get; set; }

        static AzureStorageQueue()
        {
            CommandMethodInfo = typeof(MediatorFactory).GetMethod("Command");
            QueryMethodInfo = typeof(MediatorFactory).GetMethod("Query");
            NotificationInfo = typeof(MediatorFactory).GetMethod("Notification");
            InternalEventMethodInfo = typeof(MediatorFactory).GetMethod("InternalEvent");
            DoWorkmethodInfo = typeof(AzureStorageQueue).GetMethod("DoWork");
        }


        public AzureStorageQueue(CloudQueueClient client, string rootType, NirvanaTaskInformation messageTypeRouting, int? timeout = null)
        {
            _queueName = AzureQueueController.GetQueueName(rootType, messageTypeRouting);
            _queue = client.GetQueueReference(_queueName.ToLower());
            MessageTypeRouting = messageTypeRouting;
            VisibilityTimeout = timeout != null
                ? timeout > QueueHelper.SevenDays.TotalMilliseconds
                    ? QueueHelper.SevenDays
                    : TimeSpan.FromMilliseconds(timeout.Value)
                : QueueHelper.DefaultVisibilityTimeout;
            _queue.CreateIfNotExistsAsync().Wait();
            _queue.Metadata.Add("Name",_queueName);
            _client = client;
        }


        public override int GetMessageCount()
        {
            _queue.FetchAttributesAsync().Wait();

            return _queue.ApproximateMessageCount ?? 0;
        }

        public override void Clear()
        {
            _queue.ClearAsync().Wait();
        }

        public override void GetAndExecute(int numberOfConsumers)
        {
            //TODO - handle more than one message at a time...
            //Get DoWork
            var handler = GetDoWorkHandler(MessageTypeRouting);
            Func<object, bool> workMethod = null;
            workMethod = GetWorkMethod(workMethod);
            
            
            handler.Invoke(this, new object[] {workMethod, false, false});
            
        }

        private Func<object, bool> GetWorkMethod(Func<object, bool> workMethod)
        {
            if (this.MessageTypeRouting.TaskType.IsInternalEvent())
            {
                workMethod = InvokeInternalEvent;
            }
            if (this.MessageTypeRouting.TaskType.IsCommand())
            {
                workMethod = InvokeCommand;
            }
            if (this.MessageTypeRouting.TaskType.IsUiNotification())
            {
                workMethod = InvokeUiNotification;
            }
            return workMethod;
        }


        public override void Send<T>(T message)
        {
            var queue = _client.GetQueueReference(_queueName.ToLower());

            queue.CreateIfNotExistsAsync().Wait();


            var json = Serializer.Serialize(new Message<T>
            {
                Created = SystemTime.UtcNow(),
                CreatedBy = "",
                Body = message
            });

//            var cloudQueueMessage = json.Length > 500
//                ? new CloudQueueMessage(Compression.Compress(Encoding.UTF8.GetBytes(json)))
//                : new CloudQueueMessage(json);            
            var cloudQueueMessage =  new CloudQueueMessage(json);

            queue.AddMessageAsync(cloudQueueMessage);
        }


        public override void DoWork<T>(Func<object, bool> work, bool failOnException, bool failOnActionFailure)
        {
            var cloudMessage = GetAzureMessage();

            var message = cloudMessage != null ? new AzureQueueMessage(cloudMessage, MessageTypeRouting,Logger,Compression ) : null;
            if (message == null)
            {
                return;
            }

            var success = false;
            var typed = DeserializeMessage<Message<T>, T>(message.Text);
            try
            {
                
                Logger.Debug($"Started {this._queueName}");
                success = work(typed.Body);
                Logger.Debug($"completed {this._queueName}, success: {success}");
            }
            catch (Exception ex)
            {
                Logger.Debug($"Failure {this._queueName}: {ex.Message}");
                if (failOnException)
                {
                    throw;
                }
            }

            if (success || !failOnActionFailure)
            {
                Logger.Debug($"Deleting {this._queueName}:");
                Delete(cloudMessage);
            }
        }


        public CloudQueueMessage GetAzureMessage()
        {
            return _queue.GetMessageAsync().Result;
        }


        public virtual T DeserializeMessage<T, U>(string input) where T : Message<U>
        {
            var messageContainerType = typeof(Message<>).MakeGenericType(MessageTypeRouting.TaskType);
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
            _queue.DeleteMessageAsync(message);
        }


        private MethodInfo GetDoWorkHandler(NirvanaTaskInformation messageTypeRouting)
        {
            return DoWorkmethodInfo.MakeGenericMethod(messageTypeRouting.TaskType);
        }


        public bool InvokeInternalEvent(object x)
        {
            var result = InternalEventMethodInfo.Invoke(Mediator, new[] {x});
            return ((Response) result).Success();
        }
        public bool InvokeCommand(object x)
        {
            var responseType = CqrsUtils.GetResponseType(MessageTypeRouting.TaskType, typeof(Command<>));
            var method = CommandMethodInfo.MakeGenericMethod(responseType);
            var result = method.Invoke(Mediator, new[] {x});
            return ((Response) result).Success();
        }

        private bool InvokeUiNotification(object x)
        {
            var responseType = CqrsUtils.GetResponseType(MessageTypeRouting.TaskType, typeof(UiNotification<>));
            var method = NotificationInfo.MakeGenericMethod(responseType);
            var result = method.Invoke(Mediator, new[] {x});
            return ((Response) result).Success();
        }


        public bool InvokeQuery(object x)
        {
            var responseType = CqrsUtils.GetResponseType(MessageTypeRouting.TaskType, typeof(Query<>));
            var method = QueryMethodInfo.MakeGenericMethod(responseType);
            var result = method.Invoke(Mediator, new[] {x});
            return ((Response) result).Success();
        }
    }
}