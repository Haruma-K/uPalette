using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using uPalette.Editor.Foundation.EasyTreeView;
using uPalette.Runtime.Foundation.TinyRx;

namespace uPalette.Editor.Core.PaletteEditor
{
    internal abstract class PaletteEditorTreeView<T> : TreeViewBase
    {
        private const string DragType = "PaletteEditorTreeView";

        private readonly Dictionary<int, string> _columnIndexToThemeIdMap = new Dictionary<int, string>();
        private readonly Dictionary<string, int> _entryIdToItemIdMap = new Dictionary<string, int>();

        private readonly Subject<(PaletteEditorTreeViewItem<T> item, int newIndex)> _itemIndexChangedSubject =
            new Subject<(PaletteEditorTreeViewItem<T> item, int newIndex)>();

        private readonly Subject<Empty> _onAddThemeButtonClickedSubject = new Subject<Empty>();

        private readonly Subject<string> _onRemoveThemeButtonClickedSubject = new Subject<string>();

        private readonly Subject<(string themeId, string newName)> _onRenameThemeButtonClickedSubject =
            new Subject<(string themeId, string newName)>();

        private readonly Dictionary<string, string> _themes = new Dictionary<string, string>();

        [NonSerialized] private int _currentItemId;

        protected PaletteEditorTreeView(TreeViewState state) : base(state)
        {
            showAlternatingRowBackgrounds = true;

            rowHeight = EditorGUIUtility.singleLineHeight + 8;

            SetupColumnStates();
        }

        public IObservable<(PaletteEditorTreeViewItem<T> item, int newIndex)> ItemIndexChangedAsObservable =>
            _itemIndexChangedSubject;

        public void AddThemeColumn(string id, string name)
        {
            _themes.Add(id, name);
            SetupColumnStates();
        }

        public void RemoveThemeColumn(string id)
        {
            _themes.Remove(id);
            SetupColumnStates();
        }

        public void ClearThemeColumns()
        {
            _themes.Clear();
            SetupColumnStates();
        }

        public void SetThemeName(string id, string name)
        {
            _themes[id] = name;
            SetupColumnStates();
        }

        public PaletteEditorTreeViewItem<T> AddItem(string entryId, string name, Dictionary<string, T> values)
        {
            var item = new PaletteEditorTreeViewItem<T>(entryId, name, values)
            {
                id = _currentItemId++,
                displayName = name
            };
            _entryIdToItemIdMap.Add(entryId, item.id);
            AddItemAndSetParent(item, -1);
            return item;
        }

        public void RemoveItem(string entryId, bool invokeCallback = true)
        {
            var id = _entryIdToItemIdMap[entryId];
            _entryIdToItemIdMap.Remove(entryId);
            RemoveItem(id, invokeCallback);
        }

        protected override bool CanRename(TreeViewItem item)
        {
            return true;
        }

        protected override void RenameEnded(RenameEndedArgs args)
        {
            if (args.acceptedRename)
            {
                var item = (PaletteEditorTreeViewItem<T>)GetItem(args.itemID);
                item.SetName(args.newName, true);
            }
        }

        protected override void CellGUI(int columnIndex, Rect cellRect, RowGUIArgs args)
        {
            cellRect.height -= 4;
            cellRect.y += 2;
            var item = (PaletteEditorTreeViewItem<T>)args.item;
            var columnCount = ColumnStates.Length;
            if (columnIndex == 0)
            {
                args.rowRect.xMin -= 10;
                args.rowRect.yMin += 5;
                DefaultRowGUI(args);
            }
            else if (columnIndex == columnCount - 1)
            {
                if (GUI.Button(cellRect, new GUIContent("Apply")))
                {
                    item.OnApplyButtonClicked();
                }
            }
            else
            {
                var themeId = _columnIndexToThemeIdMap[columnIndex];
                item.Values[themeId].Value = DrawValueField(cellRect, item.Values[themeId].Value);
            }
        }

        protected abstract T DrawValueField(Rect rect, T value);

        protected override IOrderedEnumerable<TreeViewItem> OrderItems(IList<TreeViewItem> items, int keyColumnIndex,
            bool ascending)
        {
            string KeySelector(TreeViewItem x)
            {
                return GetText((PaletteEditorTreeViewItem<T>)x, keyColumnIndex);
            }

            return ascending
                ? items.OrderBy(KeySelector, Comparer<string>.Create(EditorUtility.NaturalCompare))
                : items.OrderByDescending(KeySelector, Comparer<string>.Create(EditorUtility.NaturalCompare));
        }

