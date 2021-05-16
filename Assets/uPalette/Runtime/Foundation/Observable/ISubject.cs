using System;

namespace uPalette.Runtime.Foundation.Observable
{
    internal interface ISubject<T> : IObserver<T>, IObservable<T>
    {
    }
}