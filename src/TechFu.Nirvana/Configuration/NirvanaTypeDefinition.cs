using System;

namespace TechFu.Nirvana.Configuration
{
    public class NirvanaTypeDefinition
    {
        public Type TaskType { get; set; }
        public Type NirvanaActionType { get; set; }
        public string UniqueName { get; set; }
        public string TypeCorrelationId { get; set; }

        public bool Matches(Type testType)
        {
            return testType == TaskType;
        }
    }
}