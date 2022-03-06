using System;

namespace uPalette.Runtime.Foundation.TinyRx
{
    internal sealed class AnonymousDisposable : IDisposable
    {
        private readonly Action _dispose;

        public AnonymousDisposable(Action dispose)
        {
            _dispose = dispose;
        }

        public void Dispose()
        {
            _dispose?.Invoke();
        }
    }
}
