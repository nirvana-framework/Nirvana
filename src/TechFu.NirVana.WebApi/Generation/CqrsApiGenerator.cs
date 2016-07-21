using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.CQRS.Util;
using TechFu.Nirvana.Util.Extensions;

namespace TechFu.Nirvana.WebApi.Generation
{
    public class CqrsApiGenerator
    {
        private IEnumerable<MetadataReference> GetGlobalReferences()
        {
            var assemblies = new[]
            {
                typeof(object).Assembly,
                typeof(HttpCachePolicy).Assembly,
                typeof(HttpResponseMessage).Assembly
            };
            var refs = from a in assemblies
                select MetadataReference.CreateFromFile(a.Location);
            return refs.ToList();
        }


        public SyntaxTree BuildTree()
        {
            var builder = new StringBuilder();
            var codeNamespaces = new List<string>
            {
                "System.Net.Http",
                "System.Web.Http",
                "TechFu.Nirvana",
                "TechFu.Nirvana.WebApi",
                "TechFu.Nirvana.WebApi.Controllers"
            };
            //Commands and Queries
            foreach (var root in NirvanaSetup.RootNames)
            {
                var actionCode = BuildActionCode(root);
                codeNamespaces.AddRange(actionCode.AdditionalNamespaces);
                builder.Append(actionCode.Actions);
            }
            //UI Notifications 
            var eventHub = BuildEventHub();
            if (eventHub.Actions!=string.Empty)
            {
                codeNamespaces.AddRange(eventHub.AdditionalNamespaces);
                builder.Append(eventHub.Actions);
            }

            return BuildSyntaxTree( codeNamespaces, builder);
        }

        private static SyntaxTree BuildSyntaxTree( List<string> codeNamespaces,
            StringBuilder builder)
        {
            var code = new StringBuilder();
            codeNamespaces.Distinct().ForEach(x => { code.Append($"using {x};"); });
            code.Append($"namespace {NirvanaSetup.ControllerRootNamespace}.Controllers{{{builder}}}");
            var tree = CSharpSyntaxTree.ParseText(code.ToString());
            return tree;
        }

        private ControllerActionCode BuildEventHub()
        {
            if (NirvanaSetup.UiNotificationMediationStrategy != MediationStrategy.InProcess)
            {
                return new ControllerActionCode {AdditionalNamespaces = new List<string>(),Actions = String.Empty};
            }
            var builder = new StringBuilder();
            var namespaces = new List<string>
            {
                "TechFu.Nirvana.SignalRNotifications"
            };
            //For now push everything to the task channel
            var channelName = "Constants.TaskChannel";

            builder.Append("public class UiNotificationController: ApiControllerWithHub<EventHub>{");

            //                 [HttpPost]
            //                public HttpResponseMessage TestUiEvent([FromBody]TestUiEvent testUiEvent)
            //                {
            //                    PublishEvent("TestUiEvent", testUiEvent, Constants.TaskChannel);
            //                    return Request.CreateResponse(HttpStatusCode.OK, new { });
            //                }

            foreach (var key in NirvanaSetup.UiNotificationTypes.Keys)
            {
                foreach (var x in NirvanaSetup.UiNotificationTypes[key])
                {
                    var uiEventKey = $"{key}::{x.Name}";
                    namespaces.Add(x.Namespace);
                    builder.Append("[HttpPost]");
                    builder.Append($"public HttpResponseMessage {x.Name.Replace("UiEvent", "")}([FromBody]{x.Name} uiEvent)");
                    builder.Append("{");
                    builder.Append($"PublishEvent({uiEventKey}, uiEvent, {channelName});");
                    builder.Append("return Request.CreateResponse(HttpStatusCode.OK, new { });");
                    builder.Append("}");
                }
            }


            NirvanaSetup.UiNotificationTypes.SelectMany(x => x.Value).ForEach(x =>
            {

               


            });
            builder.Append("}");


            return new ControllerActionCode {Actions = builder.ToString(), AdditionalNamespaces = namespaces};
        }

        private ControllerActionCode BuildActionCode(string rootType)
        {

            var builder = new StringBuilder();
            var additionalNamespaces = new List<string>();

            builder.Append(
                $"public class {rootType}Controller:TechFu.Nirvana.WebApi.Controllers.CommandQueryApiControllerBase{{");

            foreach (var type in NirvanaSetup.QueryTypes[rootType])
            {
                additionalNamespaces.Add(type.Namespace);
                var name = type.Name;

                name = name.Replace("Query", "");
                builder.Append("[HttpGet]");
                builder.Append($"public HttpResponseMessage {name}([FromUri] {name}Query query){{");
                builder.Append("return Query(query);");
                builder.Append("}");
            }
            foreach (var type in NirvanaSetup.CommandTypes[rootType])
            {
                additionalNamespaces.Add(type.Namespace);
                var name = type.Name;
                name = name.Replace("Command", "");
                builder.Append("[HttpPost]");
                builder.Append($"public HttpResponseMessage {name}([FromBody] {name}Command command){{");
                builder.Append("return Command(command);");
                builder.Append("}");
            }

            builder.Append("}");
            additionalNamespaces.Distinct().ToList();
            return new ControllerActionCode
            {
                Actions = builder.ToString(),
                AdditionalNamespaces = additionalNamespaces.Distinct().ToList()
            };
        }

        public void LoadAssembly()
        {
            var compilation = BuildAssembly();
            using (var memoryStream = new MemoryStream())
            {
                var result = compilation.Emit(memoryStream);
                if (!result.Success)
                {
                    var failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    var failuresException = "Failed to compile code generation project : \r\n";

                    foreach (var diagnostic in failures)
                    {
                        failuresException += $"{diagnostic.Id} : {diagnostic.GetMessage()}\r\n";
                    }

                    throw new Exception(failuresException);
                }
                memoryStream.Flush();
                Assembly.Load(memoryStream.GetBuffer());
            }
        }

        private CSharpCompilation BuildAssembly()
        {
            var tree = BuildTree();
            var syntaxTrees = new[] {tree};
            var cSharpCompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create(NirvanaSetup.ControllerAssemblyName, syntaxTrees,
                options: cSharpCompilationOptions);

            compilation = compilation
                .AddReferences(GetGlobalReferences())
                .AddReferences(MetadataReference.CreateFromFile($"{NirvanaSetup.AssemblyFolder}\\System.Web.Http.dll"))
                .AddReferences(
                    MetadataReference.CreateFromFile($"{NirvanaSetup.AssemblyFolder}\\System.Web.Http.Cors.dll"))
                .AddReferences(MetadataReference.CreateFromFile($"{NirvanaSetup.AssemblyFolder}\\TechFu.Nirvana.dll"));

            foreach (var additionalAssembly in NirvanaSetup.AssemblyNameReferences)
            {
                compilation =
                    compilation.AddReferences(
                        MetadataReference.CreateFromFile($"{NirvanaSetup.AssemblyFolder}\\{additionalAssembly}"));
            }


            return compilation;
        }
    }

    internal class ControllerActionCode
    {
        public string Actions { get; set; }
        public List<string> AdditionalNamespaces { get; set; }
    }
}