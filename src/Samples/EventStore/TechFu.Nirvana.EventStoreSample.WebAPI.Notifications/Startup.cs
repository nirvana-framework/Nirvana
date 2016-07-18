using Microsoft.Owin;
using Owin;
using TechFu.Nirvana.EventStoreSample.WebAPI.Notifications;

[assembly: OwinStartup(typeof(Startup))]
namespace TechFu.Nirvana.EventStoreSample.WebAPI.Notifications
{
    public class Startup
    {
        public void ConfigureSignalR(IAppBuilder appBuilder)
        {
            appBuilder.MapSignalR();
        }
    }
}