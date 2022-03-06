using System;
using UnityEngine;
using uPalette.Runtime.Foundation.EditorPrefsProperty.EditorPrefsProperty.Editor;
using uPalette.Runtime.Foundation.TinyRx;
using uPalette.Runtime.Foundation.TinyRx.ObservableProperty;

namespace uPalette.Editor.Core.Shared
{
    [Serializable]
    internal sealed class UPaletteEditorGUIState : IDisposable
    {
        [SerializeField]
        private ObservableProperty<PaletteType> _activePaletteType = new ObservableProperty<PaletteType>();

        private EditorPrefsProperty<int> _activePaletteTypePrefs =
            new IntEditorPrefsProperty(EditorPrefsKey.ActivePaletteTypePrefsKey, 0);

        private CompositeDisposable _disposables = new CompositeDisposable();

        public UPaletteEditorGUIState()
        {
            _activePaletteType.Value = (PaletteType)_activePaletteTypePrefs.Value;
            _activePaletteType
                .Subscribe(x => _activePaletteTypePrefs.Value = (int)x)
                .DisposeWith(_disposables);
        }

        public IObservableProperty<PaletteType> ActivePaletteType => _activePaletteType;

        public void Dispose()
        {
            _activePaletteType.Dispose();
            _disposables.Dispose();
        }
    }
}
