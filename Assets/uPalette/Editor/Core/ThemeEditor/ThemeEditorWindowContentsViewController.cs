using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using uPalette.Editor.Core.Shared;
using uPalette.Editor.Foundation;
using uPalette.Runtime.Core.Model;
using uPalette.Runtime.Foundation.TinyRx;

namespace uPalette.Editor.Core.ThemeEditor
{
    internal sealed class ThemeEditorWindowContentsViewController<T> : IThemeEditorWindowContentsViewController,
        IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly EditPaletteStoreService _editService;
        private readonly Palette<T> _palette;

        private readonly Dictionary<int, CompositeDisposable> _perItemDisposables =
            new Dictionary<int, CompositeDisposable>();

        private readonly ThemeEditorWindowContentsView _view;

        public ThemeEditorWindowContentsViewController(Palette<T> palette, EditPaletteStoreService editService,
            ThemeEditorWindowContentsView view)
        {
            _palette = palette;
            _editService = editService;
            _view = view;

            var treeView = _view.TreeView;

            treeView.OnItemAddedAsObservable()
                .Subscribe(OnItemAdded)
                .DisposeWith(_disposables);

            treeView.OnItemRemovedAsObservable()
                .Subscribe(OnItemRemoved)
                .DisposeWith(_disposables);

            treeView.OnItemClearedAsObservable()
                .Subscribe(_ => OnItemCleared())
                .DisposeWith(_disposables);

            treeView.ItemIndexChangedAsObservable
                .Subscribe(x => ItemIndexChanged(x.item, x.newIndex))
                .DisposeWith(_disposables);

            view.RightClickCreateMenuClickedAsObservable
                .Subscribe(_ => AddNewTheme())
                .DisposeWith(_disposables);

            view.RightClickRemoveMenuClickedAsObservable
                .Subscribe(_ => OnRightClickRemoveMenuClicked())
                .DisposeWith(_disposables);
        }

        public void Dispose()
        {
            foreach (var disposable in _perItemDisposables.Values) disposable.Dispose();
            _disposables.Dispose();
        }

        public void AddNewTheme()
        {
            var themeId = string.Empty;
            var themeIndex = _palette.Themes.Count;

            _editService.Edit($"Add {typeof(T).Name} Theme {themeIndex}",
                () => themeId = _palette.AddTheme().Id,
                () => _palette.RemoveTheme(themeId),
                markAsIdOrNameDirty: true);
        }

        public void OnRemoveShortcutExecuted()
        {
            RemoveSelectedItems();
        }

        private void OnItemAdded(ThemeEditorTreeViewItem item)
        {
            var disposables = new CompositeDisposable();

            // Observe item name.
            var theme = _palette.Themes[item.ThemeId];
            item.Name.Skip(1).Subscribe(x =>
            {
                var oldValue = theme.Name.Value;
                _editService.Edit($"Set {typeof(T).Name} Theme Name {theme.Id}",
                    () => theme.Name.Value = x,
                    () => theme.Name.Value = oldValue,
                    markAsIdOrNameDirty: true);
            }).DisposeWith(disposables);

            // Observe IsActive property.
            item.IsActive.Skip(1).Subscribe(x =>
            {
                if (!x) return;

                var oldActiveTheme = _palette.ActiveTheme.Value;
                _editService.Edit($"Set {typeof(T).Name} Active Theme {item.ThemeId}",
                    () => _palette.SetActiveTheme(item.ThemeId),
                    () => _palette.SetActiveTheme(oldActiveTheme.Id));
            }).DisposeWith(disposables);

            _perItemDisposables.Add(item.id, disposables);
        }

        private void OnItemRemoved(TreeViewItem treeViewItem)
        {
            var disposable = _perItemDisposables[treeViewItem.id];
            disposable.Dispose();
            _perItemDisposables.Remove(treeViewItem.id);
        }

        private void OnItemCleared()
        {
            foreach (var disposable in _perItemDisposables.Values)
                disposable.Dispose();

            _perItemDisposables.Clear();
        }

        private void ItemIndexChanged(ThemeEditorTreeViewItem item, int index)
        {
            var oldIndex = _palette.GetThemeOrder(item.ThemeId);
            _editService.Edit($"Change {typeof(T).Name} Theme Index {item.ThemeId}",
                () => _palette.SetThemeOrder(item.ThemeId, index),
                () =>
                {
                    _palette.SetThemeOrder(item.ThemeId, oldIndex);
                    _view.TreeView.SetItemIndex(item, oldIndex, false);
                    _view.TreeView.Reload();
                },
                markAsIdOrNameDirty: true);
        }

        private void OnRightClickRemoveMenuClicked()
        {
            RemoveSelectedItems();
        }

        private void RemoveSelectedItems()
        {
            var treeView = _view.TreeView;
            var themes = treeView.GetSelection().Select(x =>
            {
                var item = (ThemeEditorTreeViewItem)treeView.GetItem(x);
                var theme = _palette.Themes[item.ThemeId];
                return theme;
            }).ToArray();

            foreach (var theme in themes)
                _editService.Edit($"Remove {typeof(T).Name} Theme {theme.Id}",
                    () => _palette.RemoveTheme(theme.Id),
                    () => _palette.RestoreTheme(theme.Id),
                    markAsIdOrNameDirty: true);
            treeView.SetSelection(new List<int>());
        }
    }
}
