using System;
using System.Collections.Generic;
using System.Linq;
using StructureMap;
using TechFu.Nirvana.Util;

namespace TechFu.Nirvana.EventStoreSample.Infrastructure.IoC
{
    public static class InternalDependencyResolver
    {
        private static IContainer _defaultContainer;
        private static IUnitOfWorkStorage<UnitOfWorkContainer> _unitOfWorkStorage;

        public static void SetRootContainer(IContainer container)
        {
            _defaultContainer = container;
            // For the command processor, logical call context ends up flowing
            // to other threads. CallContext stays scoped to the executing task.
            _unitOfWorkStorage = new UnitOfWorkStorage<UnitOfWorkContainer>(useLogicalCallContext: false);
        }

        /// <summary>
        /// Used for TESTING purposes only.
        /// Tells the resolver to use the provided container for the current scope.
        /// This will allow a greater nested container depth since we have tests
        /// that do setup with multiple containers.
        /// </summary>
        public static IDisposable PushTestContainer(IContainer container)
        {
            // In a test context, use LogicalCallContext
            if (_unitOfWorkStorage == null)
            {
                _unitOfWorkStorage = new UnitOfWorkStorage<UnitOfWorkContainer>(useLogicalCallContext: true);
            }
            return SetUnitOfWorkContainer(container);
        }

        private static UnitOfWorkContainer SetUnitOfWorkContainer(IContainer value)
        {
            var unitOfWorkInstance = new UnitOfWorkContainer(value);
            var disposer = _unitOfWorkStorage.Set(unitOfWorkInstance);
            unitOfWorkInstance.UnitOfWorkDisposer = disposer;
            return unitOfWorkInstance;
        }

        private static UnitOfWorkContainer GetCurrentUnitOfWorkContainer()
        {
            return _unitOfWorkStorage?.Get();
        }

        public static bool HasNestedContainer => GetCurrentUnitOfWorkContainer()?.Container != null;

        public static IContainer GetContainer()
        {
            return (GetCurrentUnitOfWorkContainer()?.Container ?? _defaultContainer);
        }

        public static IContainer GetRootContainer() => _defaultContainer ?? GetCurrentUnitOfWorkContainer()?.Container;

        public static IDisposable BeginScopedLifecycleIfNeeded(Guid? correlationId = null)
        {
            return HasNestedContainer
                ? DisposeAction.None
                : BeginScopedLifecycle(correlationId);
        }

        public static IDisposable BeginScopedLifecycle(Guid? correlationId = null)
        {
            var unitOfWorkContainer = GetCurrentUnitOfWorkContainer();
            if (unitOfWorkContainer == null && _defaultContainer != null)
            {
                unitOfWorkContainer = SetUnitOfWorkContainer(_defaultContainer.GetNestedContainer());
                if (correlationId.HasValue)
                {
//                    var logContext = unitOfWorkContainer.Container.GetInstance<ILogContext>();
//                    logContext.CorrelationId = correlationId.Value;
                }
                return unitOfWorkContainer;
            }
            
            return DisposeAction.None;
        }

        public static void Dispose()
        {
            EndScopedLifecycle();
        }

        public static void EndScopedLifecycle()
        {
            var unitOfWorkContainer = GetCurrentUnitOfWorkContainer();
            ((IDisposable) unitOfWorkContainer)?.Dispose();
        }

        public static object GetInstance(Type type)
        {
            if (type == null)
            {
                return null;
            }
            return type.IsAbstract || type.IsInterface
                ? GetContainer().TryGetInstance(type)
                : GetContainer().GetInstance(type);
        }

        public static T GetInstance<T>()
        {
            var type = typeof(T);
            return type.IsAbstract || type.IsInterface
                ? GetContainer().TryGetInstance<T>()
                : GetContainer().GetInstance<T>();
        }

        public static T GetNamedInstance<T>(string name)
        {
            var type = typeof(T);
            return type.IsAbstract || type.IsInterface
                ? GetContainer().TryGetInstance<T>(name)
                : GetContainer().GetInstance<T>(name);
        }

        public static IEnumerable<object> GetAllInstances(Type type)
        {
            return GetContainer().GetAllInstances(type).Cast<object>();
        }

        public static IEnumerable<T> GetAllInstances<T>()
        {
            return GetContainer().GetAllInstances<T>();
        }

        public static object TryGetInstance(Type type)
        {
            var container = GetContainer();

            if (container == null)
                return null;

            try
            {
                return container.TryGetInstance(type);
            }
            catch
            {
                return null;
            }
        }

        public static T TryGetInstance<T>()
        {
            return (T) TryGetInstance(typeof(T));
        }

        private class UnitOfWorkContainer : IDisposable
        {
            public UnitOfWorkContainer(IContainer container)
            {
                Container = container;
            }

            public IContainer Container { get; }
            public IDisposable UnitOfWorkDisposer { get; set; }

            void IDisposable.Dispose()
            {
                Container.Dispose();
                UnitOfWorkDisposer.Dispose();
            }
        }
    }

}