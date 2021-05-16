using System;

namespace uPalette.Runtime.Foundation.Observable
{
    /// <summary>
    ///     Implementation of <see cref="IDisposable" /> that can be used anonymously.
    /// </summary>
    internal class Disposable : IDisposable
    {
        private bool _didDispose;
        private readonly Action _disposed;
        
        public Disposable(Action disposed)
        {
            _disposed = disposed;
        }

        public void Dispose()
        {
            if (_didDispose)
            {
                return;
            }
            _disposed?.Invoke();
            _didDispose = true;
        }
    }
}