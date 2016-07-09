using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.WebApi.Sample.Controllers;

namespace TechFu.Nirvana.WebApi.Sample
{
    public class WebApiApplication : HttpApplication
    {
        private static readonly string _rootNamespace = "TechFu.Nirvana.WebApi.Sample";

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);



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
                new DynamicApiSelector(GlobalConfiguration.Configuration, new[] {typeof(APIUpdatesController)},
                    config.ControllerAssemblyName, Assembly.GetExecutingAssembly().GetName().Name));
        }
    }
}