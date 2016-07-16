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

        public NirvanaConfigurationHelper HandleInProc()
        {
            NirvanaSetup.CommandMediationStrategy = MediationStrategy.InProcess;
            return this;

        }

        public NirvanaConfigurationHelper ToQueues()
        {
            NirvanaSetup.CommandMediationStrategy  = MediationStrategy.ForwardToQueue;
            return this;
        }
        public NirvanaConfigurationHelper ToLongRunningQueues()
        {
            NirvanaSetup.CommandMediationStrategy  = MediationStrategy.ForwardLongRunningToQueue;
            return this;
        }
        public NirvanaConfigurationHelper ToWeb()
        {
            NirvanaSetup.CommandMediationStrategy  = MediationStrategy.ForwardToWeb;
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

        public void BuildConfiguration()
        {
            NirvanaSetup.RootNames = EnumExtensions.GetAll(NirvanaSetup.RootType).SelectToArray(x => x.Value);
            NirvanaSetup.QueryTypes= NirvanaSetup.RootNames.ToDictionary(x => x, x => CqrsUtils.ActionTypes(typeof(Query<>),x).ToArray());
            NirvanaSetup.CommandTypes= NirvanaSetup.RootNames.ToDictionary(x => x, x => CqrsUtils.ActionTypes(typeof(Command<>),x).ToArray());
            NirvanaSetup.UiNotificationTypes= NirvanaSetup.RootNames.ToDictionary(x => x, x => CqrsUtils.ActionTypes(typeof(UINotification<>),x).ToArray());


        }
    }
}