using System;

namespace uPalette.Runtime.Foundation.TinyRx
{
    internal interface ISubject<T> : IObserver<T>, IObservable<T>
    {
    }
}