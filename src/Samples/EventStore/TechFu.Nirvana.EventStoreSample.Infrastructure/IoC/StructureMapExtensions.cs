using StructureMap;

namespace TechFu.Nirvana.EventStoreSample.Infrastructure.IoC
{
    public static class StructureMapExtensions
    {
        public static void RegisterPerRequest<TPluginType, TConcrete>(this ConfigurationExpression x)
            where TConcrete : TPluginType
        {
            // The dependency resolver will always resolve per http request or command processor message
            x.For<TPluginType>().Use(() => InternalDependencyResolver.GetInstance<TConcrete>());
        }
    }
}