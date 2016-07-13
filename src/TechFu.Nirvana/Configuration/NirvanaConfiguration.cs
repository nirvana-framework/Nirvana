using System;

namespace TechFu.Nirvana.Configuration
{


    public static class NirvanaSetup
    {
        public static Type RootType { get; internal set; }
        public static Type AggregateAttributeType { get; internal set; }
        public static  Func<string, object, bool> AttributeMatchingFunction { get; internal set; }
        public static  string[] AssemblyNameReferences { get; internal set; }
        public static ControllerType ControllerTypes { get; internal set; }
        public static Func<Type, Object> GetService { get; internal set; }

        //Queue Configuration
        public static bool CommandsToQueueEndpoint { get; set; } = false;
        public static QueueStrategy QueueStrategy { get; set; } = QueueStrategy.None;
        public static WebMediationStrategy WebMediationStrategy { get; set; }


        public static NirvanaConfigurationHelper Configure()
        {
            return new NirvanaConfigurationHelper();
        }

    }
}