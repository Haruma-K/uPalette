using System;
using UnityEngine;
using uPalette.Runtime.Foundation.TinyRx.ObservableProperty;

namespace uPalette.Runtime.Core.Model
{
    /// <summary>
    ///     Represents a theme in a palette.
    /// </summary>
    [Serializable]
    public sealed class Theme : IDisposable
    {
        private const string DefaultName = "New Theme";

        [SerializeField] private string _id;
        [SerializeField] private ObservableProperty<string> _name;

        internal Theme() : this(Guid.NewGuid().ToString())
        {
        }

        internal Theme(string id)
        {
            _id = id;
            _name = new ObservableProperty<string>(DefaultName);
        }

        public string Id => _id;
        public ObservableProperty<string> Name => _name;

        public void Dispose()
        {
            _name?.Dispose();
        }
    }
}
