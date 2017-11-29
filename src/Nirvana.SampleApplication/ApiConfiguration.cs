using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nirvana.Configuration;
using Nirvana.SampleApplication.Services.Services;
using Nirvana.Web.Controllers;

namespace Nirvana.SampleApplication
{
    public class ApiConfiguration
    {
        public string RootNamespace => "Nirvana.Sample.WebApi";
        public string ControllerAssemblyName => "Nirvana.Sample.WebApi.dll";


        public Func<string, object, bool> AttributeMatchingFunction
            => (x, y) => x == ((SampleServiceRootAttribute) y).RootName;

        public string[] AssemblyNameReferences => new[]
        {
            "Nirvana.Sample.Infrastructure.dll"
        };

//        public Type[] InlineControllerTypes => new[]
//        {
//            typeof(ApiUpdatesController),
//        };

//
//        public object GetService(Type serviceType) => InternalDependencyResolver.GetInstance(serviceType);
//
//        public object[] GetAllServices(Type serviceType)
//            => InternalDependencyResolver.GetAllInstances(serviceType).ToArray();
//
//        public void SetNorvanaSetup(NirvanaSetup setup)
//        {
//            InternalDependencyResolver.GetContainer()
//                .Configure(x => x.For<NirvanaSetup>().Singleton().Use(setup));
//        }
    }
}
