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
        private readonly Dictionary<TaskType, TaskConfiguration> _taskConfiguration;

        public NirvanaConfigurationHelper()
        {
            _taskConfiguration = new Dictionary<TaskType, TaskConfiguration>();
        }

        public NirvanaConfigurationHelper SetRootType(Type rootType)
        {
            NirvanaSetup.RootType = rootType;
            return this;
        }

        public NirvanaConfigurationHelper SetDependencyResolver(Func<Type, object> resolverMethod)
        {
            NirvanaSetup.GetService = resolverMethod;
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

        public NirvanaConfigurationHelper ForCommands(MediationStrategy mediationStrategy, ChildMediationStrategy childMediationStrategy, MediationStrategy receiverMediationStrategy)
        {
            AddTaskConfig(mediationStrategy, childMediationStrategy, receiverMediationStrategy, TaskType.UiNotification);
            return this;
        }
        public NirvanaConfigurationHelper ForQueries(MediationStrategy mediationStrategy, ChildMediationStrategy childMediationStrategy)
        {
            AddTaskConfig(mediationStrategy, childMediationStrategy, MediationStrategy.None, TaskType.UiNotification);
            return this;
        }

        public NirvanaConfigurationHelper ForUiNotifications(MediationStrategy mediationStrategy, ChildMediationStrategy childMediationStrategy,MediationStrategy receiverMediationStrategy)
        {
            AddTaskConfig(mediationStrategy, childMediationStrategy, receiverMediationStrategy, TaskType.UiNotification);
            return this;
        }
        public NirvanaConfigurationHelper ForInternalEvents(MediationStrategy mediationStrategy, ChildMediationStrategy childMediationStrategy, MediationStrategy receiverMediationStrategy)
        {
            AddTaskConfig(mediationStrategy, childMediationStrategy, receiverMediationStrategy, TaskType.UiNotification);

            return this;
        }

        private void AddTaskConfig(MediationStrategy mediationStrategy, ChildMediationStrategy childMediationStrategy,
            MediationStrategy receiverMediationStrategy, TaskType taskType)
        {
            _taskConfiguration[taskType] = new TaskConfiguration
            {
                CanHandle = true,
                MediationStrategy = mediationStrategy,
                ReceiverMediationStrategy = receiverMediationStrategy,
                TaskType = taskType,
                ChildMediationStrategy = childMediationStrategy
            };
        }
        

        public void BuildConfiguration()
        {
            NirvanaSetup.TaskIdentifierProperty = "Identifier";
            NirvanaSetup.RootNames = EnumExtensions.GetAll(NirvanaSetup.RootType).SelectToArray(x => x.Value);
            NirvanaSetup.QueryTypes = NirvanaSetup.RootNames.ToDictionary(x => x, x => GetTypes(x, typeof(Query<>)));
            NirvanaSetup.CommandTypes = NirvanaSetup.RootNames.ToDictionary(x => x, x => GetTypes(x, typeof(Command<>)));
            NirvanaSetup.UiNotificationTypes = NirvanaSetup.RootNames.ToDictionary(x => x,x => GetTypes(x, typeof(UiEvent<>)));
            NirvanaSetup.InternalEventTypes = NirvanaSetup.RootNames.ToDictionary(x => x,x => GetTypes(x, typeof(InternalEvent)));

            var definitions = GetTypes(NirvanaSetup.QueryTypes)
                .Union(GetTypes(NirvanaSetup.CommandTypes))
                .Union(GetTypes(NirvanaSetup.UiNotificationTypes))
                .Union(GetTypes(NirvanaSetup.InternalEventTypes)).ToArray();

            ValidateConfiguration(definitions);
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
        }

        private void GuiarDisabledTasks(TaskType taskType)
        {
            if (!_taskConfiguration.ContainsKey(taskType))
            {
                _taskConfiguration[taskType] = BuildDisabledTaskConfiguration(taskType);
            }
        }

        private TaskConfiguration BuildDisabledTaskConfiguration(TaskType taskType)
        {




            return BuildTaskConfiguration(MediationStrategy.None, taskType,
                false, ChildMediationStrategy.InProcess, MediationStrategy.None);
        }

        private TaskConfiguration BuildTaskConfiguration(MediationStrategy ms, TaskType taskType, bool canHandle, ChildMediationStrategy forwardLongRunningToQueue, MediationStrategy receiverStrategy)
        {
            return new TaskConfiguration
            {
                MediationStrategy = ms,
                TaskType = taskType,
                CanHandle = canHandle,
                ChildMediationStrategy = forwardLongRunningToQueue,ReceiverMediationStrategy = receiverStrategy
            };
        }

        

        private void ValidateConfiguration(IEnumerable<NirvanaTypeDefinition> definitions)
        {
            //TODO - configure this to throw errors for early failuse.
        }


        private static NirvanaTypeDefinition[] GetTypes(IDictionary<string, NirvanaTypeDefinition[]> nirvanaTypeDefinitionses)
        {
            return nirvanaTypeDefinitionses.Keys.SelectMany(x=> nirvanaTypeDefinitionses[x]).ToArray();
        }

     

        private NirvanaTypeDefinition[] GetTypes(string rootName, Type actionType)
        {
            return
                CqrsUtils.ActionTypes(actionType, rootName)
                    .Select(t => BuildTypeDefinition(t, actionType, rootName))
                    .ToArray();
        }

        private NirvanaTypeDefinition BuildTypeDefinition(Type taskType, Type actionType, string rootName)
        {
            return new NirvanaTypeDefinition
            {
                NirvanaActionType = actionType,
                TaskType = taskType,
                TypeCorrelationId = GetTypeCorrelationID(taskType),
                UniqueName = GetUniqueName(taskType, rootName)
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

    public class TaskConfiguration
    {
        public MediationStrategy ReceiverMediationStrategy { get; set; }
        public MediationStrategy MediationStrategy { get; set; }
        public ChildMediationStrategy ChildMediationStrategy { get; set; }
        public TaskType TaskType { get; set; }
        public bool CanHandle { get; set; }
    }
}