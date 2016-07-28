using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.EventStoreSample.Infrastructure.IoC;
using TechFu.Nirvana.EventStoreSample.WebAPI.Notifications.Configuration;
using TechFu.Nirvana.SignalRNotifications;
using TechFu.Nirvana.WebApi.Generation;
using TechFu.Nirvana.WebApi.Startup;

[assembly: OwinStartup(typeof(Startup))]

namespace TechFu.Nirvana.EventStoreSample.WebAPI.Notifications.Configuration
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            StructureMapAspNet.Configure(Assembly.GetExecutingAssembly()).ForWebApi();
            var config = new NirvanaCommandProcessorEndpointConfiguration();

            NirvanaSetup.Configure()
                .UsingControllerName(config.ControllerAssemblyName,config.RootNamespace)
                .WithAssembliesFromFolder(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"))
                .SetAdditionalAssemblyNameReferences(config.AssemblyNameReferences)
                .SetRootType(config.RootType)
                .SetAggregateAttributeType(config.AggregateAttributeType)
                .SetAttributeMatchingFunction(config.AttributeMatchingFunction)
                .SetDependencyResolver(config.GetService)
                .ForUiNotifications(MediationStrategy.InProcess, ChildMediationStrategy.ForwardToQueue, MediationStrategy.None)
                .BuildConfiguration()
                ;

            app.UseCors(CorsOptions.AllowAll);


            var connectionConfiguration = new HubConfiguration
            {
                EnableDetailedErrors = true,
            };

            app.MapSignalR("/signalr", connectionConfiguration);
            GlobalConfiguration.Configure(x => WebApiConfig.Register(x, a => { }));
            RouteConfig.RegisterRoutes(RouteTable.Routes, x => { });

//            Failed to compile code generation project : 
//CS0432: Alias 'Infrastructure' not found
//CS0103: The name 'Constants' does not exist in the current context
//CS0103 : The name 'HttpStatusCode' does not exist in the current context
//CS0012 : The type 'HttpStatusCode' is defined in an assembly that is not referenced.You must add a reference to assembly 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'.


            var allAssemblies = new List<Assembly>();

            foreach (var file in Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"), "*.dll"))
            {
                allAssemblies.Add(Assembly.LoadFile(file));
            }

           
            new CqrsApiGenerator().LoadAssembly(allAssemblies.ToArray());

            var httpConfig = new HttpConfiguration();

            httpConfig.MapHttpAttributeRoutes();


            httpConfig.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional}
            );

            var dynamicApiSelector = new DynamicApiSelector(GlobalConfiguration.Configuration, new Type[0]);
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector),dynamicApiSelector);
        }
    }
}