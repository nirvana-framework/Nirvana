using System.Threading.Tasks;
using Nirvana.CQRS.UiNotifications;
using Nirvana.Util.Io;

namespace Nirvana.SignalRNotifications
{
    public class EventHub : UiNotificationHub
    {
        public EventHub(ISerializer serializer) : base(serializer)
        {
        }

        public override Task OnConnected()
        {
            var ev = new ChannelEvent
            {
                ChannelName = Constants.AdminChannel,
                Name = "user.connected"
            }.SetData(new
            {
                Context.ConnectionId
            }, Serializer);
            Publish(ev);

            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            var ev = new ChannelEvent
            {
                ChannelName = Constants.AdminChannel,
                Name = "user.disconnected"
            }.SetData(new
                {
                    Context.ConnectionId
                }
                , Serializer);

            Publish(ev);

            return base.OnDisconnected(stopCalled);
        }
    }
}