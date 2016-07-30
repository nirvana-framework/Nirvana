using System;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.Mediation;
using TechFu.Nirvana.Util.Compression;
using TechFu.Nirvana.Util.Io;
using TechFu.Nirvana.Util.Tine;

namespace TechFu.Nirvana.CQRS.Queue
{
    public abstract class BaseQueue<T> :IQueue<T>
    {


        protected IMediatorFactory Mediator;
        protected ICompression Compression;
        protected ISerializer Serializer;
        protected ISystemTime SystemTime;

        public abstract NirvanaTaskInformation MessageTypeRouting { get; }
        protected static readonly TimeSpan SevenDays = TimeSpan.FromDays(7);
        protected static readonly TimeSpan DefaultVisibilityTimeout = TimeSpan.FromSeconds(30);

        public abstract int GetMessageCount();

        public abstract void Clear();

        public abstract void DoWork<T>(Func< object, bool> work, bool failOnException = false, bool requeueOnFailure = true);

        public abstract void GetAndExecute(int numberOfConsumers);

        public abstract void Send<TCommandType>(TCommandType message);

        public abstract Func<T> GetMessage { get; }

        public BaseQueue<T> SetTime(ISystemTime systemTime)
        {
            SystemTime = systemTime;
            return this;
        }

        public BaseQueue<T> SetSerializer(ISerializer serializer)
        {
            Serializer = serializer;
            return this;
        }

        public BaseQueue<T> SetCompression(ICompression compression)
        {
            Compression = compression;
            return this;
        }
        public BaseQueue<T> SetMediator(IMediatorFactory mediator)
        {
            Mediator= mediator;
            return this;
        }

        public virtual bool HandleMessage(Type commandType,object command)
        {
            return true;
        }
    }
}