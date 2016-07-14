using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Routing;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.WebApi.Generation;
using TechFu.Nirvana.WebApi.Startup;
using TechFu.Nirvana.WebApi.Controllers;

namespace TechFu.Nirvana.WebApi.Sample
{
    public class WebApiApplication : HttpApplication
    {
        private static readonly string _rootNamespace = "TechFu.Nirvana.WebApi.Sample";

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(x => WebApiConfig.Register(x, a => { }));
            RouteConfig.RegisterRoutes(RouteTable.Routes, x => { });



            var config = new TestNirvanaConfig();

            NirvanaSetup.Configure()
                .SetAdditionalAssemblyNameReferences(config.AssemblyNameReferences)
                .SetRootType(config.RootType)
                .SetAggregateAttributeType(config.AggregateAttributeType)
                .SetAttributeMatchingFunction(config.AttributeMatchingFunction)
                .SetDependencyResolver(config.GetService)
                .ForCommandAndQuery()
                ;

            new CqrsApiGenerator().Configure().LoadAssembly(config.ControllerAssemblyName,
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"), _rootNamespace);

            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector),
                new DynamicApiSelector(GlobalConfiguration.Configuration, new[] {typeof(ApiUpdatesController)},
                    config.ControllerAssemblyName, Assembly.GetExecutingAssembly().GetName().Name));
        }
    }
}