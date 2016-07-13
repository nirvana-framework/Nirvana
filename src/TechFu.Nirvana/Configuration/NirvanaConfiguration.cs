using System;

namespace TechFu.Nirvana.Configuration
{
//    public abstract class NirvanaConfiguration
//    {
//        public abstract Type RootType { get; }
//        public abstract Type AggregateAttributeType { get; }
//        public abstract Func<string, object, bool> AttributeMatchingFunction { get; }
//        public abstract string[] AssemblyNameReferences { get; }
//        public abstract string[] AdditionalNamespaceReferences { get; }
//
//        public abstract object GetService(Type serviceType);
//    }

    public static class NirvanaSetup
    {
//        public static NirvanaConfiguration Configuration { get; private set; }

        public static Type RootType { get; internal set; }
        public static Type AggregateAttributeType { get; internal set; }
        public static  Func<string, object, bool> AttributeMatchingFunction { get; internal set; }
        public static  string[] AssemblyNameReferences { get; internal set; }
        public static ControllerType ControllerTypes { get; internal set; }
        public static Func<Type, Object> GetService { get; internal set; }
        public static bool CommandsToQueueEndpoint { get; set; }
        public static QueueStrategy QueueStrategy { get; set; }


        public static NirvanaConfigurationHelper Configure()
        {
            return new NirvanaConfigurationHelper();
        }

    }

    public enum ControllerType
    {
        Command,
        Query,
        CommandAndQuery
    }

    public enum QueueStrategy
    {
        None=0,
        AllCommands=1,
        LongRunningCommands=2,


    }
}