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

        //processing configuration
        public static MediationStrategy CommandMediationStrategy { get; internal  set; } = MediationStrategy.InProcess;
        public static MediationStrategy QueryMediationStrategy { get; internal  set; } = MediationStrategy.InProcess;
        public static MediationStrategy UiNotificationMediationStrategy { get; internal  set; } = MediationStrategy.InProcess;


        //Configuration for task processor applications
        public static MediationStrategy RecieverMediationStrategy { get; internal set; } = MediationStrategy.None;

        //How to handle sub calls within this app 
        // FOr instance, we get a call from a command processor via web
        // we want to call this in proc and syncronously
        //somewhere in the command we want to call a child command  - this may or may not be something 
        //we want to run in proc
        public static ChildMediationStrategy ChildMediationStrategy { get; internal  set; } = ChildMediationStrategy.Synchronous;


        //Called On Configuration build
        public static string[] RootNames { get; internal set; }
        public static IDictionary<string,Type[]> QueryTypes{ get; internal set; }
        public static IDictionary<string,Type[]> CommandTypes{ get; internal set; }
        public static IDictionary<string,Type[]> UiNotificationTypes{ get; internal set; }
        
        //TODO - replace CqrsUtils.GetRootTypeName  with this and use it in that function to speed up
        //public static IDictionary<Type, string> TypeRootNames{ get; internal set; }


        public static NirvanaConfigurationHelper Configure()
        {
            return new NirvanaConfigurationHelper();
        }

    }
}