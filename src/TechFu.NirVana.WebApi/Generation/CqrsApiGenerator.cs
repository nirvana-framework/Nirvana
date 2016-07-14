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
using TechFu.Nirvana.Util.Extensions;

namespace TechFu.Nirvana.WebApi.Generation
{
    public class CqrsApiGenerator
    {
        private string[] _additionalAssemblies;
        private Func<string, object, bool> _attributeMatch;
        private Type _attributeType;
        private Type rootTypeType;


        private Dictionary<string, string> ControllerNames
        {
            get
            {
                var controllers = new Dictionary<string, string>();
                foreach (var kvp in EnumExtensions.GetAll(rootTypeType))
                {
                    controllers[kvp.Value] = $"{kvp.Value}Controller";
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
            var types = GetAllTypes(rootType);

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

        private IEnumerable<Type> GetAllTypes(string rootType)
        {
            var types = new List<Type>();

            if (NirvanaSetup.ControllerTypes.Contains(ControllerType.Command))
            {
                types.AddRange(ActionTypes(typeof(Command<>), rootType));
            }
            if (NirvanaSetup.ControllerTypes.Contains(ControllerType.Query))
            {
                types.AddRange(ActionTypes(typeof(Query<>), rootType));
            }
            if (NirvanaSetup.ControllerTypes.Contains(ControllerType.Notification))
            {
                types.AddRange(ActionTypes(typeof(Notification<>), rootType));
            }

            return types;
        }


        public static Type GetQueryResultType(Type queryType)
        {
            Type[] genericTypeArguments;
            return queryType.Closes(typeof(Query<>), out genericTypeArguments)
                ? genericTypeArguments[0]
                : null;
        }


        public Type[] ActionTypes(Type types, string rootType)
        {
            var allTypes = ObjectExtensions.AddAllTypesFromAssembliesContainingTheseSeedTypes(x => x.Closes(types),
                typeof(Command<>), rootTypeType);
            return
                allTypes.Where(x => MatchesRootType(rootType, x)).ToArray();
        }

        private bool MatchesRootType(string rootType, Type x)
        {
            var customAttribute = Attribute.GetCustomAttribute(x, _attributeType);
            return customAttribute != null && _attributeMatch(rootType, customAttribute);
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

            foreach (var additionalAssembly in _additionalAssemblies)
            {
                compilation =
                    compilation.AddReferences(MetadataReference.CreateFromFile($"{folder}\\{additionalAssembly}"));
            }

            compilation = compilation
                .AddActions();


            return compilation;
        }

        public CqrsApiGenerator Configure()
        {
            _attributeMatch = NirvanaSetup.AttributeMatchingFunction;
            rootTypeType = NirvanaSetup.RootType;
            _attributeType = NirvanaSetup.AggregateAttributeType;
            _additionalAssemblies = NirvanaSetup.AssemblyNameReferences;
            return this;
        }
    }

    public static class CompilationExtensions
    {
        public static CSharpCompilation AddActions(this CSharpCompilation input)
        {
            return input;
        }
    }
}