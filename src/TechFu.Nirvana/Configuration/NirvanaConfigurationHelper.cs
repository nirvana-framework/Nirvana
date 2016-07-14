using System;

namespace TechFu.Nirvana.Configuration
{
    public class NirvanaConfigurationHelper
    {
        public NirvanaConfigurationHelper SetRootType(Type rootType)
        {
            NirvanaSetup.RootType = rootType;
            return this;
        }
        public NirvanaConfigurationHelper SetDependencyResolver(Func<Type, Object> resolverMethod)
        {
            NirvanaSetup.GetService= resolverMethod;
            return this;
        }
        public NirvanaConfigurationHelper SetAggregateAttributeType(Type attributeType)
        {
            NirvanaSetup.AggregateAttributeType= attributeType;
            return this;
        }
        public NirvanaConfigurationHelper SetAttributeMatchingFunction(Func<string, object, bool> method)
        {
            NirvanaSetup.AttributeMatchingFunction= method;
            return this;
        }
        public NirvanaConfigurationHelper SetAdditionalAssemblyNameReferences(string[] refrences)
        {
            NirvanaSetup.AssemblyNameReferences = refrences;
            return this;
        }
        public NirvanaConfigurationHelper ForCommands()
        {
            NirvanaSetup.ControllerTypes =new [] { ControllerType.Command};
            return this;
        }
        public NirvanaConfigurationHelper ToQueues(QueueStrategy queueStrategy)
        {

            NirvanaSetup.QueueStrategy = queueStrategy;
            NirvanaSetup.CommandsToQueueEndpoint = true;
            return this;
        }
        public NirvanaConfigurationHelper FromQueues(QueueStrategy queueStrategy,WebMediationStrategy webMediationStrategy)
        {

            NirvanaSetup.WebMediationStrategy = webMediationStrategy;
            NirvanaSetup.QueueStrategy = queueStrategy;
            NirvanaSetup.CommandsToQueueEndpoint = true;
            return this;
        }
        public NirvanaConfigurationHelper ForQueries()
        {
            NirvanaSetup.ControllerTypes = new [] { ControllerType.Query};
            return this;
        }
        public NirvanaConfigurationHelper ForCommandAndQuery()
        {
            NirvanaSetup.ControllerTypes = new [] { ControllerType.Command,ControllerType.Query};
            return this;
        }
        public NirvanaConfigurationHelper ForForNotifications()
        {
            NirvanaSetup.ControllerTypes = new [] { ControllerType.Notification};
            return this;
        }
        public NirvanaConfigurationHelper ForAllTypes()
        {
            NirvanaSetup.ControllerTypes = new [] { ControllerType.Command, ControllerType.Query, ControllerType.Notification};
            return this;
        }

        public NirvanaConfigurationHelper ForCqrsTypes(params ControllerType[] types)
        {
            NirvanaSetup.ControllerTypes = types;
            return this;
        }

    }
}