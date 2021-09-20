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
            var item = (ColorEntryEditorTreeViewItem)treeViewItem;
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
                var item = (ColorEntryEditorTreeViewItem)_treeView.GetItem(id);
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
                var item = (ColorEntryEditorTreeViewItem)_treeView.GetItem(x);
                return item.EntryId;
            }).ToArray();

            var findService = new FindAppliedGameObjectService();
            var gameObjects = findService.Execute(entryIds);
            Selection.objects = gameObjects;
        }

        private void ApplyEntry(string entryId)
        {
            var gameObjects = Selection.gameObjects;
            var applyInfoList = new List<ApplyInfo>();
            foreach (var colorSetterType in TypeCache.GetTypesWithAttribute<ColorSetterAttribute>())
            {
                var setterAttribute = colorSetterType.GetCustomAttribute<ColorSetterAttribute>();
                var targetType = setterAttribute.TargetType;
                foreach (var gameObj in gameObjects)
                {
                    if (gameObj.TryGetComponent(targetType, out var target))
                    {
                        var actualTargetType = target.GetType();
                        applyInfoList.Add(new ApplyInfo
                        {
                            GameObj = gameObj,
                            ActualTargetType = actualTargetType,
                            ColorSetterType = colorSetterType,
                            ColorSetterAttribute = setterAttribute
                        });
                    }
                }
            }

            var menuFunctions = new Dictionary<string, GenericMenu.MenuFunction>();
            foreach (var applyInfo in applyInfoList)
            {
                var actualTargetType = applyInfo.ActualTargetType;
                var targetPropertyName = applyInfo.ColorSetterAttribute.TargetPropertyDisplayName;
                var targetName = $"{actualTargetType.Name}{targetPropertyName}";
                targetName = ObjectNames.NicifyVariableName(targetName);
                GenericMenu.MenuFunction menuFunction = () =>
                {
                    if (!applyInfo.GameObj.TryGetComponent(applyInfo.ColorSetterType, out var setter))
                    {
                        setter = UnityEditor.Undo.AddComponent(applyInfo.GameObj, applyInfo.ColorSetterType);
                    }

                    ((ColorSetter)setter).SetEntry(entryId);

                    // Focus on the InspectorWindow to enable Undo.
                    var windowTypes = TypeCache.GetTypesDerivedFrom<EditorWindow>();
                    foreach (var windowType in windowTypes)
                    {
                        if (windowType.Name.Equals("InspectorWindow"))
                        {
                            EditorWindow.GetWindow(windowType);
                        }
                    }
                };

                if (!menuFunctions.ContainsKey(targetName))
                {
                    menuFunctions[targetName] = menuFunction;
                }
                else
                {
                    menuFunctions[targetName] += menuFunction;
                }
            }

            var menu = new GenericMenu();
            foreach (var menuFunction in menuFunctions)
            {
                menu.AddItem(new GUIContent(menuFunction.Key), false, menuFunction.Value);
            }

            if (menu.GetItemCount() == 0)
            {
                menu.AddItem(new GUIContent("No suitable Color Setter found."), false, null);
            }

            menu.ShowAsContext();
        }

        private class ApplyInfo
        {
            public GameObject GameObj { get; set; }
            public Type ActualTargetType { get; set; }
            public Type ColorSetterType { get; set; }
            public ColorSetterAttribute ColorSetterAttribute { get; set; }
        }
    }
}