using System;

namespace Nirvana.Domain
{
    public abstract class RootType
    {
        public abstract string RootName { get; }
    }


    public abstract class AggregateRootAttribute : Attribute
    {
        public string RootName => RootType.RootName;
        public RootType RootType { get; private set; }
        public string Identifier { get; private set; }
        public bool Public{ get; private set; }
        public bool Authorized { get; private set; } = false;
        public bool LongRunning { get; set; }

        public static string FilterName(string cqrsTypeName )
        {
            var q = "Query";
            if (cqrsTypeName.EndsWith(q))
            {
                return cqrsTypeName.Substring(0, cqrsTypeName.Length - q.Length);
            }
            q = "Command";
            if (cqrsTypeName.EndsWith(q))
            {
                return cqrsTypeName.Substring(0, cqrsTypeName.Length - q.Length);
            }
            q = "UiEvent";
            if (cqrsTypeName.EndsWith(q))
            {
                return cqrsTypeName.Substring(0, cqrsTypeName.Length - q.Length);
            }
            return q;
        }


        protected AggregateRootAttribute(RootType rootType, Type parameterType,bool isPublic=false, bool authorized=false,bool longRunning=false)
        {
            RootType = rootType;
            Identifier = FilterName(parameterType.Name);
            Public = isPublic;
            Authorized = authorized;
            LongRunning = longRunning;
        }

    }
}