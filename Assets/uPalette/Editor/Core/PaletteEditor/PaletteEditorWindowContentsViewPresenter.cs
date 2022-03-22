using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using uPalette.Runtime.Core.Model;
using uPalette.Runtime.Foundation.TinyRx;

namespace uPalette.Editor.Core.PaletteEditor
{
    internal sealed class PaletteEditorWindowContentsViewPresenter<T> : IDisposable
    {
        private readonly CompositeDisposable _columnsDisposables = new CompositeDisposable();
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly Palette<T> _palette;

        private readonly Dictionary<string, CompositeDisposable> _perItemDisposables =
            new Dictionary<string, CompositeDisposable>();

        private readonly PaletteEditorWindowContentsView<T> _view;

        public PaletteEditorWindowContentsViewPresenter(Palette<T> palette, PaletteEditorWindowContentsView<T> view)
        {
            _palette = palette;
            _view = view;

            // Observe Entries.
            var entries = _palette.Entries;
            entries.ObservableAdd.Subscribe(x => OnEntryAdded(x.Value)).DisposeWith(_disposables);
            entries.ObservableRemove.Subscribe(x => OnEntryRemoved(x.Value)).DisposeWith(_disposables);
            entries.ObservableClear.Subscribe(x => OnEntryCleared()).DisposeWith(_disposables);

            // Observe Themes.
            _palette.Themes.ObservableAdd
                .Subscribe(x => OnThemesChanged())
                .DisposeWith(_disposables);

            _palette.Themes.ObservableRemove
                .Subscribe(x => OnThemesChanged())
                .DisposeWith(_disposables);

            _palette.Themes.ObservableClear
                .Subscribe(x => OnThemesChanged())
                .DisposeWith(_disposables);

            _palette.Themes.ObservableReplace
                .Subscribe(x => OnThemesChanged())
                .DisposeWith(_disposables);

            _palette.ThemeOrderChangedAsObservable
                .Subscribe(_ => OnThemeOrderChanged())
                .DisposeWith(_disposables);

            _palette.ActiveTheme
                .Subscribe(_ => OnThemeOrderChanged())
                .DisposeWith(_disposables);

            // Set initial values.
            RefreshTreeViewColumns();
            var sortedEntries = entries.Values.OrderBy(x => _palette.GetEntryOrder(x.Id));
            foreach (var entry in sortedEntries)
            {
                var index = _palette.GetEntryOrder(entry.Id);
                AddTreeViewItem(entry, index);
            }

            view.TreeView.Reload();
        }

        public void Dispose()
        {
            _disposables.Dispose();
            _view.TreeView.ClearItems();
            _view.TreeView.ClearThemeColumns();
        }

        private void OnEntryAdded(Entry<T> entry)
        {
            var index = _palette.GetEntryOrder(entry.Id);
            AddTreeViewItem(entry, index);
        }

        private void OnEntryRemoved(Entry<T> entry)
        {
            RemoveTreeViewItem(entry.Id);
        }

        private void OnEntryCleared()
        {
            ClearTreeViewItem();
        }

        private void OnThemesChanged()
        {
            RefreshTreeViewColumns();
        }

        private void OnThemeOrderChanged()
        {
            RefreshTreeViewColumns();
        }

        private void AddTreeViewItem(Entry<T> entry, int index)
        {
            var treeView = _view.TreeView;

            // Set up tree view item.
            var values = new Dictionary<string, T>();
            foreach (var v in entry.Values)
            {
                var themeId = v.Key;
                var value = v.Value.Value;
                values.Add(themeId, value);
            }

            var item = treeView.AddItem(entry.Id, entry.Name.Value, values);
            treeView.SetItemIndex(item, index, false);

            // Observe entry.
            var disposable = new CompositeDisposable();

            entry.Name.Skip(1)
                .Subscribe(x => item.SetName(x, false))
                .DisposeWith(disposable);

            foreach (var v in entry.Values)
            {
                var themeId = v.Key;
                v.Value.Skip(1)
                    .Subscribe(x => item.Values[themeId].SetValueAndNotNotify(x))
                    .DisposeWith(disposable);
            }

            // Observe entry values.
            entry.Values.ObservableAdd.Subscribe(x =>
            {
                var themeId = x.Key;
                item.AddValue(themeId, x.Value.Value);
                x.Value.Skip(1)
                    .Subscribe(y => item.Values[themeId].SetValueAndNotNotify(y))
                    .DisposeWith(disposable);
            }).DisposeWith(disposable);

            entry.Values.ObservableRemove.Subscribe(x =>
            {
                var themeId = x.Key;
                item.RemoveValue(themeId);
            }).DisposeWith(disposable);

            entry.Values.ObservableClear
                .Subscribe(x => item.ClearValues())
                .DisposeWith(disposable);

            _perItemDisposables.Add(entry.Id, disposable);
            treeView.Reload();
        }

        private void RemoveTreeViewItem(string entryId)
        {
            var treeView = _view.TreeView;
            treeView.RemoveItem(entryId);
            var disposable = _perItemDisposables[entryId];
            disposable.Dispose();
            _perItemDisposables.Remove(entryId);
            treeView.Reload();
        }

        private void ClearTreeViewItem()
        {
            var treeView = _view.TreeView;
            treeView.ClearItems();
            foreach (var disposable in _perItemDisposables.Values) disposable.Dispose();

            _perItemDisposables.Clear();
            treeView.Reload();
        }

        private void RefreshTreeViewColumns()
        {
            _columnsDisposables.Clear();
            var treeView = _view.TreeView;

            treeView.ClearThemeColumns();
            foreach (var theme in _palette.Themes.Values.OrderBy(x => _palette.GetThemeOrder(x.Id)))
            {
                treeView.AddThemeColumn(theme.Id, GetDisplayThemeName(theme));
                theme.Name
                    .Subscribe(x => treeView.SetThemeName(theme.Id, GetDisplayThemeName(theme)))
                    .DisposeWith(_columnsDisposables);
            }

            treeView.Reload();
        }

        private string GetDisplayThemeName(Theme theme)
        {
            var displayName = theme.Name.Value;
            if (_palette.ActiveTheme.Value.Id == theme.Id) displayName += " (Active)";

            return displayName;
        }
    }
}
