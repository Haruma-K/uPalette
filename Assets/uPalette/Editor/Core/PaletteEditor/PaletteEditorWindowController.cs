using System;
using UnityEngine;
using uPalette.Editor.Core.Shared;
using uPalette.Runtime.Core;
using uPalette.Runtime.Foundation.CharacterStyles;
using uPalette.Runtime.Foundation.TinyRx;

namespace uPalette.Editor.Core.PaletteEditor
{
    internal sealed class PaletteEditorWindowController
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly UPaletteEditorGUIState _guiState;
        private IPaletteEditorWindowContentsViewController _activeContentsViewController;
        private PaletteEditorWindowContentsViewController<CharacterStyle> _characterStyleContentsViewController;
        private PaletteEditorWindowContentsViewController<CharacterStyleTMP> _characterStyleTMPContentsViewController;
        private PaletteEditorWindowContentsViewController<Color> _colorContentsViewController;

        private EditPaletteStoreService _editService;
        private PaletteEditorWindowEmptyViewController _emptyViewController;
        private PaletteEditorWindowContentsViewController<Gradient> _gradientContentsViewController;

        public PaletteEditorWindowController(PaletteStoreRepository storeRepository, UPaletteEditorGUIState guiState,
            PaletteEditorWindow view)
        {
            _guiState = guiState;

            view.UndoShortcutExecutedAsObservable
                .Subscribe(_ => OnUndoCommandExecuted())
                .DisposeWith(_disposables);

            view.RedoShortcutExecutedAsObservable
                .Subscribe(_ => OnRedoCommandExecuted())
                .DisposeWith(_disposables);

            view.RemoveShortcutExecutedAsObservable
                .Subscribe(_ => _activeContentsViewController.OnRemoveShortcutExecuted())
                .DisposeWith(_disposables);

            view.SelectedPaletteTypeChangedAsObservable
                .Subscribe(OnActivePaletteTypeChanged)
                .DisposeWith(_disposables);

            view.CreateButtonClickedAsObservable
                .Subscribe(_ => OnCreateButtonClicked())
                .DisposeWith(_disposables);

            view.LostFocusAsObservable
                .Subscribe(_ => OnLostFocus())
                .DisposeWith(_disposables);

            guiState.ActivePaletteType
                .Subscribe(x => _activeContentsViewController = GetPerTypeController(x))
                .DisposeWith(_disposables);

            storeRepository.Store.Subscribe(x =>
            {
                if (x == null)
                    SetupEmptyView(view);
                else
                    SetupContentsView(x, view);
            }).DisposeWith(_disposables);
        }

        private void SetupContentsView(PaletteStore store, PaletteEditorWindow view)
        {
            _colorContentsViewController?.Dispose();
            _gradientContentsViewController?.Dispose();
            _characterStyleContentsViewController?.Dispose();
            _characterStyleTMPContentsViewController?.Dispose();
            _emptyViewController?.Dispose();

            _editService = new EditPaletteStoreService(store, new GenerateNameEnumsFileService(store));

            _colorContentsViewController =
                new PaletteEditorWindowContentsViewController<Color>(store.ColorPalette, _editService,
                    view.ColorContentsView);
            _gradientContentsViewController =
                new PaletteEditorWindowContentsViewController<Gradient>(store.GradientPalette, _editService,
                    view.GradientContentsView);
            _characterStyleContentsViewController =
                new PaletteEditorWindowContentsViewController<CharacterStyle>(store.CharacterStylePalette, _editService,
                    view.CharacterStyleContentsView);
            _characterStyleTMPContentsViewController =
                new PaletteEditorWindowContentsViewController<CharacterStyleTMP>(store.CharacterStyleTMPPalette,
                    _editService, view.CharacterStyleTMPContentsView);

            _activeContentsViewController = GetPerTypeController(_guiState.ActivePaletteType.Value);
        }

        private void SetupEmptyView(PaletteEditorWindow view)
        {
            _colorContentsViewController?.Dispose();
            _gradientContentsViewController?.Dispose();
            _characterStyleContentsViewController?.Dispose();
            _characterStyleTMPContentsViewController?.Dispose();
            _emptyViewController?.Dispose();

            _emptyViewController = new PaletteEditorWindowEmptyViewController(view.EmptyView);
        }

        private void OnActivePaletteTypeChanged(PaletteType type)
        {
            var oldType = _guiState.ActivePaletteType.Value;
            _editService.Edit($"Change Palette Type To {type.ToString()}",
                () => _guiState.ActivePaletteType.Value = type,
                () => _guiState.ActivePaletteType.Value = oldType, false);
        }

        private IPaletteEditorWindowContentsViewController GetPerTypeController(PaletteType type)
        {
            switch (type)
            {
                case PaletteType.Color:
                    return _colorContentsViewController;
                case PaletteType.Gradient:
                    return _gradientContentsViewController;
                case PaletteType.CharacterStyle:
                    return _characterStyleContentsViewController;
                case PaletteType.CharacterStyleTMP:
                    return _characterStyleTMPContentsViewController;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void Dispose()
        {
            _colorContentsViewController?.Dispose();
            _gradientContentsViewController?.Dispose();
            _characterStyleContentsViewController?.Dispose();
            _characterStyleTMPContentsViewController?.Dispose();
            _emptyViewController?.Dispose();
            _disposables.Dispose();
        }

        private void OnCreateButtonClicked()
        {
            _activeContentsViewController.AddNewEntry();
        }

        private void OnUndoCommandExecuted()
        {
            _editService.Undo();
        }

        private void OnRedoCommandExecuted()
        {
            _editService.Redo();
        }

        private void OnLostFocus()
        {
            var projectSettings = UPaletteProjectSettings.instance;
            if (projectSettings.NameEnumsFileGenerateMode == NameEnumsFileGenerateMode.WhenWindowLosesFocus)
                _editService.GenerateNameEnumsFileIfNeeded();
        }
    }
}
