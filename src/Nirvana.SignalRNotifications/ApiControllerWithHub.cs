using System;
using Microsoft.AspNet.SignalR;
using Nirvana.CQRS;
using Nirvana.CQRS.UiNotifications;
using Nirvana.Mediation;
using Nirvana.Util.Io;
using Nirvana.Web.Controllers;

namespace Nirvana.SignalRNotifications
{
    public abstract class ApiControllerWithHub<THub> : CommandQueryApiControllerBase
        where THub : UiNotificationHub
    {
        public readonly Lazy<IHubContext> _hub =
            new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<THub>());

        private readonly ISerializer _serializer;

        protected ApiControllerWithHub(ISerializer serializer, IMediatorFactory mediator) : base(mediator)
        {
            _serializer = serializer;
        }

        protected IHubContext Hub => _hub.Value;

        protected string GroupName(string instance)
        {
            return  typeof(THub).Name + ":" + instance;
        }

        protected void PublishEvent(string eventName, UiEvent task,string channelName)
        {
            var channelEvent = new ChannelEvent()
            {
                ChannelName = Constants.TaskChannel,
                Name = eventName,
                AggregateRoot = task.AggregateRoot,
                
            };
            channelEvent.SetData(task, _serializer);
            Hub.Clients.Group(channelName).OnEvent(Constants.TaskChannel, channelEvent);
        }

    }
}