        protected override string GetTextForSearch(TreeViewItem item, int columnIndex)
        {
            return GetText((PaletteEditorTreeViewItem<T>)item, columnIndex);
        }

        private static string GetText(PaletteEditorTreeViewItem<T> item, int columnIndex)
        {
            switch (columnIndex)
            {
                case 0:
                    return item.Name.Value;
                default:
                    throw new NotImplementedException();
            }
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return true;
        }

        protected override bool CanBeParent(TreeViewItem item)
        {
            return false;
        }

        protected override bool CanStartDrag(CanStartDragArgs args)
        {
            return string.IsNullOrEmpty(searchString);
        }

        protected override void SetupDragAndDrop(SetupDragAndDropArgs args)
        {
            var selections = args.draggedItemIDs;
            if (selections.Count <= 0)
            {
                return;
            }

            var items = GetRows()
                .Where(i => selections.Contains(i.id))
                .Select(x => (PaletteEditorTreeViewItem<T>)x)
                .ToArray();

            if (items.Length <= 0)
            {
                return;
            }

            DragAndDrop.PrepareStartDrag();
            DragAndDrop.SetGenericData(DragType, items);
            DragAndDrop.StartDrag(items.Length > 1 ? "<Multiple>" : items[0].displayName);
        }

        protected override DragAndDropVisualMode HandleDragAndDrop(DragAndDropArgs args)
        {
            if (args.performDrop)
            {
                var data = DragAndDrop.GetGenericData(DragType);
                var items = (PaletteEditorTreeViewItem<T>[])data;

                if (items == null || items.Length <= 0)
                {
                    return DragAndDropVisualMode.None;
                }

                switch (args.dragAndDropPosition)
                {
                    case DragAndDropPosition.BetweenItems:
                        var afterIndex = args.insertAtIndex;
                        foreach (var item in items)
                        {
                            var itemIndex = RootItem.children.IndexOf(item);
                            if (itemIndex < afterIndex)
                            {
                                afterIndex--;
                            }
                            
                            SetItemIndex(item, afterIndex, true);
                            afterIndex++;
                        }

                        SetSelection(items.Select(x => x.id).ToArray());

                        Reload();
                        break;
                    case DragAndDropPosition.UponItem:
                    case DragAndDropPosition.OutsideItems:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return DragAndDropVisualMode.Move;
        }

        public void SetItemIndex(PaletteEditorTreeViewItem<T> item, int index, bool notify)
        {
            var children = RootItem.children;
            var itemIndex = RootItem.children.IndexOf(item);
            children.RemoveAt(itemIndex);
            children.Insert(index, item);
            if (notify)
            {
                _itemIndexChangedSubject.OnNext((item, index));
            }
        }

        private void SetupColumnStates()
        {
            var columnIndex = 0;
            _columnIndexToThemeIdMap.Clear();
            var columns = new List<MultiColumnHeaderState.Column>();
            var nameColumn = new MultiColumnHeaderState.Column
            {
                headerContent = new GUIContent("Name"),
                headerTextAlignment = TextAlignment.Center,
                canSort = false,
                width = 130,
                minWidth = 50,
                autoResize = false,
                allowToggleVisibility = false
            };
            columns.Add(nameColumn);
            columnIndex++;
            foreach (var theme in _themes)
            {
                var themeId = theme.Key;
                var themeName = theme.Value;
                var valueColumn = new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent(themeName),
                    headerTextAlignment = TextAlignment.Center,
                    canSort = false,
                    width = 150,
                    minWidth = 50,
                    autoResize = false,
                    allowToggleVisibility = false
                };
                _columnIndexToThemeIdMap.Add(columnIndex, themeId);
                columns.Add(valueColumn);
                columnIndex++;
            }

            var applyColumn = new MultiColumnHeaderState.Column
            {
                headerContent = new GUIContent("Apply to Selected"),
                headerTextAlignment = TextAlignment.Center,
                canSort = false,
                width = 120,
                minWidth = 50,
                autoResize = false,
                allowToggleVisibility = false
            };
            columns.Add(applyColumn);
            ColumnStates = columns.ToArray();
        }
    }
}
