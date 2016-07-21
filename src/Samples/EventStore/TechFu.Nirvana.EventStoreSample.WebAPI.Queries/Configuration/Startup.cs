using System;
using System.IO;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Routing;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.EventStoreSample.Infrastructure.IoC;
using TechFu.Nirvana.EventStoreSample.WebAPI.Queries.Configuration;
using TechFu.Nirvana.WebApi.Controllers;
using TechFu.Nirvana.WebApi.Generation;
using TechFu.Nirvana.WebApi.Startup;

[assembly: OwinStartup(typeof(Startup))]

namespace TechFu.Nirvana.EventStoreSample.WebAPI.Queries.Configuration
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            GlobalConfiguration.Configure(x => WebApiConfig.Register(x, a => { }));
            RouteConfig.RegisterRoutes(RouteTable.Routes, x => { });

            StructureMapAspNet.Configure(Assembly.GetExecutingAssembly()).ForWebApi();

            var config = new NirvanaQueueEndpointConfiguration();

            NirvanaSetup.Configure()
                .UsingControllerName(config.ControllerAssemblyName, config.RootNamespace)
                .WithAssembliesFromFolder(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"))
                .SetAdditionalAssemblyNameReferences(config.AssemblyNameReferences)
                .SetRootType(config.RootType)
                .SetAggregateAttributeType(config.AggregateAttributeType)
                .SetAttributeMatchingFunction(config.AttributeMatchingFunction)
                .SetDependencyResolver(config.GetService)
                .ForQueries()
                .BuildConfiguration();

            app.UseCors(CorsOptions.AllowAll);

            new CqrsApiGenerator().LoadAssembly();


            var dynamicApiSelector = new DynamicApiSelector(GlobalConfiguration.Configuration, config.InlineControllerTypes);
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector),dynamicApiSelector);
            
        }


        

    }

}