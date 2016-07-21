﻿using System;
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
                .SetAdditionalAssemblyNameReferences(config.AssemblyNameReferences)
                .SetRootType(config.RootType)
                .SetAggregateAttributeType(config.AggregateAttributeType)
                .SetAttributeMatchingFunction(config.AttributeMatchingFunction)
                .SetDependencyResolver(config.GetService)
                .ForNotifications(config.NotificationStrategy)
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




            new CqrsApiGenerator().LoadAssembly();

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