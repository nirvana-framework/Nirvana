using System;

namespace TechFu.Nirvana.Configuration
{
    public abstract class NirvanaConfiguration
    {
        public abstract Type RootType { get;  }
        public abstract Type AggregateAttributeType { get;  }
        public abstract Func<string, object, bool> AttributeMatchingFunction{ get;  }
        public abstract string[] AssemblyNameReferences { get; }
    }
}