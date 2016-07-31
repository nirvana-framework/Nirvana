using System;
using System.Linq;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.EventStoreSample.Infrastructure.IoC;
using TechFu.Nirvana.EventStoreSample.Services.Shared;

namespace TechFu.Nirvana.EventStoreSample.WebAPI.Notifications.Configuration
{
    public class NirvanaCommandProcessorEndpointConfiguration
    {

        public string RootNamespace => "TechFu.Nirvana.EventStoreSample.WebAPI.Notifications";
        public string ControllerAssemblyName => RootNamespace + ".dll";

        public  Type RootType => typeof(RootType);
        public  Type AggregateAttributeType => typeof(AggregateRootAttribute);

        public  Func<string, object, bool> AttributeMatchingFunction
            => (x, y) => x == ((AggregateRootAttribute) y).RootType.ToString();

        public  string[] AssemblyNameReferences => new[]
        {
            "TechFu.Nirvana.dll",
            "TechFu.Nirvana.WebApi.dll",
            "TechFu.Nirvana.EventStoreSample.Domain.dll",
            "TechFu.Nirvana.EventStoreSample.Infrastructure.dll",
            "TechFu.Nirvana.EventStoreSample.Services.Shared.dll",
            ControllerAssemblyName
        };

        public MediationStrategy NotificationStrategy => MediationStrategy.InProcess;


        public object GetService(Type serviceType) => InternalDependencyResolver.GetInstance(serviceType);
        public object[] GetAllServices(Type serviceType) => InternalDependencyResolver.GetAllInstances(serviceType).ToArray();
    }
}