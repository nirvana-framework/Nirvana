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
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.CQRS.Util;
using TechFu.Nirvana.Util.Extensions;

namespace TechFu.Nirvana.WebApi.Generation
{
    public class CqrsApiGenerator
    {

        private Dictionary<string, string> ControllerNames
        {
            get
            {
                var controllers = new Dictionary<string, string>();
                foreach (var rootName in NirvanaSetup.RootNames)
                {
                    controllers[rootName] = $"{rootName}Controller";
                }

                return controllers;
            }
        }


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

        public string GetString(string apiControllerName)
        {
            var tree = BuildTree(typeof(Query<>), apiControllerName);
            return tree.GetRoot().ToFullString();
        }

        public SyntaxTree BuildTree(Type controllerType, string apiRootNamespace)
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
            foreach (var kvp in ControllerNames)
            {
                var actionCode = BuildActionCode(kvp.Key);
                codeNamespaces.AddRange(actionCode.Item2);
                builder.Append(actionCode.Item1);
            }

            codeNamespaces = codeNamespaces.Distinct().ToList();

            var code = new StringBuilder();
            codeNamespaces.ForEach(x => { code.Append($"using {x};"); });
            code.Append($"namespace {apiRootNamespace}.Controllers{{{builder}}}");

            var tree = CSharpSyntaxTree.ParseText(code.ToString());
            return tree;
        }

        private Tuple<string, List<string>> BuildActionCode(string rootType)
        {
            var types = CqrsUtils.GetAllTypes(rootType);

            var builder = new StringBuilder();
            var additionalNamespaces = new List<string>();
            
            builder.Append($"public class {rootType}Controller:TechFu.Nirvana.WebApi.Controllers.CommandQueryApiControllerBase{{");

            foreach (var type in types)
            {
                additionalNamespaces.Add(type.Namespace);
                var name = type.Name;
                if (type.Closes(typeof(Query<>)))
                {
                    name = name.Replace("Query", "");
                    builder.Append("[HttpGet]");
                    builder.Append($"public HttpResponseMessage {name}([FromUri] {name}Query query){{");
                    builder.Append("return Query(query);");
                    builder.Append("}");
                }
                if (type.Closes(typeof(Command<>)))
                {
                    name = name.Replace("Command", "");
                    builder.Append("[HttpPost]");
                    builder.Append($"public HttpResponseMessage {name}([FromBody] {name}Command command){{");
                    builder.Append("return Command(command);");
                    builder.Append("}");
                }
            }

            builder.Append("}");
            additionalNamespaces.Distinct().ToList();
            return new Tuple<string, List<string>>(builder.ToString(), additionalNamespaces.Distinct().ToList());
        }


        public void Compile(string assemblyName, string folder, string apiRootNamespace)
        {
            var compilation = BuildAssembly(assemblyName, folder, apiRootNamespace);

            compilation.Emit($"{folder}\\{assemblyName}");
        }

        public void LoadAssembly(string assemblyName, string folder, string apiRootNamespace)
        {
            var compilation = BuildAssembly(assemblyName, folder, apiRootNamespace);
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

        private CSharpCompilation BuildAssembly(string assemblyName, string folder, string apiRootNamespace)
        {
            var tree = BuildTree(typeof(Query<>), apiRootNamespace);
            var syntaxTrees = new[] {tree};
            var cSharpCompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create(assemblyName, syntaxTrees, options: cSharpCompilationOptions);

            compilation = compilation
                .AddReferences(GetGlobalReferences())
                .AddReferences(MetadataReference.CreateFromFile($"{folder}\\System.Web.Http.dll"))
                .AddReferences(MetadataReference.CreateFromFile($"{folder}\\System.Web.Http.Cors.dll"))
                .AddReferences(MetadataReference.CreateFromFile($"{folder}\\TechFu.Nirvana.dll"));

            foreach (var additionalAssembly in NirvanaSetup.AssemblyNameReferences)
            {
                compilation =
                    compilation.AddReferences(MetadataReference.CreateFromFile($"{folder}\\{additionalAssembly}"));
            }

      


            return compilation;
        }

       
    }
}