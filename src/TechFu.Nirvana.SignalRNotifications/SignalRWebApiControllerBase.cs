using System;
using Microsoft.AspNet.SignalR;
using TechFu.Nirvana.CQRS.UiNotifications;
using TechFu.Nirvana.WebApi.Controllers;

namespace TechFu.Nirvana.SignalRNotifications
{
    public abstract class ApiControllerWithHub<THub> : CommandQueryApiControllerBase
        where THub : UiNotificationHub
    {
        public readonly Lazy<IHubContext> _hub =
            new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<THub>());

        protected IHubContext Hub => _hub.Value;

        protected string GroupName(string instance)
        {
            return  typeof(THub).Name + ":" + instance;
        }

        protected void PublishEvent(string eventName, object task,string channelName)
        {
            Hub.Clients.Group(channelName).OnEvent(Constants.TaskChannel, new ChannelEvent()
            {
                ChannelName = Constants.TaskChannel,
                Name = eventName,
                Data = task
            });
        }

    }
}