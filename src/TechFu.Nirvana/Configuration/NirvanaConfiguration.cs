using System;

namespace TechFu.Nirvana.Configuration
{
//    public abstract class NirvanaConfiguration
//    {
//        public abstract Type RootType { get; }
//        public abstract Type AggregateAttributeType { get; }
//        public abstract Func<string, object, bool> AttributeMatchingFunction { get; }
//        public abstract string[] AssemblyNameReferences { get; }
//        public abstract string[] AdditionalNamespaceReferences { get; }
//
//        public abstract object GetService(Type serviceType);
//    }

    public static class NirvanaSetup
    {
//        public static NirvanaConfiguration Configuration { get; private set; }

        public static Type RootType { get; internal set; }
        public static Type AggregateAttributeType { get; internal set; }
        public static  Func<string, object, bool> AttributeMatchingFunction { get; internal set; }
        public static  string[] AssemblyNameReferences { get; internal set; }
        public static ControllerType ControllerTypes { get; internal set; }
        public static Func<Type, Object> GetService { get; internal set; }
        public static bool CommandsToQueueEndpoint { get; set; }


        public static NirvanaConfigurationHelper Configure()
        {
            return new NirvanaConfigurationHelper();
        }

    }

    public enum ControllerType
    {
        Command,
        Query,
        CommandAndQuery
    }

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
            NirvanaSetup.ControllerTypes = ControllerType.Command;
            return this;
        }
        public NirvanaConfigurationHelper WithQueues()
        {
            NirvanaSetup.CommandsToQueueEndpoint = true;
            return this;
        }
        public NirvanaConfigurationHelper ForQuery()
        {
            NirvanaSetup.ControllerTypes = ControllerType.Query;
            return this;
        }
        public NirvanaConfigurationHelper ForCommandAndQuery()
        {
            NirvanaSetup.ControllerTypes = ControllerType.CommandAndQuery;
            return this;
        }

    }

}