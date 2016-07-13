using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.EventStoreSample.Infrastructure.IoC;
using TechFu.Nirvana.EventStoreSample.WebAPI.Commands.Controllers;
using TechFu.Nirvana.WebApi;

namespace TechFu.Nirvana.EventStoreSample.WebAPI.Commands
{
    public class WebApiApplication : HttpApplication
    {

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);



            StructureMapAspNet.Configure(Assembly.GetExecutingAssembly()).ForWebApi();
            var config = new TestNirvanaConfig();

            NirvanaSetup.Configure()
                .SetAdditionalAssemblyNameReferences(config.AssemblyNameReferences)
                .SetRootType(config.RootType)
                .SetAggregateAttributeType(config.AggregateAttributeType)
                .SetAttributeMatchingFunction(config.AttributeMatchingFunction)
                .SetDependencyResolver(config.GetService)
                .ForCommands()
                .ToQueues(QueueStrategy.AllCommands)
                ;

            new CqrsApiGenerator().Configure().LoadAssembly(config.ControllerAssemblyName,
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"), config.RootNamespace);

            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector),
                new DynamicApiSelector(GlobalConfiguration.Configuration, new[] {typeof(ApiUpdatesController)},
                    config.ControllerAssemblyName, Assembly.GetExecutingAssembly().GetName().Name));
        }
    }
}