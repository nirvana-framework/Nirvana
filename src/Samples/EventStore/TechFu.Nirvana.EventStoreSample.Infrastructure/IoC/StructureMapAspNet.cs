using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dependencies;

namespace TechFu.Nirvana.EventStoreSample.Infrastructure.IoC
{
    public class StructureMapAspNet
    {
        private StructureMapAspNet()
        {
        }

        public static StructureMapAspNet Configure(Assembly assembly = null)
        {
            IoC.Initialize(assembly);
            return new StructureMapAspNet();
        }

        public StructureMapAspNet ForWebApi()
        {
            GlobalConfiguration.Configuration.DependencyResolver = new StructureMapWebApiDependencyResolver();
            return this;
        }


        public static void DoBeginRequest()
        {
            InternalDependencyResolver.BeginScopedLifecycle();
        }

        public static void DoEndRequest()
        {
            InternalDependencyResolver.EndScopedLifecycle();
        }
    }

    public class StructureMapWebApiDependencyResolver : StructureMapMvcDependencyResolver, IDependencyResolver
    {
        public IDependencyScope BeginScope()
        {
            // Nested scope will likely already be created on Application_BeginRequest
            // So call the *IfNeeded version
            var scopeDisposer = InternalDependencyResolver.BeginScopedLifecycleIfNeeded();
            return new StructureMapDependencyScope(scopeDisposer);
        }

        public void Dispose() => InternalDependencyResolver.Dispose();

        private class StructureMapDependencyScope : StructureMapMvcDependencyResolver, IDependencyScope
        {
            private readonly IDisposable _disposable;

            public StructureMapDependencyScope(IDisposable disposable)
            {
                _disposable = disposable;
            }

            public void Dispose() => _disposable.Dispose();
        }
    }

    public class StructureMapMvcDependencyResolver : System.Web.Mvc.IDependencyResolver
    {
        public object GetService(Type serviceType) => InternalDependencyResolver.GetInstance(serviceType);

        public IEnumerable<object> GetServices(Type serviceType) => InternalDependencyResolver.GetAllInstances(serviceType);
    }
}