using System;
using System.Collections.Generic;
using System.Linq;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.CQRS.Util;
using TechFu.Nirvana.Util.Extensions;

namespace TechFu.Nirvana.Configuration
{
    public class NirvanaConfigurationHelper
    {
        private readonly Dictionary<TaskType, NirvanaTypeRoutingDefinition> _taskConfiguration;

        public NirvanaConfigurationHelper()
        {
            _taskConfiguration = new Dictionary<TaskType, NirvanaTypeRoutingDefinition>();
        }

        public NirvanaConfigurationHelper SetRootType(Type rootType)
        {
            NirvanaSetup.RootType = rootType;
            return this;
        }

        public NirvanaConfigurationHelper SetDependencyResolver(Func<Type, object> resolverMethod,
            Func<Type, object[]> resolveMultipleMethod)
        {
            NirvanaSetup.GetService = resolverMethod;
            NirvanaSetup.GetServices = resolveMultipleMethod;
            return this;
        }

        public NirvanaConfigurationHelper SetAggregateAttributeType(Type attributeType)
        {
            NirvanaSetup.AggregateAttributeType = attributeType;
            return this;
        }

        public NirvanaConfigurationHelper SetAttributeMatchingFunction(Func<string, object, bool> method)
        {
            NirvanaSetup.AttributeMatchingFunction = method;
            return this;
        }

        public NirvanaConfigurationHelper SetAdditionalAssemblyNameReferences(string[] refrences)
        {
            NirvanaSetup.AssemblyNameReferences = refrences;
            return this;
        }

        public NirvanaConfigurationHelper UsingControllerName(string controllerAssemblyName, string rootNamesapce)
        {
            NirvanaSetup.ControllerAssemblyName = controllerAssemblyName;
            NirvanaSetup.ControllerRootNamespace = rootNamesapce;
            return this;
        }

        public NirvanaConfigurationHelper WithAssembliesFromFolder(string assemblyFolder)
        {
            NirvanaSetup.AssemblyFolder = assemblyFolder;
            return this;
        }

        public NirvanaConfigurationHelper ForCommands(MediationStrategy mediationStrategy,
            MediationStrategy childMediationStrategy, MediationStrategy receiverMediationStrategy)
        {
            AddTaskConfig(mediationStrategy, childMediationStrategy, receiverMediationStrategy, TaskType.Command);
            return this;
        }

        public NirvanaConfigurationHelper ForQueries(MediationStrategy mediationStrategy,
            MediationStrategy childMediationStrategy)
        {
            AddTaskConfig(mediationStrategy, childMediationStrategy, MediationStrategy.None, TaskType.Query);
            return this;
        }

        public NirvanaConfigurationHelper ForUiNotifications(MediationStrategy mediationStrategy,
            MediationStrategy childMediationStrategy, MediationStrategy receiverMediationStrategy)
        {
            AddTaskConfig(mediationStrategy, childMediationStrategy, receiverMediationStrategy, TaskType.UiNotification);
            return this;
        }

        public NirvanaConfigurationHelper ForInternalEvents(MediationStrategy mediationStrategy,
            MediationStrategy childMediationStrategy, MediationStrategy receiverMediationStrategy)
        {
            AddTaskConfig(mediationStrategy, childMediationStrategy, receiverMediationStrategy, TaskType.InternalEvent);

            return this;
        }

        private void AddTaskConfig(MediationStrategy mediationStrategy, MediationStrategy childMediationStrategy,
            MediationStrategy receiverMediationStrategy, TaskType taskType)
        {
            _taskConfiguration[taskType] = new NirvanaTypeRoutingDefinition
            {
                CanHandle = true,
                MediationStrategy = mediationStrategy,
                ReceiverMediationStrategy = receiverMediationStrategy,
                NirvanaTaskType = taskType,
                ChildMediationStrategy = childMediationStrategy,
                Tasks =new NirvanaTaskInformation[0]
            };
        }


