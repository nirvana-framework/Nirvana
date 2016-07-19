using System.Reflection;
using System.Web.Http;
using Owin;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.EventStoreSample.Infrastructure.IoC;

namespace Notifications.Console
{
    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            StructureMapAspNet.Configure(Assembly.GetExecutingAssembly()).ForWebApi();
            var config = new NirvanaCommandProcessorEndpointConfiguration();



            NirvanaSetup.Configure()
             .SetAdditionalAssemblyNameReferences(config.AssemblyNameReferences)
             .SetRootType(config.RootType)
             .SetAggregateAttributeType(config.AggregateAttributeType)
             .SetAttributeMatchingFunction(config.AttributeMatchingFunction)
             .SetDependencyResolver(config.GetService)
             .ForCommandAndQuery()
             .BuildConfiguration()
             ;
            
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.MapSignalR();
            
            var httpConfig = new HttpConfiguration();

            httpConfig.MapHttpAttributeRoutes();


            httpConfig.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional}
            );


            app.UseWebApi(httpConfig);
        }
    }
}