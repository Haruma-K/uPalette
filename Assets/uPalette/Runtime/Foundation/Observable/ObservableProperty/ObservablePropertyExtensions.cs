namespace uPalette.Runtime.Foundation.Observable.ObservableProperty
{
    public static class ObservablePropertyExtensions
    {
        public static ReadOnlyObservableProperty<T> ToReadOnly<T> (this IObservableProperty<T> self)
        {
            return new ReadOnlyObservableProperty<T>(self);
        }
    }
}