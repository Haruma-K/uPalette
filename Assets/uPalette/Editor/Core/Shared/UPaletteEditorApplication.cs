using System;
using uPalette.Editor.Core.PaletteEditor;
using uPalette.Editor.Core.ThemeEditor;

namespace uPalette.Editor.Core.Shared
{
    internal sealed class UPaletteEditorApplication : IDisposable
    {
        private static int _referenceCount;
        private static UPaletteEditorApplication _instance;
        private readonly PaletteStoreRepository _storeRepository = new PaletteStoreRepository();
        private readonly UPaletteEditorGUIState _guiState;
        private PaletteEditorWindowController _paletteEditorWindowController;
        private PaletteEditorWindowPresenter _paletteEditorWindowPresenter;
        private ThemeEditorWindowController _themeEditorWindowController;
        private ThemeEditorWindowPresenter _themeEditorWindowPresenter;

        private UPaletteEditorApplication()
        {
            _guiState = new UPaletteEditorGUIState();
        }

        public void Dispose()
        {
            _guiState.Dispose();
            _paletteEditorWindowController?.Dispose();
            _paletteEditorWindowPresenter?.Dispose();
            _themeEditorWindowController?.Dispose();
            _themeEditorWindowPresenter?.Dispose();
            _storeRepository.Dispose();

            var store = _storeRepository.Store.Value;
            if (store != null)
            {
                store.ColorPalette.ClearRemovedEntries();
                store.GradientPalette.ClearRemovedEntries();
                store.ColorPalette.ClearRemovedThemes();
                store.GradientPalette.ClearRemovedThemes();
            }
        }

        public void SetupPaletteEditor(PaletteEditorWindow window)
        {
            var controller = new PaletteEditorWindowController(_storeRepository, _guiState, window);
            var presenter = new PaletteEditorWindowPresenter(_storeRepository, _guiState, window);
            _paletteEditorWindowController = controller;
            _paletteEditorWindowPresenter = presenter;
        }

        public void CleanupPaletteEditor()
        {
            _paletteEditorWindowController.Dispose();
            _paletteEditorWindowPresenter.Dispose();
        }

        public void SetupThemeEditor(ThemeEditorWindow window)
        {
            var controller = new ThemeEditorWindowController(_storeRepository, _guiState, window);
            var presenter = new ThemeEditorWindowPresenter(_storeRepository, _guiState, window);
            _themeEditorWindowController = controller;
            _themeEditorWindowPresenter = presenter;
        }

        public void CleanupThemeEditor()
        {
            _themeEditorWindowController.Dispose();
            _themeEditorWindowPresenter.Dispose();
        }

        public static UPaletteEditorApplication RequestInstance()
        {
            if (_referenceCount++ == 0)
                _instance = new UPaletteEditorApplication();

            return _instance;
        }

        public static void ReleaseInstance()
        {
            if (--_referenceCount == 0)
            {
                _instance.Dispose();
                _instance = null;
            }
        }
    }
}
