using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace TechFu.Nirvana.SignalRNotifications
{
    public abstract class UiNotificationHubBase : Hub
    {
        public async Task Subscribe(string instance)
        {
            await Groups.Add(Context.ConnectionId, GroupName(instance));
        }

        protected string GroupName(string instance)
        {
            return GetType().Name + ":" + instance;
        }
    }
}