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

        public object Data
        {
            get { return _data; }
            set
            {
                _data = value;
                Json = ((ISerializer)NirvanaSetup.GetService(typeof(ISerializer))).Serialize(_data);
            }
        }

        public string Json { get; private set; }

        public ChannelEvent()
        {
            Timestamp = DateTimeOffset.Now;
        }
    }
}