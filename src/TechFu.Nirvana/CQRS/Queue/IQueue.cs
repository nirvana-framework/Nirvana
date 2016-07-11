using System;
using System.Collections.Generic;

namespace TechFu.Nirvana.CQRS.Queue
{
    public interface IQueue
    {

        void Send<T>(T message);

        T DeserializeMessage<T,U>(string input) where T : Message<U>;
        string SerializeMessage<T, U>(T message) where T : Message<U>;
        void Delete<T>(T message);
        void ReEnqueue<T>(T message);

        Type MessageType { get; }
        TimeSpan VisibilityTimeout { get; }
        IQueueMessage GetMessage();
        int GetMessageCount();
        IList<IQueueMessage> PeekAtMessages();
        void Clear();
        bool DeleteMessage(string messageId);
    }
}