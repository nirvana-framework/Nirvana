using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TechFu.Nirvana.CQRS.Util;
using TechFu.Nirvana.Util.Extensions;

namespace TechFu.Nirvana.Configuration
{


    public static class NirvanaSetup
    {
        //Wonky settings, fix these...
        public static Guid ApplicationLevelViewModelKey { get; set; } = Guid.Parse("f9f54603-2cda-43da-a414-cd7f12eae4a1");
        public static string UiNotificationHubName { get; set; } = "UiNotifications";

        public static string AssemblyFolder { get; set; }
        public static string ControllerAssemblyName { get; set; }
        public static string ControllerRootNamespace { get; set; }
        public static Type RootType { get; internal set; }
        public static Type AggregateAttributeType { get; internal set; }
        public static string TaskIdentifierProperty { get; internal set; }
        public static  Func<string, object, bool> AttributeMatchingFunction { get; internal set; }
        public static  string[] AssemblyNameReferences { get; internal set; }
        //public static TaskType[] TaskTypes { get; internal set; }
        public static Func<Type, object> GetService { get; internal set; }
        public static Func<Type, object[]> GetServices { get; internal set; }

//        //processing configuration
//        public static MediationStrategy CommandMediationStrategy { get; internal  set; } = MediationStrategy.InProcess;
//        public static MediationStrategy QueryMediationStrategy { get; internal  set; } = MediationStrategy.InProcess;
//        public static MediationStrategy UiNotificationMediationStrategy { get; internal  set; } = MediationStrategy.None;
//
//
//        //Configuration for task processor applications
//        public static MediationStrategy RecieverMediationStrategy { get; internal set; } = MediationStrategy.None;
//
//        //How to handle sub calls within this app 
//        // FOr instance, we get a call from a command processor via web
//        // we want to call this in proc and syncronously
//        //somewhere in the command we want to call a child command  - this may or may not be something 
//        //we want to run in proc
//        public static ChildMediationStrategy ChildMediationStrategy { get; internal  set; } = ChildMediationStrategy.Synchronous;


        //Called On Configuration build
        public static string[] RootNames { get; internal set; }
        public static IDictionary<string, NirvanaTypeRoutingDefinition[]> QueryTypes{ get; internal set; }
        public static IDictionary<string, NirvanaTypeRoutingDefinition[]> CommandTypes{ get; internal set; }
        public static IDictionary<string, NirvanaTypeRoutingDefinition[]> UiNotificationTypes{ get; internal set; }
        public static IDictionary<string, NirvanaTypeRoutingDefinition[]> InternalEventTypes { get; set; }
        public static IDictionary<Type, NirvanaTypeRoutingDefinition> DefinitionsByType{ get; internal set; }
        public static Dictionary<TaskType, NirvanaTypeRoutingDefinition> TaskConfiguration { get; set; }


        //TODO - replace CqrsUtils.GetRootTypeName  with this and use it in that function to speed up
        //public static IDictionary<Type, string> TypeRootNames{ get; internal set; }


        public static NirvanaConfigurationHelper Configure()
        {
            return new NirvanaConfigurationHelper();
        }

        public static string ShowSetup()
        {
            var builder = new StringBuilder();
            var propertyInfos = typeof(NirvanaSetup).GetProperties(BindingFlags.Public | BindingFlags.Static);
            var fieldInfos = typeof(NirvanaSetup).GetFields(BindingFlags.Public | BindingFlags.Static);

            propertyInfos.ForEach(x =>
            {

                builder.AppendLine($"{x.Name} : {x.GetValue(null)}");
            });
            fieldInfos.ForEach(x =>
            {
                builder.AppendLine($"{x.Name} : {x.GetValue(null)}");
            });

            return builder.ToString();

        }


        public static NirvanaTypeRoutingDefinition FindTypeDefinition(Type getType)
        {

            return DefinitionsByType[getType];
        }

        public static bool IsInProcess(TaskType taskType, bool isChildTask)
        {
            var config = GetTaskConfiguration(taskType);


            return config.CanHandle
                &&
                isChildTask
                ? config.ChildMediationStrategy == MediationStrategy.InProcess
                : config.MediationStrategy == MediationStrategy.InProcess;
        }

        public static bool ShouldForwardToWeb(TaskType taskType, bool isChildTask)
        {
            var config = GetTaskConfiguration(taskType);


            return config.CanHandle
                &&
                isChildTask
                ? config.ChildMediationStrategy == MediationStrategy.ForwardToWeb
                : config.MediationStrategy == MediationStrategy.ForwardToWeb;


        }
        public static bool ShouldForwardToQueue(TaskType taskType, bool isChildTask)
        {
            var config = GetTaskConfiguration(taskType);
            

            return config.CanHandle 
                && 
                isChildTask
                ? config.ChildMediationStrategy== MediationStrategy.ForwardToQueue 
                :config.MediationStrategy == MediationStrategy.ForwardToQueue;
        }

        private static TaskConfiguration GetTaskConfiguration(TaskType taskType)
        {
            return TaskConfiguration[taskType];
        }

        public static bool CanProcess(TaskType query)
        {
            return TaskConfiguration[query].CanHandle;
        }
    }
}