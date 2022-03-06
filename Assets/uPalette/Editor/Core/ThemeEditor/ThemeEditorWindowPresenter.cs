using System;
using UnityEngine;
using uPalette.Editor.Core.Shared;
using uPalette.Runtime.Core;
using uPalette.Runtime.Foundation.CharacterStyles;
using uPalette.Runtime.Foundation.TinyRx;

namespace uPalette.Editor.Core.ThemeEditor
{
    internal sealed class ThemeEditorWindowPresenter : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly UPaletteEditorGUIState _guiState;
        private ThemeEditorWindowContentsViewPresenter<CharacterStyle> _characterStyleContentsViewPresenter;
        private ThemeEditorWindowContentsViewPresenter<CharacterStyleTMP> _characterStyleTMPContentsViewPresenter;
        private ThemeEditorWindowContentsViewPresenter<Color> _colorContentsViewPresenter;
        private ThemeEditorEmptyViewPresenter _emptyViewPresenter;
        private ThemeEditorWindowContentsViewPresenter<Gradient> _gradientContentsViewPresenter;

        public ThemeEditorWindowPresenter(PaletteStoreRepository storeRepository, UPaletteEditorGUIState guiState,
            ThemeEditorWindow view)
        {
            _guiState = guiState;

            _guiState.ActivePaletteType
                .Subscribe(view.SetActiveContentView)
                .DisposeWith(_disposables);

            view.SetActiveContentView(_guiState.ActivePaletteType.Value);

            storeRepository.Store.Subscribe(x =>
            {
                if (x == null)
                    SetupEmptyView(view);
                else
                    SetupContentsView(x, view);
            }).DisposeWith(_disposables);
        }

        public void Dispose()
        {
            _colorContentsViewPresenter?.Dispose();
            _gradientContentsViewPresenter?.Dispose();
            _characterStyleContentsViewPresenter?.Dispose();
            _characterStyleTMPContentsViewPresenter?.Dispose();
            _emptyViewPresenter?.Dispose();
            _disposables?.Dispose();
        }

        private void SetupContentsView(PaletteStore store, ThemeEditorWindow view)
        {
            _colorContentsViewPresenter?.Dispose();
            _gradientContentsViewPresenter?.Dispose();
            _characterStyleContentsViewPresenter?.Dispose();
            _characterStyleTMPContentsViewPresenter?.Dispose();
            _emptyViewPresenter?.Dispose();

            _colorContentsViewPresenter =
                new ThemeEditorWindowContentsViewPresenter<Color>(store.ColorPalette, view.ColorContentsView);
            _gradientContentsViewPresenter =
                new ThemeEditorWindowContentsViewPresenter<Gradient>(store.GradientPalette,
                    view.GradientContentsView);
            _characterStyleContentsViewPresenter =
                new ThemeEditorWindowContentsViewPresenter<CharacterStyle>(store.CharacterStylePalette,
                    view.CharacterStyleContentsView);
            _characterStyleTMPContentsViewPresenter =
                new ThemeEditorWindowContentsViewPresenter<CharacterStyleTMP>(store.CharacterStyleTMPPalette,
                    view.CharacterStyleTMPContentsView);

            view.SetMode(ThemeEditorWindow.Mode.Contents);
        }

        private void SetupEmptyView(ThemeEditorWindow view)
        {
            _colorContentsViewPresenter?.Dispose();
            _gradientContentsViewPresenter?.Dispose();
            _characterStyleContentsViewPresenter?.Dispose();
            _characterStyleTMPContentsViewPresenter?.Dispose();
            _emptyViewPresenter?.Dispose();

            _emptyViewPresenter = new ThemeEditorEmptyViewPresenter(view.EmptyView);

            view.SetMode(ThemeEditorWindow.Mode.Empty);
        }
    }
}
