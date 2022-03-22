using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace uPalette.Runtime.Foundation.TinyRx.ObservableCollection
{
    /// <summary>
    ///     The list that can be observed changes of the items.
    /// </summary>
    /// <typeparam name="T">Type of items.</typeparam>
    [Serializable]
    public sealed class ObservableList<T> : IObservableList<T>, IReadOnlyObservableList<T>, IDisposable
    {
        [SerializeField] private List<T> _internalList = new List<T>();
        
        private readonly Subject<ListAddEvent<T>> _subjectAdd = new Subject<ListAddEvent<T>>();
        private readonly Subject<Empty> _subjectClear = new Subject<Empty>();
        private readonly Subject<ListRemoveEvent<T>> _subjectRemove = new Subject<ListRemoveEvent<T>>();
        private readonly Subject<ListReplaceEvent<T>> _subjectReplace = new Subject<ListReplaceEvent<T>>();

        private bool _didDispose;

        public ObservableList()
        {
        }

        public ObservableList(IEnumerable<T> items)
        {
            _internalList.AddRange(items);
        }

        public IObservable<ListAddEvent<T>> ObservableAdd => _subjectAdd;

        public IObservable<ListRemoveEvent<T>> ObservableRemove => _subjectRemove;

        public IObservable<Empty> ObservableClear => _subjectClear;

        public IObservable<ListReplaceEvent<T>> ObservableReplace => _subjectReplace;

        public void Dispose()
        {
            DisposeSubject(_subjectAdd);
            DisposeSubject(_subjectRemove);
            DisposeSubject(_subjectClear);
            DisposeSubject(_subjectReplace);

            _didDispose = true;
        }

        public bool IsReadOnly => false;

        public T this[int index]
        {
            get => _internalList[index];
            set
            {
                var oldValue = _internalList[index];
                if (Equals(oldValue, value))
                    return;

                _internalList[index] = value;
                _subjectReplace?.OnNext(new ListReplaceEvent<T>(index, oldValue, value));
            }
        }

        public void Add(T item)
        {
            Assert.IsFalse(_didDispose);
            Assert.IsFalse(item == null);

            _internalList.Add(item);
            var index = _internalList.Count - 1;
            _subjectAdd.OnNext(new ListAddEvent<T>(index, item));
        }

        public void Insert(int index, T item)
        {
            Assert.IsFalse(_didDispose);
            Assert.IsTrue(index >= 0);
            Assert.IsFalse(item == null);

            _internalList.Insert(index, item);
            _subjectAdd.OnNext(new ListAddEvent<T>(index, item));
        }

        public bool Remove(T item)
        {
            Assert.IsFalse(_didDispose);
            Assert.IsFalse(item == null);

            var index = _internalList.IndexOf(item);
            if (index < 0)
                return false;

            _internalList.RemoveAt(index);
            _subjectRemove.OnNext(new ListRemoveEvent<T>(index, item));
            return true;
        }

        public void RemoveAt(int index)
        {
            Assert.IsFalse(_didDispose);
            Assert.IsTrue(index >= 0);

            var item = _internalList[index];
            _internalList.RemoveAt(index);
            _subjectRemove.OnNext(new ListRemoveEvent<T>(index, item));
        }

        public void Clear()
        {
            Assert.IsFalse(_didDispose);

            _internalList.Clear();
            _subjectClear.OnNext(Empty.Default);
        }

        public int Count => _internalList.Count;

        public bool Contains(T item)
        {
            Assert.IsFalse(_didDispose);

            return _internalList.Contains(item);
        }

        public int IndexOf(T item)
        {
            Assert.IsFalse(_didDispose);

            return _internalList.IndexOf(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Assert.IsFalse(_didDispose);
            Assert.IsNotNull(array);

            _internalList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        private static void DisposeSubject<TSubjectValue>(Subject<TSubjectValue> subject)
        {
            if (subject.DidDispose) return;
            if (subject.DidTerminate)
            {
                subject.Dispose();
                return;
            }

            subject.OnCompleted();
            subject.Dispose();
        }
    }
}
