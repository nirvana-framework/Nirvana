using System;
using System.IO;
using System.Reflection;
using Nirvana.Configuration;
using Nirvana.TestFramework;
using Should;
using Xunit;

namespace Nirvana.Tests.Configuration
{
    public abstract class NirvanaSetupTests
    {
        public class when_setting_SetRootTypeAssembly :
            BddTestBase<NirvanaConfigurationHelper, Assembly, NirvanaSetup>
        {
            public override Action Inject => () => { };
            public override Action Establish=> () =>
            {
                Input = typeof(Console).Assembly;
            };

            public override Action Because
                => () => { Result = Sut.SetRootTypeAssembly(Input).Setup; };
            
            [Fact]
            public void should_set_root_type_assembly()
            {
              Result.RootTypeAssembly.FullName.ShouldEqual(typeof(Console).Assembly.FullName);
            }
        }


        public class when_building_command_with_claims:
            BddTestBase<NirvanaConfigurationHelper, NirvanaTestInput, NirvanaSetup>
        {
            public override Action Inject => () => { };
            public override Action Establish=> () =>
            {
                Input = new NirvanaTestInput
                {
                    
                    Assembly = GetType().Assembly,
                    AssemblyNameReferences = new[] {"test 1","test 2"},
                    ControllerName = "Controller Assembly Name",
                    RootNamespace = "Root Namespace"
                };
            };

            public override Action Because
                => () => { Result = Sut
                .UsingControllerName(Input.ControllerName, Input.RootNamespace)
                .WithAssembliesFromFolder(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"))
                .SetAdditionalAssemblyNameReferences(Input.AssemblyNameReferences)
                .SetRootTypeAssembly(Input.Assembly)
                .SetAttributeMatchingFunction(Input.AttributeMatchingFunctionStub)
                .SetDependencyResolver(Input.GetService, Input.GetAllServices)
                .ForQueries(MediationStrategy.InProcess, MediationStrategy.InProcess)
                .ForCommands(MediationStrategy.InProcess, MediationStrategy.InProcess, MediationStrategy.None)
                .ForInternalEvents(MediationStrategy.InProcess, MediationStrategy.InProcess, MediationStrategy.None)
                .ForUiNotifications(MediationStrategy.None, MediationStrategy.None, MediationStrategy.None)
                .BuildConfiguration(); };
            
            [Fact]
            public void should_set_root_type_assembly()
            {
              Result.RootTypeAssembly.FullName.ShouldEqual(Input.Assembly.FullName);
            }
            [Fact]
            public void should_set_controller_assembly_name()
            {
              Result.ControllerAssemblyName.ShouldEqual("Controller Assembly Name");
            }
            [Fact]
            public void should_set_root_namespace()
            {
              Result.ControllerRootNamespace.ShouldEqual("Root Namespace");
            }
            [Fact]
            public void should_set_reference_assemblies()
            {
              Result.AssemblyNameReferences.ShouldContain("test 1");
              Result.AssemblyNameReferences.ShouldContain("test 2");
            }
            [Fact]
            public void should_set_commands()
            {
              Result.CommandTypes.Count.ShouldEqual(1);
            }
        }

        
    }

}