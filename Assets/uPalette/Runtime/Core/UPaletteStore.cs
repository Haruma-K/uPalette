using System;
using UnityEngine;
using uPalette.Runtime.Foundation.Observable.ObservableCollection;
using uPalette.Runtime.Foundation.Observable.ObservableProperty;

namespace uPalette.Runtime.Core
{
    [Serializable]
    public class UPaletteStore
    {
        [SerializeField] private ObservableList<ColorEntry> _entries = new ObservableList<ColorEntry>();

        public ObservableList<ColorEntry> Entries => _entries;
        public ObservableProperty<bool> IsDirty { get; } = new ObservableProperty<bool>();
    }
}