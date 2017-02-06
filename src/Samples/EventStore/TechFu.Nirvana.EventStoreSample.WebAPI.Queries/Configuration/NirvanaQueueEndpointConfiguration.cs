using System;
using System.Linq;
using TechFu.Nirvana.Domain;
using TechFu.Nirvana.EventStoreSample.Infrastructure.IoC;
using TechFu.Nirvana.EventStoreSample.Services.Shared;
using TechFu.Nirvana.WebApi.Controllers;

namespace TechFu.Nirvana.EventStoreSample.WebAPI.Queries.Configuration
{
    public class NirvanaQueueEndpointConfiguration
    {

        public string RootNamespace => "TechFu.Nirvana.EventStoreSample.WebAPI.Commands";
        public string ControllerAssemblyName => "TechFu.Nirvana.EventStoreSample.WebAPI.Commands.dll";

        public  Type RootType => typeof(RootType);
        public  Type AggregateAttributeType => typeof(AggregateRootAttribute);

        public  Func<string, object, bool> AttributeMatchingFunction
            => (x, y) => x == ((AggregateRootAttribute) y).RootType.ToString();

        public  string[] AssemblyNameReferences => new[]
        {
            "TechFu.Nirvana.EventStoreSample.Domain.dll",
            "TechFu.Nirvana.EventStoreSample.Infrastructure.dll",
            "TechFu.Nirvana.EventStoreSample.Services.Shared.dll",
            "TechFu.Nirvana.EventStoreSample.WebAPI.Commands.dll"
        };

        public Type[] InlineControllerTypes=> new[] { typeof(ApiUpdatesController) };


        public object GetService(Type serviceType) => InternalDependencyResolver.GetInstance(serviceType);
        public object[] GetAllServices(Type serviceType) => InternalDependencyResolver.GetAllInstances(serviceType).ToArray();
    }
}