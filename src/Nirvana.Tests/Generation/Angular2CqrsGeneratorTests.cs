using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using Nirvana.Configuration;
using Nirvana.Security;
using Nirvana.TestFramework;
using Nirvana.Tests.Configuration;
using Nirvana.Util;
using Should;
using Xunit;

namespace Nirvana.Tests.Generation
{
    public abstract class Angular2CqrsGeneratorTests : BddTestBase<Angular2CqrsGenerator, NirvanaTestInput, string>
    {
        protected NirvanaSetup Setup;

        public override Action Because => () =>
        {
            Result = Sut.GetV2Items();
        };

        public class when_sending_byte_array: Angular2CqrsGeneratorTests
        {
            public override Action Inject => () =>
            {
                Input = new NirvanaTestInput
                {
                    RootNamespace = "namespace",
                    AssemblyNameReferences = new string[0],
                    ControllerName = "controllerName",
                    Assembly = this.GetType().Assembly
                };


                Setup = NirvanaSetup.Configure().UsingControllerName(Input.ControllerName, Input.RootNamespace)
                .WithAssembliesFromFolder(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"))
                .SetAdditionalAssemblyNameReferences(Input.AssemblyNameReferences)
                .SetRootTypeAssembly(Input.Assembly)
                .SetAttributeMatchingFunction(Input.AttributeMatchingFunctionStub)
                .SetDependencyResolver(Input.GetService, Input.GetAllServices)
                .ForQueries(MediationStrategy.InProcess, MediationStrategy.InProcess)
                .ForCommands(MediationStrategy.InProcess, MediationStrategy.InProcess, MediationStrategy.None)
                .ForInternalEvents(MediationStrategy.InProcess, MediationStrategy.InProcess, MediationStrategy.None)
                .ForUiNotifications(MediationStrategy.InProcess, MediationStrategy.InProcess, MediationStrategy.None)
                .BuildConfiguration();




                DependsOnConcrete(Setup);
            };
            [Fact]
            public void should_show_as_any()
            {
                var path = $"{GetFolderPath("")}\\byteArrayTest.txt";
                Result.Trim().ShouldEqual(File.ReadAllText(path).Trim());
            }
        }
    }
}