using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nirvana.CQRS;
using Nirvana.CQRS.Util;
using Nirvana.Domain;
using Nirvana.Security;
using Nirvana.Util.Extensions;

namespace Nirvana.Configuration
{
    public class NirvanaConfigurationHelper
    {
        private readonly Dictionary<TaskType, NirvanaTypeRoutingDefinition> _taskConfiguration;
        public readonly NirvanaSetup Setup;
        public NirvanaConfigurationHelper()
        {
            Setup = new NirvanaSetup();
            _taskConfiguration = new Dictionary<TaskType, NirvanaTypeRoutingDefinition>();
        }

        public NirvanaConfigurationHelper SetRootTypeAssembly(Assembly typeAssembly)
        {
            Setup.RootTypeAssembly = typeAssembly;
            return this;
        }

        public NirvanaConfigurationHelper SetDependencyResolver(Func<Type, object> resolverMethod,
            Func<Type, object[]> resolveMultipleMethod)
        {
            Setup.GetService = resolverMethod;
            Setup.GetServices = resolveMultipleMethod;
            return this;
        }

     

        public NirvanaConfigurationHelper SetAttributeMatchingFunction(Func<string, object, bool> method)
        {
            Setup.AttributeMatchingFunction = method;
            return this;
        }

        public NirvanaConfigurationHelper SetAdditionalAssemblyNameReferences(string[] refrences)
        {

             var commonReferences = new[]
                {
                    "Nirvana.dll",
                    "Nirvana.Web.dll"
                };

            Setup.AssemblyNameReferences = commonReferences.Concat(refrences).ToArray();
            return this;
        }

        public NirvanaConfigurationHelper UsingControllerName(string controllerAssemblyName, string rootNamesapce)
        {
            Setup.ControllerAssemblyName = controllerAssemblyName;
            Setup.ControllerRootNamespace = rootNamesapce;
            return this;
        }

        public NirvanaConfigurationHelper WithAssembliesFromFolder(string assemblyFolder)
        {
            Setup.AssemblyFolder = assemblyFolder;
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


        public NirvanaSetup BuildConfiguration()
        {

            var rootTypeNames = ObjectExtensions.AddAllTypesFromAssembliesContainingTheseSeedTypes
                (x => typeof(RootType).IsAssignableFrom(x), Setup.RootTypeAssembly)
                .Select(x=>(Activator.CreateInstance(x) as RootType).RootName);

            Setup.TaskIdentifierProperty = "Identifier";
            Setup.RootNames = rootTypeNames.ToArray();
            Setup.QueryTypes = Setup.RootNames.ToDictionary(x => x, x => GetTypes(x, typeof(Query<>)));
            Setup.CommandTypes = Setup.RootNames.ToDictionary(x => x, x => GetTypes(x, typeof(Command<>)));
            Setup.UiNotificationTypes = Setup.RootNames.ToDictionary(x => x,
                x => GetTypes(x, typeof(UiEvent<>)));
            Setup.InternalEventTypes = Setup.RootNames.ToDictionary(x => x,
                x => GetTypes(x, typeof(InternalEvent)));

            var definitions = GetTypes(Setup.QueryTypes)
                .Union(GetTypes(Setup.CommandTypes))
                .Union(GetTypes(Setup.UiNotificationTypes))
                .Union(GetTypes(Setup.InternalEventTypes)).ToArray();

            Setup.DefinitionsByType = definitions.ToDictionary(x => x.TaskType, x => x);


            BuildTaskConfiguration();
            return Setup;
        }

        private void BuildTaskConfiguration()
        {
            GuiarDisabledTasks(TaskType.Command);
            GuiarDisabledTasks(TaskType.Query);
            GuiarDisabledTasks(TaskType.UiNotification);
            GuiarDisabledTasks(TaskType.InternalEvent);

            Setup.TaskConfiguration = _taskConfiguration;

            Setup.TaskConfiguration[TaskType.Command].Tasks =
                Setup.CommandTypes.SelectMany(x => x.Value).ToArray();

            Setup.TaskConfiguration[TaskType.Query].Tasks =
                Setup.QueryTypes.SelectMany(x => x.Value).ToArray();

            Setup.TaskConfiguration[TaskType.UiNotification].Tasks =
                Setup.UiNotificationTypes.SelectMany(x => x.Value).ToArray();

            Setup.TaskConfiguration[TaskType.InternalEvent].Tasks =
                Setup.InternalEventTypes.SelectMany(x => x.Value).ToArray();

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


        private NirvanaTaskInformation[] GetTypes(string rootName, Type taskType)
        {
            return
                Setup.TaskTypes(taskType, rootName)
                    .Select(t => BuildTypeDefinition(t, rootName))
                    .ToArray();
        }

        private NirvanaTaskInformation BuildTypeDefinition(Type taskType, string rootName)
        {
            var customAttribute = CqrsUtils.CustomAttribute(taskType);
            var taskInfo = new NirvanaTaskInformation
            {
                TaskType = taskType,
                TypeCorrelationId = GetTypeCorrelationId(customAttribute),
                UniqueName = GetUniqueName(taskType, rootName),
                RootName = rootName,
                Claims = BuildClaims(taskType),
                LongRunning = customAttribute.LongRunning
            };
            taskInfo.RequiresAuthentication = taskInfo.Claims.Any() || (customAttribute?.Authorized ?? false);
            return taskInfo;
        }

        private Dictionary<ClaimType, AccessType[]> BuildClaims(Type taskType)
        {
            var list = new List<KeyValuePair<ClaimType, AccessType[]>>();

            var attrs = taskType.GetCustomAttributes(true);
            foreach (object attr in attrs)
            {
                var authAttr = attr as ClaimTypeAttribute;
                if (authAttr != null)
                {

                    list.Add(new KeyValuePair<ClaimType, AccessType[]>(authAttr.ClaimType, authAttr.AllowedActions));
                }
            }


            return list.ToDictionary(x => x.Key, x => x.Value);
        }

        private string GetUniqueName(Type taskType, string rootName)
        {
            return $"{rootName}-{taskType.Name}".ToLower();
        }

        //TODO - more robust necessary here...
        private string GetTypeCorrelationId(Attribute rootAttribute)
        {
            return rootAttribute.GetProperty(Setup.TaskIdentifierProperty).ToString();
        }
    }
}