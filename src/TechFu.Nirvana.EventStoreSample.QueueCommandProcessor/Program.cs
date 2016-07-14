using System.Reflection;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.EventStoreSample.Infrastructure.IoC;

namespace TechFu.Nirvana.EventStoreSample.QueueCommandProcessor
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            StructureMapAspNet.Configure(Assembly.GetExecutingAssembly());
            var config = new TestNirvanaConfig();

            NirvanaSetup.Configure()
               .SetAdditionalAssemblyNameReferences(config.AssemblyNameReferences)
               .SetRootType(config.RootType)
               .SetAggregateAttributeType(config.AggregateAttributeType)
               .SetAttributeMatchingFunction(config.AttributeMatchingFunction)
               .SetDependencyResolver(config.GetService)
               .ForCommands()
               .FromQueues(QueueStrategy.AllCommands,WebMediationStrategy.ForwardAll)
               
               ;


        }
    }

   
}