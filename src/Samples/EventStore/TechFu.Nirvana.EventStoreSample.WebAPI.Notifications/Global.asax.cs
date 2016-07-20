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

namespace TechFu.Nirvana.EventStoreSample.WebAPI.Notifications
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
    
         

      

//            new CqrsApiGenerator().LoadAssembly(config.ControllerAssemblyName,
//                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"), config.RootNamespace);
        }
    }
}