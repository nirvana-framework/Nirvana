using System;
using System.Collections.Generic;

namespace TechFu.Nirvana.Configuration
{


    public static class NirvanaSetup
    {
        public static Type RootType { get; internal set; }
        public static Type AggregateAttributeType { get; internal set; }
        public static  Func<string, object, bool> AttributeMatchingFunction { get; internal set; }
        public static  string[] AssemblyNameReferences { get; internal set; }
        public static ControllerType[] ControllerTypes { get; internal set; }
        public static Func<Type, Object> GetService { get; internal set; }

        //Queue Configuration
        public static bool CommandsToQueueEndpoint { get; set; } = false;
        public static bool CommandsFromQueueEndpoint { get; set; } = false;
        public static QueueStrategy QueueStrategy { get; set; } = QueueStrategy.None;
        public static WebMediationStrategy WebMediationStrategy { get; set; } = WebMediationStrategy.None;


        //Called On Configuration build
        public static string[] RootNames { get; internal set; }
        public static IDictionary<string,Type[]> QueryTypes{ get; internal set; }
        public static IDictionary<string,Type[]> CommandTypes{ get; internal set; }
        public static IDictionary<string,Type[]> UiNotificationTypes{ get; internal set; }


        public static NirvanaConfigurationHelper Configure()
        {
            return new NirvanaConfigurationHelper();
        }

    }
}