using System;
using System.Reflection;
using Nirvana.Configuration;
using Should;
using TechFu.Nirvana.TestFramework;
using Xunit;

namespace TechFu.Nirvana.Tests.Configuration
{
    public abstract class NirvanaSetupTests
    {
        public class when_setting_SetRootTypeAssembly :
            BddTestBase<NirvanaConfigurationHelper, Assembly, object>
        {
            public override Action Inject => () => { };

            public override Action Because
                => () => { NirvanaSetup.Configure().SetRootTypeAssembly(typeof(Console).Assembly); };
            
            [Fact]
            public void should_set_root_type_assembly()
            {
              NirvanaSetup.RootTypeAssembly.FullName.ShouldEqual(typeof(Console).Assembly.FullName);
            }
        }
    }
}