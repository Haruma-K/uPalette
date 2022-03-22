using System;
using UnityEngine;
using uPalette.Editor.Core.Shared;
using uPalette.Runtime.Core;
using uPalette.Runtime.Foundation.CharacterStyles;
using uPalette.Runtime.Foundation.TinyRx;

namespace uPalette.Editor.Core.PaletteEditor
{
    internal sealed class PaletteEditorWindowPresenter : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly UPaletteEditorGUIState _guiState;
        private PaletteEditorWindowContentsViewPresenter<CharacterStyle> _characterStyleContentsViewPresenter;
        private PaletteEditorWindowContentsViewPresenter<CharacterStyleTMP> _characterStyleTMPContentsViewPresenter;
        private PaletteEditorWindowContentsViewPresenter<Color> _colorContentsViewPresenter;
        private PaletteEditorEmptyViewPresenter _emptyViewPresenter;
        private PaletteEditorWindowContentsViewPresenter<Gradient> _gradientContentsViewPresenter;

        public PaletteEditorWindowPresenter(PaletteStoreRepository storeRepository, UPaletteEditorGUIState guiState,
            PaletteEditorWindow view)
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
            _disposables.Dispose();
        }

        private void SetupContentsView(PaletteStore store, PaletteEditorWindow view)
        {
            _colorContentsViewPresenter?.Dispose();
            _gradientContentsViewPresenter?.Dispose();
            _characterStyleContentsViewPresenter?.Dispose();
            _characterStyleTMPContentsViewPresenter?.Dispose();
            _emptyViewPresenter?.Dispose();

            _colorContentsViewPresenter =
                new PaletteEditorWindowContentsViewPresenter<Color>(store.ColorPalette, view.ColorContentsView);
            _gradientContentsViewPresenter =
                new PaletteEditorWindowContentsViewPresenter<Gradient>(store.GradientPalette,
                    view.GradientContentsView);
            _characterStyleContentsViewPresenter =
                new PaletteEditorWindowContentsViewPresenter<CharacterStyle>(store.CharacterStylePalette,
                    view.CharacterStyleContentsView);
            _characterStyleTMPContentsViewPresenter =
                new PaletteEditorWindowContentsViewPresenter<CharacterStyleTMP>(store.CharacterStyleTMPPalette,
                    view.CharacterStyleTMPContentsView);

            view.SetMode(PaletteEditorWindow.Mode.Contents);
        }

        private void SetupEmptyView(PaletteEditorWindow view)
        {
            _colorContentsViewPresenter?.Dispose();
            _gradientContentsViewPresenter?.Dispose();
            _characterStyleContentsViewPresenter?.Dispose();
            _characterStyleTMPContentsViewPresenter?.Dispose();
            _emptyViewPresenter?.Dispose();

            _emptyViewPresenter = new PaletteEditorEmptyViewPresenter(view.EmptyView);

            view.SetMode(PaletteEditorWindow.Mode.Empty);
        }
    }
}
