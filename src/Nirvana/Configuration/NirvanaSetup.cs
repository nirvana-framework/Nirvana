using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Nirvana.Util.Extensions;

namespace Nirvana.Configuration
{



    public class NirvanaSetup
    {
        //Wonky settings, fix these...
        public Guid ApplicationLevelViewModelKey { get; set; } = Guid.Parse("f9f54603-2cda-43da-a414-cd7f12eae4a1");
        public string UiNotificationHubName { get; set; } = "UiNotifications";

        public string AssemblyFolder { get; set; }
        public string ControllerAssemblyName { get; set; }
        public string ControllerRootNamespace { get; set; }
        public Assembly RootTypeAssembly { get; internal set; }
        public string TaskIdentifierProperty { get; internal set; }
        public Func<string, object, bool> AttributeMatchingFunction { get; internal set; }
        public string[] AssemblyNameReferences { get; internal set; }

        public Func<Type, object> GetService { get; internal set; }
        public Func<Type, object[]> GetServices { get; internal set; }


        //Called On Configuration build
        public string[] RootNames { get; internal set; }
        public IDictionary<string, NirvanaTaskInformation[]> QueryTypes { get; internal set; }
        public IDictionary<string, NirvanaTaskInformation[]> CommandTypes { get; internal set; }
        public IDictionary<string, NirvanaTaskInformation[]> UiNotificationTypes { get; internal set; }
        public IDictionary<string, NirvanaTaskInformation[]> InternalEventTypes { get; set; }

        public IDictionary<Type, NirvanaTaskInformation> DefinitionsByType { get; internal set; }

        public Dictionary<TaskType, NirvanaTypeRoutingDefinition> TaskConfiguration { get; set; }


        //TODO - replace CqrsUtils.GetRootTypeName  with this and use it in that function to speed up
        //public  IDictionary<Type, string> TypeRootNames{ get; internal set; }


        public static NirvanaConfigurationHelper Configure()
        {
            return new NirvanaConfigurationHelper();
        }

        public string ShowSetup()
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


        public NirvanaTaskInformation FindTypeDefinition(Type getType)
        {

            return DefinitionsByType[getType];
        }

        public bool IsInProcess(TaskType taskType, bool isChildTask)
        {
            var config = GetTaskConfiguration(taskType);


            return config.CanHandle
                &&
                isChildTask
                ? config.ChildMediationStrategy == MediationStrategy.InProcess
                : config.MediationStrategy == MediationStrategy.InProcess;
        }

        public bool ShouldForwardToWeb(TaskType taskType, bool isChildTask)
        {
            var config = GetTaskConfiguration(taskType);


            return config.CanHandle
                &&
                isChildTask
                ? config.ChildMediationStrategy == MediationStrategy.ForwardToWeb
                : config.MediationStrategy == MediationStrategy.ForwardToWeb;


        }
        public bool ShouldForwardToQueue(TaskType taskType, bool isChildTask, Type messageType)
        {
            var config = GetTaskConfiguration(taskType);

            if (!config.CanHandle)
            {
                return false;
            }

            var taskInfo = FindTypeDefinition(messageType);

            if (taskInfo.LongRunning)
            {
                return isChildTask
                    ? config.ChildMediationStrategy == MediationStrategy.ForwardToQueue
                      || config.ChildMediationStrategy == MediationStrategy.ForwardLongRunningToQueue
                    : config.MediationStrategy == MediationStrategy.ForwardToQueue
                      || config.MediationStrategy == MediationStrategy.ForwardLongRunningToQueue;
            }


            return isChildTask
                ? config.ChildMediationStrategy == MediationStrategy.ForwardToQueue
                : config.MediationStrategy == MediationStrategy.ForwardToQueue;
        }

        private NirvanaTypeRoutingDefinition GetTaskConfiguration(TaskType taskType)
        {
            return TaskConfiguration[taskType];
        }

        public bool CanProcess(TaskType query)
        {
            return TaskConfiguration[query].CanHandle;
        }
    }
}