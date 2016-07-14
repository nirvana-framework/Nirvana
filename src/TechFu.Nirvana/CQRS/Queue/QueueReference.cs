using System;

namespace TechFu.Nirvana.CQRS.Queue
{
    public class QueueReference
    {
        public QueueStatus Status { get; set; }
        public string Name { get; set; }
        public Type MessageType{ get; set; }
        public int  MessageCount{ get; set; }
        public int  NumberOfConsumers{ get; set; }
    }
}