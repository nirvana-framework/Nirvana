using System;
using Microsoft.AspNet.SignalR;
using TechFu.Nirvana.WebApi.Controllers;

namespace TechFu.Nirvana.SignalRNotifications
{
    public abstract class ApiControllerWithHub<THub> : CommandQueryApiControllerBase
        where THub : UiNotificationHubBase
    {
        protected readonly Lazy<IHubContext> _hub =
            new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<THub>());

        protected IHubContext Hub => _hub.Value;

        protected string GroupName(string instance)
        {
            return  typeof(THub).Name + ":" + instance;
        }
    }
}