using System;
using System.Collections.Generic;
using UnityEditor;
using uPalette.Runtime.Core;
using uPalette.Runtime.Foundation.Observable;

namespace uPalette.Editor.Core
{
    public class ColorEntryEditorPresenter : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly UPaletteStore _store;

        private readonly Dictionary<string, CompositeDisposable> _perItemDisposables =
            new Dictionary<string, CompositeDisposable>();
        private ColorEntryEditorTreeView _treeView;
        private UPaletteEditorWindow _window;

        public ColorEntryEditorPresenter(UPaletteStore store)
        {
            _store = store;
        }

        public void Dispose()
        {
            _disposables.Dispose();
            Selection.selectionChanged -= OnSelectionChanged;
        }

        public void Setup(UPaletteEditorWindow window)
        {
            _window = window;
            _treeView = window.TreeView;

            var entries = _store.Entries;
            entries.ObservableAdd.Subscribe(x => OnEntryAdded(x.Value)).DisposeWith(_disposables);
            entries.ObservableRemove.Subscribe(x => OnEntryRemoved(x.Value)).DisposeWith(_disposables);
            entries.ObservableClear.Subscribe(x => OnEntryCleared()).DisposeWith(_disposables);

            // Set initial values.
            foreach (var entry in _store.Entries)
            {
                AddTreeViewItem(entry);
            }

            Selection.selectionChanged += OnSelectionChanged;
            OnSelectionChanged();

            _treeView.Reload();
        }

        public void Clear()
        {
            _disposables.Clear();
        }

        private void OnEntryAdded(ColorEntry entry)
        {
            AddTreeViewItem(entry);
        }

        private void OnEntryRemoved(ColorEntry entry)
        {
            RemoveTreeViewItem(entry);
        }

        private void OnEntryCleared()
        {
            ClearTreeViewItem();
        }

        private void OnSelectionChanged()
        {
            _treeView.IsApplyButtonEnabled = Selection.activeGameObject != null;
            _window.Repaint();
        }

        private void AddTreeViewItem(ColorEntry entry)
        {
            var item = _treeView.AddItem(entry);
            var disposable = new CompositeDisposable();
            entry.Name.Subscribe(x => item.Name.SetValueAndNotNotify(x)).DisposeWith(disposable);
            entry.Value.Subscribe(x => item.Color.SetValueAndNotNotify(x)).DisposeWith(disposable);
            _perItemDisposables.Add(entry.ID, disposable);
            _treeView.Reload();
        }

        private void RemoveTreeViewItem(ColorEntry entry)
        {
            _treeView.RemoveItem(entry);
            var disposable = _perItemDisposables[entry.ID];
            disposable.Dispose();
            _perItemDisposables.Remove(entry.ID);
            _treeView.Reload();
        }

        private void ClearTreeViewItem()
        {
            _treeView.ClearItems();
            foreach (var disposable in _perItemDisposables.Values)
            {
                disposable.Dispose();
            }
            _perItemDisposables.Clear();
            _treeView.Reload();
        }
    }
}