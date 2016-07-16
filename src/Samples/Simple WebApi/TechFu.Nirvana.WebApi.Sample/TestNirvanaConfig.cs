using System;
using TechFu.Nirvana.Mediation;
using TechFu.Nirvana.Mediation.Implementation;
using TechFu.Nirvana.WebApi.Sample.DomainSpecificData;
using TechFu.Nirvana.WebApi.Sample.DomainSpecificData.Handlers;
using TechFu.Nirvana.WebApi.Sample.DomainSpecificData.Queries;

namespace TechFu.Nirvana.WebApi.Sample
{
    public class TestNirvanaConfig
    {
        public string ControllerAssemblyName => "TechFu.Nirvana.WebApi.Sample.Controllers.dll";

        public  Type RootType => typeof(RootType);
        public  Type AggregateAttributeType => typeof(AggregateRootAttribute);

        public  Func<string, object, bool> AttributeMatchingFunction
            => (x, y) => x == ((AggregateRootAttribute) y).RootType.ToString();

        public  string[] AssemblyNameReferences => new[]
        {
            "TechFu.Nirvana.dll",
            "TechFu.Nirvana.WebApi.dll",
            "TechFu.Nirvana.WebApi.Sample.dll"
        };

        
   

        //Plug your IoC in here
        public  object GetService(Type serviceType)
        {
            if (serviceType == typeof(IMediator))
            {
                return new Mediator();
            }

            if (serviceType == typeof(IQueryHandler<GetVersionQuery,string>))
            {
                return new GetVersionHandler();
            }

            return Activator.CreateInstance(serviceType);
        }
    }
}