using System;
using Nirvana.Domain;

namespace Nirvana.Tests.Configuration.SampleSetup
{
    public class TestRootAttribute : AggregateRootAttribute
    {
        
        public TestRootAttribute( Type parameterType, bool isPublic = false) : base(new TestRoot(), parameterType, isPublic)
        {
        }
    }
}