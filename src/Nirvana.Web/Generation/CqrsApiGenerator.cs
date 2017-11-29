using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Nirvana.Configuration;
using Nirvana.Util.Extensions;

namespace Nirvana.Web.Generation
{
    public class CqrsApiGenerator
    {

        private readonly NirvanaSetup _setup;

        public CqrsApiGenerator(NirvanaSetup setup)
        {
            _setup = setup;
        }


        private IEnumerable<MetadataReference> GetGlobalReferences(Assembly[] thirdPartyReferences)
        {
            var locations = thirdPartyReferences.Select(x => x.Location);

            var refs = locations.Select(l => MetadataReference.CreateFromFile(l));
            return refs.ToList();
        }


        public SyntaxTree BuildTree()
        {
            var builder = new StringBuilder();
            var codeNamespaces = new List<string>
            {
                "Microsoft.AspNetCore.Mvc",
                "Nirvana",
                "Nirvana.Mediation",
                "Nirvana.Web.Controllers"
            };
            //Commands and Queries
            foreach (var root in _setup.RootNames)
            {
                var actionCode = BuildCommandAndQueryController(root);
                if (actionCode.Actions != string.Empty)
                {

                    codeNamespaces.AddRange(actionCode.AdditionalNamespaces);
                    builder.Append(actionCode.Actions);
                }
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

        private SyntaxTree BuildSyntaxTree( List<string> codeNamespaces,
            StringBuilder builder)
        {
            var code = new StringBuilder();
            codeNamespaces.Distinct().ForEach(x => { code.Append($"using {x};"); });
            code.Append($"namespace {_setup.ControllerRootNamespace}.Controllers{{{builder}}}");
            var tree = CSharpSyntaxTree.ParseText(code.ToString());
            return tree;
        }

        private ControllerActionCode BuildEventHub()
        {
            if (!_setup.IsInProcess(TaskType.UiNotification,false))
            {
                return new ControllerActionCode {AdditionalNamespaces = new List<string>(),Actions = String.Empty};
            }
            var builder = new StringBuilder();
            var namespaces = new List<string>
            {
                "Nirvana.SignalRNotifications"
            };
            //For now push everything to the task channel
            var channelName = "Nirvana.CQRS.UiNotifications.Constants.TaskChannel";

            builder.Append($"[Route(\"UiNotifications\")]");
            builder.Append("public class UiNotificationsController: ApiControllerWithHub<EventHub>{");

            foreach (var key in _setup.UiNotificationTypes.Keys)
            {
                foreach (var x in _setup.UiNotificationTypes[key])
                {
                    var uiEventKey = $"{key}::{x.TaskType.Name}";
                    namespaces.Add(x.TaskType.Namespace);
                    builder.Append("[HttpPost]");
                    var uiEventName = x.TaskType.Name.Replace("UiEvent", "");
                    builder.Append($"public HttpResponseMessage {uiEventName}([FromBody]{x.TaskType.Name} uiEvent)");
                    builder.Append("{");
                    builder.Append($"PublishEvent(\"{uiEventKey}\", uiEvent, {channelName});");
                    builder.Append("return Request.CreateResponse(System.Net.HttpStatusCode.OK, new { });");
                    builder.Append("}");
                }
            }
            
            builder.Append("}");


            return new ControllerActionCode {Actions = builder.ToString(), AdditionalNamespaces = namespaces};
        }

        private ControllerActionCode BuildCommandAndQueryController(string rootType)
        {
            if (!_setup.CanProcess(TaskType.Query) && !_setup.CanProcess(TaskType.Command))
            {
                return new ControllerActionCode {Actions = string.Empty,AdditionalNamespaces = new List<string>()};
            }

            var builder = new StringBuilder();
            var additionalNamespaces = new List<string>();
            
            builder.Append($"[Route(\"api/{rootType}\")]");
            builder.Append($"public class {rootType}Controller:Nirvana.Web.Controllers.CommandQueryApiControllerBase{{");

            builder.Append($"public {rootType}Controller(Nirvana.Mediation.IMediatorFactory mediator,Nirvana.Util.Io.ISerializer serializer): base(mediator,serializer){{}}");

            if (_setup.CanProcess(TaskType.Query))
            {
                foreach (var type in _setup.QueryTypes[rootType])
                {
                    additionalNamespaces.Add(type.TaskType.Namespace);
                    var name = type.TaskType.Name;
                    name = name.Replace("Query", "");
                    if (type.RequiresAuthentication)
                    {
                        //TODO - throw claims in here
                        builder.Append($"[Authorize]");
                    
                    }
                    
                    builder.Append($"[HttpGet(\"{name}\")]");
//                    builder.Append($"[HttpGet]public HttpResponseMessage {name}([FromUri] {name}Query query){{return Query(query);}}");
                    builder.Append($"public Nirvana.CQRS.QueryResponse<{type.ReturnType.Name}> {name}({name}Query query){{return Query(query);}}");
                }
            }
            if (_setup.CanProcess(TaskType.Command))
            {
                foreach (var type in _setup.CommandTypes[rootType])
                {
                    additionalNamespaces.Add(type.TaskType.Namespace);
                    var name = type.TaskType.Name;
                    name = name.Replace("Command", "");
                    
                    builder.Append($"[HttpPost(\"{name}\")]");
//                    builder.Append($"[HttpPost]public HttpResponseMessage {name}([FromBody] {name}Command command){{return Command(command);}}");
                    builder.Append($"public Nirvana.CQRS.CommandResponse<{type.ReturnType.Name}> {name}({name}Command command){{return Command(command);}}");
                }
            }
            if (_setup.CanProcess(TaskType.InternalEvent))
            {
                foreach (var type in _setup.InternalEventTypes[rootType])
                {
                    additionalNamespaces.Add(type.TaskType.Namespace);
                    var name = type.TaskType.Name;
//                    builder.Append($"[HttpPost]public HttpResponseMessage {name}([FromBody] {name} internalEvent){{return InternalEvent(internalEvent);}}");

                    builder.Append($"[HttpPost(\"{name}\")]");
                    builder.Append($"public Nirvana.CQRS.InternalEventResponse {name}({name} internalEvent){{return InternalEvent(internalEvent);}}");
                }
            }
           

            builder.Append("}");
            additionalNamespaces.Distinct().ToList();
            return new ControllerActionCode
            {
                Actions = builder.ToString(),
                AdditionalNamespaces = additionalNamespaces.Distinct().ToList()
            };
        }

        public Assembly LoadAssembly(Assembly[] additionalReferences)
        {
            var compilation = BuildAssembly(additionalReferences.Union(new[]
            {
                typeof(object).Assembly,
                typeof(Stack<>).Assembly,
                typeof(HttpResponseMessage).Assembly
            }).ToArray());
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
                return Assembly.Load(memoryStream.GetBuffer());
            }
        }

        private CSharpCompilation BuildAssembly(Assembly[] thirdPartyReferences)
        {
            var tree = BuildTree();
            var syntaxTrees = new[] {tree};
            var cSharpCompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create(_setup.ControllerAssemblyName, syntaxTrees,
                options: cSharpCompilationOptions);



            var data = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES").ToString().Split(';');


            var aspnetCore = data.First(x => x.Contains("Microsoft.AspNetCore.Mvc.dll"));
            var cors = data.First(x => x.Contains("Microsoft.AspNetCore.Cors.dll"));
            var viewFeatures = data.First(x => x.Contains("Microsoft.AspNetCore.Mvc.ViewFeatures.dll"));
            var core = data.First(x => x.Contains("Microsoft.AspNetCore.Mvc.Core.dll"));
            var standard = data.First(x => x.Contains("netstandard.dll"));
            var runtime = data.First(x => x.Contains("System.Runtime.dll"));
            

            compilation = compilation
                    .AddReferences(GetGlobalReferences(thirdPartyReferences))
                    .AddReferences(MetadataReference.CreateFromFile(aspnetCore))
                    .AddReferences(MetadataReference.CreateFromFile(cors))
                    .AddReferences(MetadataReference.CreateFromFile(viewFeatures))
                    .AddReferences(MetadataReference.CreateFromFile(core))
                    .AddReferences(MetadataReference.CreateFromFile(standard))
                    .AddReferences(MetadataReference.CreateFromFile(runtime))
                //.AddReferences(MetadataReference.CreateFromFile($@"{AppContext.BaseDirectory}Nirvana.dll" ));
                ;

            foreach (var additionalAssembly in _setup.AssemblyNameReferences)
            {
                compilation =
                    compilation.AddReferences(
                        MetadataReference.CreateFromFile($@"{AppContext.BaseDirectory}\{additionalAssembly}"));
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