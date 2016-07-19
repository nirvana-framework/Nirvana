using System.Reflection;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Owin.Hosting;
using TechFu.Nirvana.EventStoreSample.Infrastructure.IoC;
using TechFu.Nirvana.SignalRNotifications;

namespace Notifications.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {

      
            var baseAddress = "http://localhost:9123/";




            // Start OWIN host 
            using (WebApp.Start<Startup>(baseAddress))
            {
                var hubConnection = new HubConnection(baseAddress);
                var eventHubProxy = hubConnection.CreateHubProxy("EventHub");
                eventHubProxy.On<string, UiNotificationHubBase.ChannelEvent>("OnEvent",
                    (channel, ev) => System.Console.WriteLine($"Event received on {channel} channel - {@ev}", channel, ev));
                hubConnection.Start().Wait();
                eventHubProxy.Invoke("Subscribe", Constants.AdminChannel);
                eventHubProxy.Invoke("Subscribe", Constants.TaskChannel);
                System.Console.WriteLine($"Server is running on {baseAddress}");
                System.Console.WriteLine("Press <enter> to stop server");
                System.Console.ReadLine();
            }
        }
    }
}