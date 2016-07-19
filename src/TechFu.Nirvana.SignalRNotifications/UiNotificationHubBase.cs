using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace TechFu.Nirvana.SignalRNotifications
{
    public abstract class UiNotificationHubBase : Hub
    {
        public static class Constants
        {
            public const string AdminChannel = "admin";
            public const string TaskChannel = "tasks";
        }

        public class ChannelEvent
        {
            private object _data;

            public string Name { get; set; }
            public string ChannelName { get; set; }
            public DateTimeOffset Timestamp { get; set; }

            public object Data
            {
                get { return _data; }
                set
                {
                    _data = value;
                    Json = JsonConvert.SerializeObject(_data);
                }
            }

            public string Json { get; private set; }

            public ChannelEvent()
            {
                Timestamp = DateTimeOffset.Now;
            }
        }
        public Task Publish(ChannelEvent channelEvent)
        {
            Clients.Group(channelEvent.ChannelName).OnEvent(channelEvent.ChannelName, channelEvent);

            if (channelEvent.ChannelName != Constants.AdminChannel)
            {
                // Push this out on the admin channel
                //
                Clients.Group(Constants.AdminChannel).OnEvent(Constants.AdminChannel, channelEvent);
            }

            return Task.FromResult(0);
        }

        public async Task Subscribe(string channel)
        {
            await Groups.Add(Context.ConnectionId, channel);

            var ev = new ChannelEvent
            {
                ChannelName = Constants.AdminChannel,
                Name = "user.subscribed",
                Data = new
                {
                    Context.ConnectionId,
                    ChannelName = channel
                }
            };

            await Publish(ev);
        }

        public async Task Unsubscribe(string channel)
        {
            await Groups.Remove(Context.ConnectionId, channel);

            var ev = new ChannelEvent
            {
                ChannelName = Constants.AdminChannel,
                Name = "user.unsubscribed",
                Data = new
                {
                    Context.ConnectionId,
                    ChannelName = channel
                }
            };

            await Publish(ev);
        }


    }
}