using System.Threading.Tasks;

namespace TechFu.Nirvana.CQRS.UiNotifications
{
    public interface IUiNotificationHub
    {
        Task Publish(ChannelEvent channelEvent);
        Task Subscribe(string channel);
        Task Unsubscribe(string channel);
        Task OnDisconnected(bool stopCalled);
        Task OnConnected();
        Task OnReconnected();
    }
}