        public void BuildConfiguration()
        {
            NirvanaSetup.TaskIdentifierProperty = "Identifier";
            NirvanaSetup.RootNames = EnumExtensions.GetAll(NirvanaSetup.RootType).SelectToArray(x => x.Value);
            NirvanaSetup.QueryTypes = NirvanaSetup.RootNames.ToDictionary(x => x, x => GetTypes(x, typeof(Query<>)));
            NirvanaSetup.CommandTypes = NirvanaSetup.RootNames.ToDictionary(x => x, x => GetTypes(x, typeof(Command<>)));
            NirvanaSetup.UiNotificationTypes = NirvanaSetup.RootNames.ToDictionary(x => x,
                x => GetTypes(x, typeof(UiEvent<>)));
            NirvanaSetup.InternalEventTypes = NirvanaSetup.RootNames.ToDictionary(x => x,
                x => GetTypes(x, typeof(InternalEvent)));

            var definitions = GetTypes(NirvanaSetup.QueryTypes)
                .Union(GetTypes(NirvanaSetup.CommandTypes))
                .Union(GetTypes(NirvanaSetup.UiNotificationTypes))
                .Union(GetTypes(NirvanaSetup.InternalEventTypes)).ToArray();

            NirvanaSetup.DefinitionsByType = definitions.ToDictionary(x => x.TaskType, x => x);


            BuildTaskConfiguration();
        }

        private void BuildTaskConfiguration()
        {
            GuiarDisabledTasks(TaskType.Command);
            GuiarDisabledTasks(TaskType.Query);
            GuiarDisabledTasks(TaskType.UiNotification);
            GuiarDisabledTasks(TaskType.InternalEvent);

            NirvanaSetup.TaskConfiguration = _taskConfiguration;

            NirvanaSetup.TaskConfiguration[TaskType.Command].Tasks =
                NirvanaSetup.CommandTypes.SelectMany(x => x.Value).ToArray();

            NirvanaSetup.TaskConfiguration[TaskType.Query].Tasks =
                NirvanaSetup.QueryTypes.SelectMany(x => x.Value).ToArray();

            NirvanaSetup.TaskConfiguration[TaskType.UiNotification].Tasks =
                NirvanaSetup.UiNotificationTypes.SelectMany(x => x.Value).ToArray();

            NirvanaSetup.TaskConfiguration[TaskType.InternalEvent].Tasks =
                NirvanaSetup.InternalEventTypes.SelectMany(x => x.Value).ToArray();

        }

        private void GuiarDisabledTasks(TaskType taskType)
        {
            if (!_taskConfiguration.ContainsKey(taskType))
            {
                _taskConfiguration[taskType] = BuildDisabledTaskConfiguration(taskType);
            }
        }

        private NirvanaTypeRoutingDefinition BuildDisabledTaskConfiguration(TaskType taskType)
        {
            return BuildTaskConfiguration(MediationStrategy.None, taskType,
                false, MediationStrategy.None, MediationStrategy.None);
        }

        private NirvanaTypeRoutingDefinition BuildTaskConfiguration(MediationStrategy outboundStrategy,
            TaskType taskType,
            bool canHandle, MediationStrategy childTaskStrategy, MediationStrategy receiverStrategy)
        {
            return new NirvanaTypeRoutingDefinition
            {
                MediationStrategy = outboundStrategy,
                NirvanaTaskType = taskType,
                CanHandle = canHandle,
                ChildMediationStrategy = childTaskStrategy,
                ReceiverMediationStrategy = receiverStrategy,
                Tasks = new NirvanaTaskInformation[0]
            };
        }


        private void ValidateConfiguration(IEnumerable<NirvanaTaskInformation> definitions)
        {
            //TODO - configure this to throw errors for early failuse.
        }


        private static NirvanaTaskInformation[] GetTypes(
            IDictionary<string, NirvanaTaskInformation[]> rootTypeData)
        {
            var items = rootTypeData.Keys.SelectMany(k => rootTypeData[k]);

            return items.ToArray();
        }


        private NirvanaTaskInformation[] GetTypes(string rootName, Type actionType)
        {
            return
                CqrsUtils.ActionTypes(actionType, rootName)
                    .Select(t => BuildTypeDefinition(t, actionType, rootName))
                    .ToArray();
        }

        private NirvanaTaskInformation BuildTypeDefinition(Type taskType, Type actionType, string rootName)
        {
            return new NirvanaTaskInformation
            {
                TaskType = taskType,
                TypeCorrelationId = GetTypeCorrelationID(taskType),
                UniqueName = GetUniqueName(taskType, rootName),
                RootName = rootName,
            };
        }

        private string GetUniqueName(Type taskType, string rootName)
        {
            return $"{rootName}-{taskType.Name}".ToLower();
        }

        //TODO - more robust necessary here...
        private string GetTypeCorrelationID(Type taskType)
        {
            var aggType = CqrsUtils.CustomAttribute(taskType);
            return aggType.GetProperty(NirvanaSetup.TaskIdentifierProperty).ToString();
        }
    }
}