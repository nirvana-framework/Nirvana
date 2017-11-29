using System;
using Nirvana.Domain;

namespace Nirvana.SampleApplication.Services.Services
{
    public class SampleServiceServiceRoot : ServiceRootType
    {
        public override string RootName => this.GetType().Name.Replace("ServiceRoot","");
    }

    public class SampleServiceRootAttribute : ServiceRootAttribute
    {
        
        public SampleServiceRootAttribute(Type identifier) : base(new SampleServiceServiceRoot(), identifier,false,false,false)
        {
        }

    }
}