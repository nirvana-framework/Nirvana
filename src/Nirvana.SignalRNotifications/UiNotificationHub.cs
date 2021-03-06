﻿using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Nirvana.CQRS.UiNotifications;
using Nirvana.Util.Io;

namespace Nirvana.SignalRNotifications
{
    public abstract class UiNotificationHub : Hub, IUiNotificationHub
    {
        protected ISerializer Serializer;

        protected UiNotificationHub(ISerializer serializer)
        {
            Serializer = serializer;
        }

        public Task Publish(ChannelEvent channelEvent)
        {
            Clients.Group(channelEvent.ChannelName).OnEvent(channelEvent.ChannelName, channelEvent);

            if (channelEvent.ChannelName == Constants.AdminChannel)
                Clients.Group(Constants.AdminChannel).OnEvent(Constants.AdminChannel, channelEvent);
            if (channelEvent.ChannelName == Constants.AggregateLevel)
                Clients.Group(Constants.AggregateLevel).OnEvent(Constants.AggregateLevel, channelEvent);

            return Task.FromResult(0);
        }

        public async Task Subscribe(string channel)
        {
            await Groups.Add(Context.ConnectionId, channel);

            var ev = new ChannelEvent
            {
                ChannelName = Constants.AdminChannel,
                Name = "user.subscribed"
            }.SetData(new
                {
                    Context.ConnectionId,
                    ChannelName = channel
                }
                , Serializer);

            await Publish(ev);
        }

        public async Task Unsubscribe(string channel)
        {
            await Groups.Remove(Context.ConnectionId, channel);

            var ev = new ChannelEvent
            {
                ChannelName = Constants.AdminChannel,
                Name = "user.unsubscribed"
            }.SetData(new
                {
                    Context.ConnectionId,
                    ChannelName = channel
                },
                Serializer);

            await Publish(ev);
        }
    }
}