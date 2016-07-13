using Microsoft.Owin;
using Owin;
using TechFu.Nirvana.EventStoreSample.WebAPI.Commands;

[assembly: OwinStartup(typeof(Startup))]

namespace TechFu.Nirvana.EventStoreSample.WebAPI.Commands
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}