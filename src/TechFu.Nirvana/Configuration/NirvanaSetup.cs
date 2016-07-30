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


        //Called On Configuration build
        public static string[] RootNames { get; internal set; }
        public static IDictionary<string, NirvanaTaskInformation[]> QueryTypes{ get; internal set; }
        public static IDictionary<string, NirvanaTaskInformation[]> CommandTypes{ get; internal set; }
        public static IDictionary<string, NirvanaTaskInformation[]> UiNotificationTypes{ get; internal set; }
        public static IDictionary<string, NirvanaTaskInformation[]> InternalEventTypes { get; set; }

        public static IDictionary<Type, NirvanaTaskInformation> DefinitionsByType{ get; internal set; }

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


        public static NirvanaTaskInformation FindTypeDefinition(Type getType)
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

        private static NirvanaTypeRoutingDefinition GetTaskConfiguration(TaskType taskType)
        {
            return TaskConfiguration[taskType];
        }

        public static bool CanProcess(TaskType query)
        {
            return TaskConfiguration[query].CanHandle;
        }
    }
}