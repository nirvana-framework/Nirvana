using System;

namespace TechFu.Nirvana.CQRS.Queue
{
    public interface IMessage
    {
        DateTime? Created { get; }
        string CreatedBy { get; }
        object Body { get; }
    }


    public class Message<T> : IMessage
    {
        public Guid? CorrelationId { get; set; }
        public T Body { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }

        object IMessage.Body => Body;
    }
}