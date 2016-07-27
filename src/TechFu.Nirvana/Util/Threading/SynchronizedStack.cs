using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace TechFu.Nirvana.Util.Threading
{
    public class SynchronizedStack<T> : IEnumerable<T>
    {
        private readonly Func<string, object> _contextGetter;
        private readonly ConcurrentDictionary<string, Stack<T>> _contexts = new ConcurrentDictionary<string, Stack<T>>();
        private readonly Action<string, object> _contextSetter;
        private readonly string _key = Guid.NewGuid().ToString();

        public SynchronizedStack(bool useLogicalCallContext = false)
        {
            if (useLogicalCallContext)
            {
                _contextGetter = CallContext.LogicalGetData;
                _contextSetter = CallContext.LogicalSetData;
            }
            else
            {
                _contextGetter = CallContext.GetData;
                _contextSetter = CallContext.SetData;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var context = GetExecutionId();

            Stack<T> currentStack;

            return _contexts.TryGetValue(context, out currentStack)
                ? currentStack.GetEnumerator()
                : Enumerable.Empty<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T GetTopValue(T defaultValue = default(T))
        {
            return GetTopValue(() => defaultValue);
        }

        public T GetTopValue(Func<T> getDefaultValue)
        {
            var context = GetExecutionId();

            Stack<T> currentStack;

            return _contexts.TryGetValue(context, out currentStack) && currentStack.Count > 0
                ? currentStack.Peek()
                : getDefaultValue();
        }

        public bool HasValue()
        {
            var context = GetExecutionId();

            Stack<T> currentStack;

            return _contexts.TryGetValue(context, out currentStack) && currentStack.Count > 0;
        }

        public IDisposable PushValue(T value)
        {
            var context = GetExecutionId();

            var currentStack = _contexts.GetOrAdd(context, s => new Stack<T>());

            currentStack.Push(value);

            return new DisposeAction(() =>
            {
                currentStack.Pop();

                if (currentStack.Count == 0)
                    _contexts.TryRemove(context, out currentStack);
            });
        }

        private string GetExecutionId()
        {
            var executionId = _contextGetter(_key) as string;

            if (executionId != null)
                return executionId;

            executionId = Guid.NewGuid().ToString();

            _contextSetter(_key, executionId);

            return executionId;
        }
    }
}