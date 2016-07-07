using System;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.Mediation;
using TechFu.Nirvana.WebApi.Sample.DomainSpecificData;
using TechFu.Nirvana.WebApi.Sample.DomainSpecificData.Handlers;
using TechFu.Nirvana.WebApi.Sample.DomainSpecificData.Queries;

namespace TechFu.Nirvana.WebApi.Sample
{
    public class TestNirvanaConfig : NirvanaConfiguration
    {
        public override Type RootType => typeof(RootType);
        public override Type AggregateAttributeType => typeof(AggregateRootAttribute);

        public override Func<string, object, bool> AttributeMatchingFunction
            => (x, y) => x == ((AggregateRootAttribute) y).RootType.ToString();

        public override string[] AssemblyNameReferences => new[]
        {
            "TechFu.Nirvana.dll",
            "TechFu.Nirvana.WebApi.dll",
            "TechFu.Nirvana.WebApi.Sample.dll"
        };

        
        public override string[] AdditionalNamespaceReferences => new[] { "TechFu.Services.Shared.Command" };

        //Plug your IoC in here
        public override object GetService(Type serviceType)
        {
            if (serviceType == typeof(IMediator))
            {
                return new Mediator(this);
            }

            if (serviceType == typeof(IQueryHandler<GetVersionQuery,string>))
            {
                return new GetVersionHandler();
            }

            return Activator.CreateInstance(serviceType);
        }
    }
}