using System;

namespace TechFu.Nirvana.WebApi.Sample.DomainSpecificData
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