using System;
using System.Web;
using TechFu.Nirvana.EventStoreSample.Infrastructure.Threading;
using TechFu.Nirvana.Util;

namespace TechFu.Nirvana.EventStoreSample.Infrastructure.IoC
{
    public interface IUnitOfWorkStorage<T>
    {
        T Get();

        IDisposable Set(T value);
    }

    public class UnitOfWorkStorage<T> : IUnitOfWorkStorage<T>
    {
        private readonly CallContextUnitOfWorkStorage _callContextUnitOfWorkStorage;
        private readonly HttpContextUnitOfWorkStorage _httpContextUnitOfWorkStorage;

        public UnitOfWorkStorage(bool useLogicalCallContext = false)
        {
            _httpContextUnitOfWorkStorage = new HttpContextUnitOfWorkStorage();
            _callContextUnitOfWorkStorage = new CallContextUnitOfWorkStorage(useLogicalCallContext);
        }

        public T Get()
        {
            return _httpContextUnitOfWorkStorage.IsAvailable
                ? _httpContextUnitOfWorkStorage.Get()
                : _callContextUnitOfWorkStorage.Get();
        }

        public IDisposable Set(T value)
        {
            if (_httpContextUnitOfWorkStorage.IsAvailable)
            {
                return _httpContextUnitOfWorkStorage.Set(value);
            }
            return _callContextUnitOfWorkStorage.Set(value);
        }

        private class HttpContextUnitOfWorkStorage : IUnitOfWorkStorage<T>
        {
            private readonly string _key = Guid.NewGuid().ToString();
            public bool IsAvailable => HttpContext.Current != null;

            public T Get()
            {
                var httpContext = HttpContext.Current;
                if (!IsAvailable || !httpContext.Items.Contains(_key))
                {
                    return default(T);
                }

                return (T) httpContext.Items[_key];
            }

            public IDisposable Set(T value)
            {
                if (!IsAvailable)
                {
                    return DisposeAction.None;
                }
                if (HttpContext.Current.Items.Contains(_key))
                {
                    throw new InvalidOperationException("Cannot store more than one instance of a scoped object");
                }
                HttpContext.Current.Items[_key] = value;
                return new DisposeAction(() =>
                {
                    if (IsAvailable && HttpContext.Current.Items.Contains(_key))
                    {
                        HttpContext.Current.Items.Remove(_key);
                    }
                });
            }
        }

        private class CallContextUnitOfWorkStorage : IUnitOfWorkStorage<T>
        {
            private readonly SynchronizedStack<T> _instances;

            public CallContextUnitOfWorkStorage(bool useLogicalCallContext)
            {
                _instances = new SynchronizedStack<T>(useLogicalCallContext);
            }

            public T Get()
            {
                return _instances.GetTopValue();
            }

            public IDisposable Set(T value)
            {
                return _instances.PushValue(value);
            }
        }
    }
}