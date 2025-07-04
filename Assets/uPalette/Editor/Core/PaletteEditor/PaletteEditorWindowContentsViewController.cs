using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using uPalette.Editor.Core.Shared;
using uPalette.Editor.Foundation;
using uPalette.Runtime.Core.Model;
using uPalette.Runtime.Core.Synchronizer;
using uPalette.Runtime.Foundation.TinyRx;
using uPalette.Runtime.Foundation.TinyRx.ObservableProperty;
using Object = UnityEngine.Object;

namespace uPalette.Editor.Core.PaletteEditor
{
    internal sealed class PaletteEditorWindowContentsViewController<T> : IPaletteEditorWindowContentsViewController,
        IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly EditPaletteStoreService _editService;
        private readonly Palette<T> _palette;

        private readonly Dictionary<int, CompositeDisposable> _perItemDisposables =
            new Dictionary<int, CompositeDisposable>();

        private readonly PaletteEditorWindowContentsView<T> _view;

        public PaletteEditorWindowContentsViewController(
            Palette<T> palette,
            EditPaletteStoreService editService,
            PaletteEditorWindowContentsView<T> view
        )
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
                .Subscribe(_ => AddNewEntry())
                .DisposeWith(_disposables);

            view.RightClickRemoveMenuClickedAsObservable
                .Subscribe(_ => OnRightClickRemoveMenuClicked())
                .DisposeWith(_disposables);

