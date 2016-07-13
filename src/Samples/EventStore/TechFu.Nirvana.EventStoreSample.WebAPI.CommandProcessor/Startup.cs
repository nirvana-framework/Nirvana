using Microsoft.Owin;
using Owin;
using TechFu.Nirvana.EventStoreSample.WebAPI.CommandProcessor;

[assembly: OwinStartup(typeof(Startup))]

namespace TechFu.Nirvana.EventStoreSample.WebAPI.CommandProcessor
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}