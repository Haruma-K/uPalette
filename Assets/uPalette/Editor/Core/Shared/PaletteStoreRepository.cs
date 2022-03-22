using System;
using UnityEditor;
using uPalette.Runtime.Core;
using uPalette.Runtime.Foundation.TinyRx.ObservableProperty;

namespace uPalette.Editor.Core.Shared
{
    internal sealed class PaletteStoreRepository : IDisposable
    {
        private readonly ObservableProperty<PaletteStore> _store = new ObservableProperty<PaletteStore>();
        private PaletteStore _storeInstance;

        public PaletteStoreRepository()
        {
            EditorApplication.update += Update;

            Update();
        }

        public IReadOnlyObservableProperty<PaletteStore> Store => _store;

        public void Dispose()
        {
            EditorApplication.update -= Update;
            _store.Dispose();
        }

        private void Update()
        {
            if (_storeInstance != null)
                return;

            var store = PaletteStore.LoadAsset();
            _store.Value = store;
            _storeInstance = store;
        }
    }
}
