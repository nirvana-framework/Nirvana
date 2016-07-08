using System;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared
{
    public enum RootType
    {
        Infrastructure,
        Users
    }

    public class AggregateRootAttribute : Attribute
    {
        public AggregateRootAttribute(RootType rootType)
        {
            RootType = rootType;
        }

        public RootType RootType { get; private set; }
    }
}