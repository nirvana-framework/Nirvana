using System;
using System.Linq;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.CQRS.Util;
using TechFu.Nirvana.Util.Extensions;

namespace TechFu.Nirvana.Configuration
{
    public class NirvanaConfigurationHelper
    {
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

        public NirvanaConfigurationHelper ForCommands()
        {
            NirvanaSetup.ControllerTypes = new[] {ControllerType.Command};
            return this;
        }

        public NirvanaConfigurationHelper HandleInProc()
        {
            NirvanaSetup.CommandMediationStrategy = MediationStrategy.InProcess;
            return this;
        }

        public NirvanaConfigurationHelper ToQueues()
        {
            NirvanaSetup.CommandMediationStrategy = MediationStrategy.ForwardToQueue;
            return this;
        }

        public NirvanaConfigurationHelper ToLongRunningQueues()
        {
            NirvanaSetup.CommandMediationStrategy = MediationStrategy.ForwardLongRunningToQueue;
            return this;
        }

        public NirvanaConfigurationHelper ToWeb()
        {
            NirvanaSetup.CommandMediationStrategy = MediationStrategy.ForwardToWeb;
            return this;
        }

        public NirvanaConfigurationHelper FromQueues()
        {
            NirvanaSetup.RecieverMediationStrategy = MediationStrategy.ForwardToQueue;
            return this;
        }

        public NirvanaConfigurationHelper FromLongRunningQueues()
        {
            NirvanaSetup.RecieverMediationStrategy = MediationStrategy.ForwardLongRunningToQueue;
            return this;
        }

        public NirvanaConfigurationHelper ForQueries()
        {
            NirvanaSetup.ControllerTypes = new[] {ControllerType.Query};
            return this;
        }

        public NirvanaConfigurationHelper ForCommandAndQuery()
        {
            NirvanaSetup.ControllerTypes = new[] {ControllerType.Command, ControllerType.Query};
            return this;
        }

        public NirvanaConfigurationHelper ForNotifications(MediationStrategy strategy)
        {
            NirvanaSetup.UiNotificationMediationStrategy = strategy;
            NirvanaSetup.ControllerTypes = new[] {ControllerType.UiNotification};
            return this;
        }

        public NirvanaConfigurationHelper ForAllTypes()
        {
            NirvanaSetup.ControllerTypes = new[]
                {ControllerType.Command, ControllerType.Query, ControllerType.UiNotification};
            return this;
        }

        public NirvanaConfigurationHelper ForwardUiNotificationsToWeb()
        {
            NirvanaSetup.UiNotificationMediationStrategy = MediationStrategy.ForwardToWeb;
            return this;
        }

        public NirvanaConfigurationHelper ForControllerTypes(params ControllerType[] types)
        {
            NirvanaSetup.ControllerTypes = types;
            return this;
        }

        public void BuildConfiguration()
        {
            NirvanaSetup.TaskIdentifierProperty = "Identifier";
            NirvanaSetup.RootNames = EnumExtensions.GetAll(NirvanaSetup.RootType).SelectToArray(x => x.Value);
            NirvanaSetup.QueryTypes = NirvanaSetup.RootNames.ToDictionary(x => x, x => GetTypes(x, typeof(Query<>)));
            NirvanaSetup.CommandTypes = NirvanaSetup.RootNames.ToDictionary(x => x, x => GetTypes(x, typeof(Command<>)));
            NirvanaSetup.UiNotificationTypes = NirvanaSetup.RootNames.ToDictionary(x => x,
                x => GetTypes(x, typeof(UiEvent<>)));
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
}