using System;
using System.Collections.Generic;

namespace Nirvana.Domain
{

    public abstract class ViewModel<T> : ViewModel
    {
        public Guid RootEntityKey { get; set; }
        public T Id { get; set; }

        protected bool Equals(ViewModel<T> other)
        {
            return Id.Equals(other.Id);
        }
    }

    public abstract class ViewModel
    {
    }

    public abstract class ViewModelBuilder<T>
    {
        private IDictionary<string, object> _inputs;

        public ViewModelBuilder<T> SetInputs(IDictionary<string, object> inputs)
        {
            _inputs = inputs;
            return this;
        }

        public abstract T Build();

        public TValue GetValue<TValue>(Enum key, TValue defaultValue = default(TValue))
        {
            return GetValue(key.ToString(), defaultValue);
        }

        public TValue GetValue<TValue>(string key,TValue defaultValue=default(TValue))
        {
            try
            {
                return (TValue) _inputs[key];
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}