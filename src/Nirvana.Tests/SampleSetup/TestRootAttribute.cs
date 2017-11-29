using System;
using Nirvana.Domain;

namespace Nirvana.Tests.SampleSetup
{
    public class TestRootAttribute : ServiceRootAttribute
    {
        
        public TestRootAttribute( Type parameterType, bool isPublic = false) : base(new TestRoot(), parameterType, isPublic)
        {
        }
    }
}