using System;

namespace TechFu.Nirvana.Util
{
    public class DisposeAction : IDisposable
    {
        private readonly Action _disposeAction;

        public DisposeAction(Action disposeAction)
        {
            _disposeAction = disposeAction;
        }

        public static DisposeAction None => new DisposeAction(() => { });


        void IDisposable.Dispose()
        {
            _disposeAction();
        }
    }
}