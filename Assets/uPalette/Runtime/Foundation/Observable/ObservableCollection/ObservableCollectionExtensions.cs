namespace uPalette.Runtime.Foundation.Observable.ObservableCollection
{
    public static class ObservableCollectionExtensions
    {
        public static IReadOnlyObservableList<TValue> ToReadOnly<TValue> (this IObservableList<TValue> self)
        {
            return (IReadOnlyObservableList<TValue>)self;
        }
    }
}