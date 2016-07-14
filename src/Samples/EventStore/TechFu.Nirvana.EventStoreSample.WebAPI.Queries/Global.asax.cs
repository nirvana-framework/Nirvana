using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Routing;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.EventStoreSample.Infrastructure.IoC;
using TechFu.Nirvana.WebApi.Controllers;
using TechFu.Nirvana.WebApi.Generation;
using TechFu.Nirvana.WebApi.Startup;

namespace TechFu.Nirvana.EventStoreSample.WebAPI.Queries
{
    public class WebApiApplication : HttpApplication
    {

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(x=>WebApiConfig.Register(x, a => { }));
            RouteConfig.RegisterRoutes(RouteTable.Routes,x=>{});



            StructureMapAspNet.Configure(Assembly.GetExecutingAssembly()).ForWebApi();
            var config = new NirvanaQueueEndpointConfiguration();

            NirvanaSetup.Configure()
                .SetAdditionalAssemblyNameReferences(config.AssemblyNameReferences)
                .SetRootType(config.RootType)
                .SetAggregateAttributeType(config.AggregateAttributeType)
                .SetAttributeMatchingFunction(config.AttributeMatchingFunction)
                .SetDependencyResolver(config.GetService)
                .ForQueries()
                .BuildConfiguration()
                ;

            new CqrsApiGenerator().LoadAssembly(config.ControllerAssemblyName,
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"), config.RootNamespace);

            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector),
                new DynamicApiSelector(GlobalConfiguration.Configuration, new[] {typeof(ApiUpdatesController)},
                    config.ControllerAssemblyName, Assembly.GetExecutingAssembly().GetName().Name));
        }
    }
}