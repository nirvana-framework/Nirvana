using System;
using Nirvana.Domain;
using Nirvana.Mediation;
using Nirvana.Web.Controllers;
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
            "TechFu.Nirvana.WebApi.Sample.dll"
        };

        public Type[] InlineControllerTypes => new[] {typeof(ApiUpdatesController)};


        //Plug your IoC in here
        public  object GetService(Type serviceType)
        {
            if (serviceType == typeof(IMediatorFactory))
            {
                return new MediatorFactory();
            }

            if (serviceType == typeof(IQueryHandler<GetVersionQuery,string>))
            {
                return new GetVersionHandler(new ChildMediatorFactory());
            }

            return Activator.CreateInstance(serviceType);
        }


        
        public object[] GetAllServices(Type serviceType) => new object[0];
    }
}