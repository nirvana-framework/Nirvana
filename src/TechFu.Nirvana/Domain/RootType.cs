using System;

namespace TechFu.Nirvana.Domain
{
    public abstract class RootType
    {
    }


    public abstract class AggregateRootAttribute : Attribute
    {
        public abstract string RootName { get; }


        public RootType RootType { get; private set; }
        public string Identifier { get; private set; }


        protected AggregateRootAttribute(RootType rootType, string identifier)
        {
            RootType = rootType;
            Identifier = identifier;
        }
    }
}