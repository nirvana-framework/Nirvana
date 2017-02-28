using System;
using System.Collections.Generic;
using Nirvana.Security;

namespace Nirvana.Configuration
{



    public class NirvanaTypeRoutingDefinition
    {

        public MediationStrategy ReceiverMediationStrategy { get; set; }
        public MediationStrategy MediationStrategy { get; set; }
        public MediationStrategy ChildMediationStrategy { get; set; }
        public bool CanHandle { get; set; }
        public TaskType NirvanaTaskType { get; set; }

        public NirvanaTaskInformation[] Tasks { get; set; }
    }

    public class NirvanaTaskInformation
    {
        public string RootName { get; set; }
        public Type TaskType { get; set; }
        public Dictionary<ClaimType, AccessType[]> Claims { get; set; }
        public string UniqueName { get; set; }
        public string TypeCorrelationId { get; set; }
        public bool RequiresAuthentication { get; set; }

        public bool Matches(Type testType)
        {
            return testType == TaskType;
        }
    }
}