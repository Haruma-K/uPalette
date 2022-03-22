using System;
using System.Text.RegularExpressions;
using UnityEngine;
using uPalette.Runtime.Foundation.TinyRx;
using uPalette.Runtime.Foundation.TinyRx.ObservableCollection;
using uPalette.Runtime.Foundation.TinyRx.ObservableProperty;

namespace uPalette.Runtime.Core.Model
{
    /// <summary>
    ///     <para>Represents a value in a theme.</para>
    ///     <para>This class is not made an abstract class due to serialization.</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Entry<T> : IDisposable
    {
        [SerializeField] private string _id;
        [SerializeField] private ObservableProperty<string> _name = new ObservableProperty<string>();

        [SerializeField] private ObservableDictionary<string, ObservableProperty<T>> _values =
            new ObservableDictionary<string, ObservableProperty<T>>();

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        internal Entry() : this(Guid.NewGuid().ToString())
        {
        }

        internal Entry(string entryId)
        {
            _id = entryId;
            _name.Value = GetDefaultName();
            _name.DisposeWith(_disposables);
        }

        public string Id => _id;
        public ObservableProperty<string> Name => _name;
        public IReadOnlyObservableDictionary<string, ObservableProperty<T>> Values => _values;

        public void Dispose()
        {
            _disposables.Dispose();
        }

        internal ObservableProperty<T> AddValue(string themeId, T value)
        {
            var prop = new ObservableProperty<T>(value);
            prop.DisposeWith(_disposables);
            _values.Add(themeId, prop);
            return prop;
        }

        internal bool RemoveValue(string themeId)
        {
            _values[themeId].Dispose();
            return _values.Remove(themeId);
        }

        public bool TryGetValue(string themeId, out IObservableProperty<T> value)
        {
            if (_values.TryGetValue(themeId, out var v))
            {
                value = v;
                return true;
            }

            value = null;
            return false;
        }

        private string GetDefaultName()
        {
            var name = $"New{typeof(T).Name}";
            name = Regex.Replace(name, "([^A-Z])([A-Z])", "$1 $2");
            return name;
        }
    }
}
