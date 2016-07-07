using System;

namespace TechFu.Nirvana.Configuration
{
    public abstract class NirvanaConfiguration
    {
        public abstract Type RootType { get; }
        public abstract Type AggregateAttributeType { get; }
        public abstract Func<string, object, bool> AttributeMatchingFunction { get; }
        public abstract string[] AssemblyNameReferences { get; }
        public abstract string[] AdditionalNamespaceReferences { get; }

        public abstract object GetService(Type serviceType);
    }

    public static class NirvanaConfigSettings
    {
        public static NirvanaConfiguration Configuration { get; private set; }

        public static void SetConfiguration(NirvanaConfiguration config)
        {
            Configuration = config;
        }
    }
}