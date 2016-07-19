using System.Threading.Tasks;
using TechFu.Nirvana.CQRS.UiNotifications;

namespace TechFu.Nirvana.SignalRNotifications
{
    public class EventHub : UiNotificationHub
    {
        public override Task OnConnected()
        {
            var ev = new ChannelEvent
            {
                ChannelName = Constants.AdminChannel,
                Name = "user.connected",
                Data = new
                {
                    Context.ConnectionId
                }
            };

            Publish(ev);

            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            var ev = new ChannelEvent
            {
                ChannelName = Constants.AdminChannel,
                Name = "user.disconnected",
                Data = new
                {
                    Context.ConnectionId
                }
            };

            Publish(ev);

            return base.OnDisconnected(stopCalled);
        }
    }
}