            view.RightClickHighlightMenuClickedAsObservable
                .Subscribe(_ => HighlightSelectedEntries())
                .DisposeWith(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void AddNewEntry()
        {
            var entryId = string.Empty;
            _editService.Edit($"Add {typeof(T).Name} Entry {entryId}",
                () => entryId = _palette.AddEntry().Id,
                () => _palette.RemoveEntry(entryId),
                markAsIdOrNameDirty: true);
        }

        public void OnRemoveShortcutExecuted()
        {
            RemoveSelectedItems();
        }

        private void OnItemAdded(PaletteEditorTreeViewEntryItem<T> entryItem)
        {
            var disposables = new CompositeDisposable();
            disposables.DisposeWith(_disposables);

            // Observe item name.
            var entry = _palette.Entries[entryItem.EntryId];
            entryItem.Name.Skip(1)
                .Subscribe(x =>
                {
                    var oldValue = entry.Name.Value;
                    _editService.Edit($"Set {typeof(T).Name} Entry Name {entry.Id}",
                        () => entry.Name.Value = x,
                        () => entry.Name.Value = oldValue,
                        markAsIdOrNameDirty: true);
                })
                .DisposeWith(disposables);

            // Observe item values.
            void ObserveItemValue(KeyValuePair<string, ObservableProperty<T>> value)
            {
                var themeId = value.Key;
                value.Value.Skip(1)
                    .Subscribe(x =>
                    {
                        var oldValue = entry.Values[themeId].Value;
                        _editService.Edit($"Set {typeof(T).Name} Entry Value {entry.Id}",
                            () => { entry.Values[themeId].SetValueAndNotify(x); },
                            () => { entry.Values[themeId].SetValueAndNotify(oldValue); });
                    })
                    .DisposeWith(disposables);
            }

            foreach (var value in entryItem.Values)
                ObserveItemValue(value);

            entryItem.Values.ObservableAdd
                .Subscribe(value =>
                {
                    ObserveItemValue(new KeyValuePair<string, ObservableProperty<T>>(value.Key, value.Value));
                })
                .DisposeWith(disposables);

            // Observe apply button clicks.
            entryItem.ApplyButtonClickedAsObservable.Subscribe(_ => { OpenRegisterEntryIdMenu(entryItem.EntryId); })
                .DisposeWith(disposables);

            _perItemDisposables.Add(entryItem.id, disposables);
        }

        private void OnItemRemoved(TreeViewItem treeViewItem)
        {
            var disposable = _perItemDisposables[treeViewItem.id];
            disposable.Dispose();
            _perItemDisposables.Remove(treeViewItem.id);
        }

        private void OnItemCleared()
        {
            foreach (var disposable in _perItemDisposables.Values) disposable.Dispose();

            _perItemDisposables.Clear();
        }

        private void ItemIndexChanged(PaletteEditorTreeViewEntryItem<T> entryItem, int index)
        {
            var oldIndex = _palette.GetEntryOrder(entryItem.EntryId);
            _editService.Edit($"Change {typeof(T).Name} Entry Index {entryItem.EntryId}",
                () => _palette.SetEntryOrder(entryItem.EntryId, index),
                () =>
                {
                    _palette.SetEntryOrder(entryItem.EntryId, oldIndex);
                    _view.TreeView.SetItemIndex(entryItem, oldIndex, false);
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
            var entries = treeView.GetSelection()
                .Where(x => treeView.GetItem(x) is PaletteEditorTreeViewEntryItem<T>)
                .Select(x =>
                {
                    var item = (PaletteEditorTreeViewEntryItem<T>)treeView.GetItem(x);
                    var entry = _palette.Entries[item.EntryId];
                    return entry;
                })
                .ToArray();

            foreach (var entry in entries)
                _editService.Edit($"Remove Entry {typeof(T).Name} {entry.Id}",
                    () => { _palette.RemoveEntry(entry.Id); },
                    () => { _palette.RestoreEntry(entry.Id); },
                    markAsIdOrNameDirty: true);
            treeView.SetSelection(new List<int>());
        }

        private void HighlightSelectedEntries()
        {
            var treeView = _view.TreeView;
            if (!treeView.HasSelection())
                return;

            var entryIds = treeView.GetSelection()
                .Where(x => treeView.GetItem(x) is PaletteEditorTreeViewEntryItem<T>)
                .Select(x =>
                {
                    var item = (PaletteEditorTreeViewEntryItem<T>)treeView.GetItem(x);
                    return item.EntryId;
                })
                .ToArray();

            var findService = new FindAppliedGameObjectService();
            var gameObjects = findService.Execute(entryIds);
            Selection.objects = gameObjects;
        }

        private void OpenRegisterEntryIdMenu(string entryId)
        {
            var gameObjects = Selection.gameObjects;

            var findService = new FindSynchronizeTargetService();
            var menuFunctions = new Dictionary<string, GenericMenu.MenuFunction>();

            // ComponentType+PropertyDisplayNameをキーとして、該当するSynchronizerをグループ化
            // 例: "CustomImageColor" => [(gameObj1, GraphicColorSynchronizer), (gameObj1, CustomSynchronizer)]
            var synchronizerResults =
                new Dictionary<string, List<(GameObject gameObj, FindSynchronizeTargetService.Result result)>>();

            // Collect all results and group by component/property
            foreach (var gameObj in gameObjects)
            {
                var results = findService.Run<T>(gameObj);

                foreach (var result in results)
                {
                    var key = $"{result.ComponentType.Name}{result.PropertyDisplayName}";
                    if (!synchronizerResults.ContainsKey(key))
                        synchronizerResults[key] = new List<(GameObject, FindSynchronizeTargetService.Result)>();

                    synchronizerResults[key].Add((gameObj, result));
                }
            }

            // Create menu items
            foreach (var kvp in synchronizerResults)
            {
                var groupedResults = kvp.Value;
                // 同一のComponent/Propertyに対して複数のSynchronizerが存在するかチェック
                // 存在する場合はメニュー項目にSynchronizer名を含める
                var hasMultipleSynchronizers =
                    groupedResults.Select(r => r.result.SynchronizerType).Distinct().Count() > 1;

                foreach (var (gameObj, result) in groupedResults)
                {
                    var componentType = result.ComponentType;

                    void MenuFunction()
                    {
                        var wasSynchronizerAttached = false;
                        T oldValue = default;
                        string oldEntryId = null;
                        _editService.Edit($"Register {typeof(T).Name} Entry Id {entryId}",
                            () =>
                            {
                                if (!gameObj.TryGetComponent(result.SynchronizerType, out var synchronizer))
                                {
                                    synchronizer = gameObj.AddComponent(result.SynchronizerType);
                                    wasSynchronizerAttached = true;
                                }

                                var valueSynchronizer = (ValueSynchronizer<T>)synchronizer;
                                oldValue = valueSynchronizer.GetValue();
                                oldEntryId = valueSynchronizer.EntryId.Value;
                                valueSynchronizer.SetEntryId(entryId);
                                EditorUtility.SetDirty(gameObj);
                            },
                            () =>
                            {
                                if (gameObj.TryGetComponent(result.SynchronizerType, out var synchronizer))
                                {
                                    var valueSynchronizer = (ValueSynchronizer<T>)synchronizer;
                                    if (wasSynchronizerAttached)
                                    {
                                        valueSynchronizer.SetValue(oldValue);
                                        Object.DestroyImmediate(synchronizer);
                                    }
                                    else
                                    {
                                        valueSynchronizer.SetEntryId(oldEntryId);
                                    }

                                    EditorUtility.SetDirty(gameObj);
                                }

                                wasSynchronizerAttached = false;
                                oldValue = default;
                            });
                    }

                    var menuName = hasMultipleSynchronizers
                        ? $"{componentType.Name}{result.PropertyDisplayName}/{result.SynchronizerType.Name}"
                        : $"{componentType.Name}{result.PropertyDisplayName}";
                    menuName = ObjectNames.NicifyVariableName(menuName);

                    if (!menuFunctions.ContainsKey(menuName))
                        menuFunctions[menuName] = MenuFunction;
                    else
                        menuFunctions[menuName] += MenuFunction;
                }
            }

            var menu = new GenericMenu();
            foreach (var menuFunction in menuFunctions)
                menu.AddItem(new GUIContent(menuFunction.Key), false, menuFunction.Value);

            if (menu.GetItemCount() == 0)
                menu.AddItem(new GUIContent("There is no suitable setter."), false, null);

            menu.ShowAsContext();
        }
    }
}