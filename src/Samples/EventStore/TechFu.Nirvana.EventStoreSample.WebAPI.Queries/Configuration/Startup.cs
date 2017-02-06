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
                .SetRootTypeAssembly(typeof(Services.Shared.InfrastructureRoot).Assembly)
                .SetAttributeMatchingFunction(config.AttributeMatchingFunction)
                .SetDependencyResolver(config.GetService, config.GetAllServices)
                .ForQueries(MediationStrategy.InProcess, MediationStrategy.InProcess)
                //Don't create endpoints, but do allow the mediator to fire events / commands when a query is called
                .ForCommands(MediationStrategy.None, MediationStrategy.ForwardToQueue,MediationStrategy.None)
                .ForInternalEvents(MediationStrategy.None, MediationStrategy.ForwardToQueue,MediationStrategy.None)
                .ForUiNotifications(MediationStrategy.None, MediationStrategy.ForwardToQueue,MediationStrategy.None)
                .BuildConfiguration();

            app.UseCors(CorsOptions.AllowAll);

            var thirdPartyReferences = new Assembly[0];
            new CqrsApiGenerator().LoadAssembly(thirdPartyReferences);


            var dynamicApiSelector = new DynamicApiSelector(GlobalConfiguration.Configuration, config.InlineControllerTypes);
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector),dynamicApiSelector);
            
        }


        

    }

}