using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using uPalette.Editor.Foundation;
using uPalette.Editor.Foundation.CommandBasedUndo;
using uPalette.Runtime.Core;
using uPalette.Runtime.Foundation.Observable;

namespace uPalette.Editor.Core
{
    public class ColorEntryEditorController : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly AutoIncrementHistory _history = new AutoIncrementHistory();

        private readonly Dictionary<int, CompositeDisposable> _perItemDisposables =
            new Dictionary<int, CompositeDisposable>();

        private readonly UPaletteStore _store;
        private ColorEntryEditorTreeView _treeView;
        private UPaletteEditorWindow _window;

        public ColorEntryEditorController(UPaletteStore store)
        {
            _store = store;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void Setup(UPaletteEditorWindow window)
        {
            _window = window;
            _treeView = window.TreeView;

            _window.UndoShortcutExecutedAsObservable.Subscribe(_ => Undo());
            _window.RedoShortcutExecutedAsObservable.Subscribe(_ => Redo());
            _window.CreateButtonClickedAsObservable.Subscribe(_ => OnCreateButtonClicked()).DisposeWith(_disposables);
            _window.RightClickRemoveMenuClickedAsObservable.Subscribe(_ => OnRightClickRemoveMenuClicked())
                .DisposeWith(_disposables);
            _window.RightClickHighlightMenuClickedAsObservable.Subscribe(_ => OnRightClickHighlightMenuClicked())
                .DisposeWith(_disposables);
            _treeView.OnApplyButtonClickedAsObservable.Subscribe(OnApplyButtonClicked).DisposeWith(_disposables);
            _treeView.OnItemAddedAsObservable().Subscribe(OnItemAdded).DisposeWith(_disposables);
            _treeView.OnItemRemovedAsObservable().Subscribe(OnItemRemoved).DisposeWith(_disposables);
            _treeView.OnItemClearedAsObservable().Subscribe(_ => OnItemCleared()).DisposeWith(_disposables);
        }

        public void Clear()
        {
            _disposables.Clear();
        }

        private void OnCreateButtonClicked()
        {
            AddNewEntry();
        }

        private void OnRightClickRemoveMenuClicked()
        {
            RemoveSelectedEntries();
        }

        private void OnRightClickHighlightMenuClicked()
        {
            HighlightSelectedEntries();
        }

        private void OnApplyButtonClicked(ColorEntryEditorTreeViewItem item)
        {
            ApplyEntry(item.EntryId);
        }

        private void OnItemAdded(TreeViewItem treeViewItem)
        {
            var item = (ColorEntryEditorTreeViewItem) treeViewItem;
            var disposables = new CompositeDisposable();

            var entry = _store.Entries.First(x => x.ID.Equals(item.EntryId));
            item.Name.Subscribe(newValue =>
            {
                var oldValue = entry.Name.Value;
                _history.Register($"{GetType().Name}{nameof(OnItemAdded)}{entry.ID}{nameof(item.Name)}",
                    () =>
                    {
                        entry.Name.Value = newValue;
                        _store.IsDirty.Value = true;
                    },
                    () =>
                    {
                        entry.Name.Value = oldValue;
                        _store.IsDirty.Value = true;
                    });
            }).DisposeWith(disposables);
            item.Color.Subscribe(newValue =>
            {
                var oldValue = entry.Value.Value;
                _history.Register($"{GetType().Name}{nameof(OnItemAdded)}{entry.ID}{nameof(item.Color)}",
                    () =>
                    {
                        entry.SetColor(newValue);
                        _store.IsDirty.Value = true;
                    },
                    () =>
                    {
                        entry.SetColor(oldValue);
                        _store.IsDirty.Value = true;
                    });
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
            {
                disposable.Dispose();
            }

            _perItemDisposables.Clear();
        }

        private void Undo()
        {
            _history.Undo();
            GUI.FocusControl("");
        }

        private void Redo()
        {
            _history.Redo();
            GUI.FocusControl("");
        }

        private void AddNewEntry()
        {
            var entry = new ColorEntry();
            _history.Register($"{GetType().Name}{nameof(AddNewEntry)}{entry.ID}",
                () =>
                {
                    _store.Entries.Add(entry);
                    _store.IsDirty.Value = true;
                },
                () =>
                {
                    _store.Entries.Remove(entry);
                    _store.IsDirty.Value = true;
                });
        }

        private void RemoveSelectedEntries()
        {
            if (!_treeView.HasSelection())
            {
                return;
            }

            foreach (var id in _treeView.GetSelection())
            {
                var item = (ColorEntryEditorTreeViewItem) _treeView.GetItem(id);
                var entry = _store.Entries.First(x => x.ID.Equals(item.EntryId));
                _history.Register($"{GetType().Name}{nameof(RemoveSelectedEntries)}{entry.ID}",
                    () =>
                    {
                        _store.Entries.Remove(entry);
                        _store.IsDirty.Value = true;
                    },
                    () =>
                    {
                        _store.Entries.Add(entry);
                        _store.IsDirty.Value = true;
                    });
            }

            _treeView.SetSelection(new List<int>());
        }

        private void HighlightSelectedEntries()
        {
            if (!_treeView.HasSelection())
            {
                return;
            }

            var entryIds = _treeView.GetSelection().Select(x =>
            {
                var item = (ColorEntryEditorTreeViewItem) _treeView.GetItem(x);
                return item.EntryId;
            }).ToArray();

            var findService = new FindAppliedGameObjectService();
            var gameObjects = findService.Execute(entryIds);
            Selection.objects = gameObjects;
        }

        private void ApplyEntry(string entryId)
        {
            var activeGameObject = Selection.activeGameObject;
            if (activeGameObject == null)
            {
                return;
            }

            var menu = new GenericMenu();
            var setters = TypeCache.GetTypesWithAttribute<ColorSetterAttribute>();
            foreach (var setterType in setters)
            {
                var attribute = setterType.GetCustomAttribute<ColorSetterAttribute>();
                var targetType = attribute.TargetType;
                if (activeGameObject.TryGetComponent(targetType, out var target))
                {
                    var targetTypeName = target.GetType().Name;
                    var targetName = $"{targetTypeName}{attribute.TargetPropertyDisplayName}";
                    targetName = ObjectNames.NicifyVariableName(targetName);
                    menu.AddItem(new GUIContent(targetName), false, () =>
                    {
                        if (!activeGameObject.TryGetComponent(setterType, out var setter))
                        {
                            setter = UnityEditor.Undo.AddComponent(activeGameObject, setterType);
                        }

                        ((ColorSetter) setter).SetEntry(entryId);

                        // Focus on the InspectorWindow to enable Undo.
                        var windowTypes = TypeCache.GetTypesDerivedFrom<EditorWindow>();
                        foreach (var windowType in windowTypes)
                        {
                            if (windowType.Name.Equals("InspectorWindow"))
                            {
                                EditorWindow.GetWindow(windowType);
                            }
                        }
                    });
                }
            }

            if (menu.GetItemCount() == 0)
            {
                menu.AddItem(new GUIContent("No suitable Color Setter found."), false, null);
            }

            menu.ShowAsContext();
        }
    }
}