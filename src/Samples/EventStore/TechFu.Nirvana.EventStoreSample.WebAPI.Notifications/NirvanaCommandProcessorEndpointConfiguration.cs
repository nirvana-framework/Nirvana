using System;
using TechFu.Nirvana.EventStoreSample.Infrastructure.IoC;
using TechFu.Nirvana.EventStoreSample.Services.Shared;

namespace TechFu.Nirvana.EventStoreSample.WebAPI.Notifications
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
        


        public object GetService(Type serviceType) => InternalDependencyResolver.GetInstance(serviceType);
    }
}