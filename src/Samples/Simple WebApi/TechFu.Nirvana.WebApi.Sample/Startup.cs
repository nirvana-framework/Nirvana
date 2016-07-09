using Microsoft.Owin;
using Owin;
using TechFu.Nirvana.WebApi.Sample;

[assembly: OwinStartup(typeof(Startup))]

namespace TechFu.Nirvana.WebApi.Sample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}