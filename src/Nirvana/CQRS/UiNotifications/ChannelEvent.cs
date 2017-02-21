using System;
using Nirvana.Configuration;
using Nirvana.Util.Io;

namespace Nirvana.CQRS.UiNotifications
{
    public class ChannelEvent
    {
        private object _data;

        public string Name { get; set; }
        public string ChannelName { get; set; }
        public Guid AggregateRoot { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public object GetData()
        {
            return _data;
        }

        public ChannelEvent SetData(object value, ISerializer serializer)
        {
            _data = value;
            Json = serializer.Serialize(_data);
            return this;
        }

        public string Json { get; private set; }

        public ChannelEvent()
        {
            Timestamp = DateTimeOffset.Now;
        }
    }
}