using System;

namespace uPalette.Runtime.Foundation.TinyRx
{
    internal static class ConvertObservable
    {
        public static IObservable<TDst> Convert<TSrc, TDst>(this IObservable<TSrc> source, Func<TSrc, TDst> converter)
        {
            return new ConvertObservable<TSrc,TDst>(source, converter);
        }
    }
    
    internal class ConvertObservable<TSrc, TDst> : IObservable<TDst>
    {
        private readonly IObservable<TSrc> _source;
        private readonly Func<TSrc, TDst> _converter;
        
        public ConvertObservable(IObservable<TSrc> source, Func<TSrc, TDst> converter)
        {
            _source = source;
            _converter = converter;
        }

        public IDisposable Subscribe(IObserver<TDst> observer)
        {
            return _source.Subscribe(x =>
            {
                var dst = _converter.Invoke(x);
                observer.OnNext(dst);
            });
        }
    }
    
}
