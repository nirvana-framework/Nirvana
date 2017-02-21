using System;
using System.Reflection;
using Nirvana.Domain;

namespace Nirvana.Tests.Configuration
{
    public class NirvanaTestInput
    {
        public Assembly Assembly { get; set; }
        public string ControllerName { get; set; }
        public string RootNamespace { get; set; }
        public string[] AssemblyNameReferences { get; set; }


        public Func<string, object, bool> AttributeMatchingFunctionStub
          => (x, y) => x == ((AggregateRootAttribute)y).RootName;

       

        public object GetService(Type arg)
        {
            return null;
        }

        public object[] GetAllServices(Type arg)
        {
            return new object[0];
        }
    }
}