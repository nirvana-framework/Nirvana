using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Nirvana.Configuration;
using Nirvana.Mediation;
using Nirvana.TestFramework;
using Nirvana.Tests.SampleSetup;
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
                .ForUiNotifications(MediationStrategy.InProcess, MediationStrategy.InProcess, MediationStrategy.None)
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

        public class when_forwarding_long_runnning_only:
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
                .ForCommands(MediationStrategy.ForwardLongRunningToQueue, MediationStrategy.ForwardLongRunningToQueue, MediationStrategy.None)
                
                .ForQueries(MediationStrategy.InProcess, MediationStrategy.InProcess)
                .ForInternalEvents(MediationStrategy.InProcess, MediationStrategy.InProcess, MediationStrategy.None)
                .ForUiNotifications(MediationStrategy.InProcess, MediationStrategy.InProcess, MediationStrategy.None)
                
                .BuildConfiguration(); };
            
          
            [Fact]
            public void should_set_commands()
            {
                var key = Result.CommandTypes.Keys.First();

                Result.CommandTypes[key].First(x=>x.TaskType==typeof(TestLongRunningCommand)).LongRunning.ShouldBeTrue();

                Result.ShouldForwardToQueue(TaskType.Command,false, typeof(TestLongRunningCommand)).ShouldBeTrue();
                Result.ShouldForwardToQueue(TaskType.Command,true, typeof(TestLongRunningCommand)).ShouldBeTrue();

                var m = new MediatorFactory(Result);
                m.GetMediatorStrategy(typeof(TestLongRunningCommand),false).ShouldEqual(MediatorStrategy.ForwardToQueue);
                m.GetMediatorStrategy(typeof(TestLongRunningCommand),true).ShouldEqual(MediatorStrategy.ForwardToQueue);
                
                
                m.GetMediatorStrategy(typeof(TestCommand),false).ShouldEqual(MediatorStrategy.HandleInProc);
                m.GetMediatorStrategy(typeof(TestCommand),true).ShouldEqual(MediatorStrategy.HandleInProc);
                
            }
            [Fact]
            public void should_set_query()
            {
                var query = Result.QueryTypes["Test"].First(x=>x.TaskType==typeof(TestQuery));
                query.LongRunning.ShouldBeTrue();

                query.ChildAction.ShouldEqual(MediationStrategy.InProcess);
                query.TopLevelAction.ShouldEqual(MediationStrategy.InProcess);

                
                
            }
            [Fact]
            public void should_set_events()
            {
                var query = Result.InternalEventTypes["Test"].First(x=>x.TaskType==typeof(TestEvent));
                query.LongRunning.ShouldBeTrue();

                query.ChildAction.ShouldEqual(MediationStrategy.InProcess);
                query.TopLevelAction.ShouldEqual(MediationStrategy.InProcess);

                
                
            }
            [Fact]
            public void should_set_UI_notifications()
            {
                var query = Result.UiNotificationTypes["Test"].First(x=>x.TaskType==typeof(TestUiNotification));
                query.LongRunning.ShouldBeTrue();

                query.ChildAction.ShouldEqual(MediationStrategy.InProcess);
                query.TopLevelAction.ShouldEqual(MediationStrategy.InProcess);

                
                
            }
            
        }

        public class when_reading_from_queue_and_forwarding:
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
                .WithAssembliesFromFolder(AppDomain.CurrentDomain.BaseDirectory)
                .SetAdditionalAssemblyNameReferences(Input.AssemblyNameReferences)
                .SetRootTypeAssembly(Input.Assembly)
                .SetDependencyResolver(Input.GetService, Input.GetAllServices)
                .ForCommands(MediationStrategy.ForwardToWeb, MediationStrategy.ForwardToWeb, MediationStrategy.None)
                .BuildConfiguration(); };
            
          
            [Fact]
            public void should_set_commands()
            {
                var key = Result.CommandTypes.Keys.First();

                Result.CommandTypes[key].First(x=>x.TaskType==typeof(TestLongRunningCommand)).LongRunning.ShouldBeTrue();

                Result.ShouldForwardToWeb(TaskType.Command,false).ShouldBeTrue();
                Result.ShouldForwardToWeb(TaskType.Command,true).ShouldBeTrue();

                var m = new MediatorFactory(Result);
                m.GetMediatorStrategy(typeof(TestLongRunningCommand),false).ShouldEqual(MediatorStrategy.ForwardToWeb);
                m.GetMediatorStrategy(typeof(TestLongRunningCommand),true).ShouldEqual(MediatorStrategy.ForwardToWeb);
                
                
            }
            [Fact]
            public void should_set_query()
            {
                var query = Result.QueryTypes["Test"].First(x=>x.TaskType==typeof(TestQuery));
                query.LongRunning.ShouldBeTrue();

                query.ChildAction.ShouldEqual(MediationStrategy.InProcess);
                query.TopLevelAction.ShouldEqual(MediationStrategy.InProcess);

                
                
            }
            [Fact]
            public void should_set_events()
            {
                var query = Result.InternalEventTypes["Test"].First(x=>x.TaskType==typeof(TestEvent));
                query.LongRunning.ShouldBeTrue();

                query.ChildAction.ShouldEqual(MediationStrategy.InProcess);
                query.TopLevelAction.ShouldEqual(MediationStrategy.InProcess);

                
                
            }
            [Fact]
            public void should_set_UI_notifications()
            {
                var query = Result.UiNotificationTypes["Test"].First(x=>x.TaskType==typeof(TestUiNotification));
                query.LongRunning.ShouldBeTrue();

                query.ChildAction.ShouldEqual(MediationStrategy.InProcess);
                query.TopLevelAction.ShouldEqual(MediationStrategy.InProcess);

                
                
            }
            
        }

        
    }

}