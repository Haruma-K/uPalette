using System;
using System.Collections.Generic;
using System.Linq;
using uPalette.Runtime.Core.Model;
using uPalette.Runtime.Foundation.TinyRx;

namespace uPalette.Editor.Core.ThemeEditor
{
    internal sealed class ThemeEditorWindowContentsViewPresenter<T> : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly Palette<T> _palette;

        private readonly Dictionary<string, CompositeDisposable> _perItemDisposables =
            new Dictionary<string, CompositeDisposable>();

        private readonly ThemeEditorWindowContentsView _view;

        public ThemeEditorWindowContentsViewPresenter(Palette<T> palette, ThemeEditorWindowContentsView view)
        {
            _palette = palette;
            _view = view;

            var themes = _palette.Themes;

            themes.ObservableAdd
                .Subscribe(x => OnThemeAdded(x.Value))
                .DisposeWith(_disposables);

            themes.ObservableRemove
                .Subscribe(x => OnThemeRemoved(x.Value))
                .DisposeWith(_disposables);

            themes.ObservableClear
                .Subscribe(x => OnThemeCleared())
                .DisposeWith(_disposables);

            // Set initial values.
            var sortedThemes = themes.Values.OrderBy(x => _palette.GetThemeOrder(x.Id));
            foreach (var theme in sortedThemes)
            {
                var index = _palette.GetThemeOrder(theme.Id);
                AddTreeViewItem(theme, index);
            }

            view.TreeView.Reload();
        }

        public void Dispose()
        {
            _disposables.Dispose();
            _view.TreeView.ClearItems();
        }

        private void OnThemeAdded(Theme theme)
        {
            var index = _palette.GetThemeOrder(theme.Id);
            AddTreeViewItem(theme, index);
        }

        private void OnThemeRemoved(Theme theme)
        {
            RemoveTreeViewItem(theme.Id);
        }

        private void OnThemeCleared()
        {
            ClearTreeViewItem();
        }

        private void AddTreeViewItem(Theme theme, int index)
        {
            var treeView = _view.TreeView;

            var isActiveTheme = _palette.ActiveTheme.Value.Id == theme.Id;
            var item = treeView.AddItem(theme.Id, theme.Name.Value, isActiveTheme);
            treeView.SetItemIndex(item, index, false);

            // Observe theme.
            var disposable = new CompositeDisposable();
            theme.Name.Skip(1)
                .Subscribe(x => item.SetName(x, false))
                .DisposeWith(disposable);

            _palette.ActiveTheme.Skip(1).Subscribe(x =>
            {
                var isActive = _palette.ActiveTheme.Value.Id == theme.Id;
                item.IsActive.SetValueAndNotNotify(isActive);
            }).DisposeWith(disposable);

            _perItemDisposables.Add(theme.Id, disposable);
            treeView.Reload();
        }

        private void RemoveTreeViewItem(string themeId)
        {
            var treeView = _view.TreeView;
            treeView.RemoveItem(themeId);
            var disposable = _perItemDisposables[themeId];
            disposable.Dispose();
            _perItemDisposables.Remove(themeId);
            treeView.Reload();
        }

        private void ClearTreeViewItem()
        {
            var treeView = _view.TreeView;
            treeView.ClearItems();

            foreach (var disposable in _perItemDisposables.Values)
                disposable.Dispose();

            _perItemDisposables.Clear();
            treeView.Reload();
        }
    }
}
