using Microsoft.Owin;
using Owin;
using TechFu.Nirvana.EventStoreSample.WebAPI;

[assembly: OwinStartup(typeof(Startup))]

namespace TechFu.Nirvana.EventStoreSample.WebAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}