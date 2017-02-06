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

namespace TechFu.Nirvana.WebApi.Sample
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(x => WebApiConfig.Register(x, a => { }));
            RouteConfig.RegisterRoutes(RouteTable.Routes, x => { });


            var config = new TestNirvanaConfig();

            NirvanaSetup.Configure()
                .UsingControllerName(config.ControllerAssemblyName, "TechFu.Nirvana.WebApi.Sample")
                .WithAssembliesFromFolder(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"))
                .SetAdditionalAssemblyNameReferences(config.AssemblyNameReferences)
                .SetRootTypeAssembly(GetType().Assembly)
                .SetAttributeMatchingFunction(config.AttributeMatchingFunction)
                .SetDependencyResolver(config.GetService, config.GetAllServices)
                .ForCommands(MediationStrategy.InProcess, MediationStrategy.InProcess, MediationStrategy.InProcess)
                .ForInternalEvents(MediationStrategy.InProcess, MediationStrategy.InProcess, MediationStrategy.InProcess)
                .ForUiNotifications(MediationStrategy.InProcess, MediationStrategy.InProcess,
                    MediationStrategy.InProcess)
                .ForQueries(MediationStrategy.InProcess, MediationStrategy.InProcess)
                .BuildConfiguration()
                ;

            var thirdPartyReferences = new Assembly[0];
            new CqrsApiGenerator().LoadAssembly(thirdPartyReferences);


            var dynamicApiSelector = new DynamicApiSelector(GlobalConfiguration.Configuration,
                config.InlineControllerTypes);
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector), dynamicApiSelector);
        }
    }
}