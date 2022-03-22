using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using uPalette.Editor.Foundation.EasyTreeView;
using uPalette.Runtime.Foundation.TinyRx;

namespace uPalette.Editor.Core.ThemeEditor
{
    internal class ThemeEditorTreeView : TreeViewBase
    {
        private const string DragType = "ThemeEditorTreeView";

        private readonly Subject<(ThemeEditorTreeViewItem item, int newIndex)> _itemIndexChangedSubject =
            new Subject<(ThemeEditorTreeViewItem item, int newIndex)>();

        private readonly Dictionary<string, int> _themeIdToItemIdMap = new Dictionary<string, int>();

        [NonSerialized] private int _currentItemId;

        public ThemeEditorTreeView(TreeViewState state) : base(state)
        {
            showAlternatingRowBackgrounds = true;
            rowHeight = EditorGUIUtility.singleLineHeight + 8;

            SetupColumnStates();
        }

        public IObservable<(ThemeEditorTreeViewItem item, int newIndex)> ItemIndexChangedAsObservable =>
            _itemIndexChangedSubject;

        public ThemeEditorTreeViewItem AddItem(string themeId, string name, bool isActive)
        {
            var item = new ThemeEditorTreeViewItem(themeId, name, isActive)
            {
                id = _currentItemId++
            };
            
            _themeIdToItemIdMap.Add(themeId, item.id);
            AddItemAndSetParent(item, -1);
            return item;
        }

        public void RemoveItem(string themeId, bool invokeCallback = true)
        {
            var itemId = _themeIdToItemIdMap[themeId];
            _themeIdToItemIdMap.Remove(themeId);
            RemoveItem(itemId, invokeCallback);
        }

        protected override bool CanRename(TreeViewItem item)
        {
            return true;
        }

        protected override void RenameEnded(RenameEndedArgs args)
        {
            if (args.acceptedRename)
            {
                var item = (ThemeEditorTreeViewItem)GetItem(args.itemID);
                item.SetName(args.newName, true);
            }
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = (ThemeEditorTreeViewItem)args.item;
            if (item.IsActive.Value)
            {
                var color = new Color(76.0f / 255.0f, 197.0f / 255.0f, 68.0f / 255.0f, 1.0f);
                var rect = args.rowRect;
                rect.width = 4;
                EditorGUI.DrawRect(rect, color);
            }
            base.RowGUI(args);
        }

        protected override void CellGUI(int columnIndex, Rect cellRect, RowGUIArgs args)
        {
            cellRect.height -= 4;
            cellRect.y += 2;
            var item = (ThemeEditorTreeViewItem)args.item;
            switch ((Columns)columnIndex)
            {
                case Columns.DisplayName:
                    args.rowRect.xMin -= 5;
                    args.rowRect.yMin += 5;
                    DefaultRowGUI(args);
                    break;
                case Columns.ActivateButton:
                    GUI.enabled = !item.IsActive.Value;
                    var guiContent = item.IsActive.Value
                        ? new GUIContent("Active")
                        : new GUIContent("Activate");
                    if (GUI.Button(cellRect, guiContent))
                    {
                        SetActiveTheme(item.ThemeId);
                    }
                    GUI.enabled = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(columnIndex), columnIndex, null);
            }
        }

        protected override IOrderedEnumerable<TreeViewItem> OrderItems(IList<TreeViewItem> items, int keyColumnIndex,
            bool ascending)
        {
            string KeySelector(TreeViewItem x)
            {
                return GetText((ThemeEditorTreeViewItem)x, keyColumnIndex);
            }

            return ascending
                ? items.OrderBy(KeySelector, Comparer<string>.Create(EditorUtility.NaturalCompare))
                : items.OrderByDescending(KeySelector, Comparer<string>.Create(EditorUtility.NaturalCompare));
        }

        protected override string GetTextForSearch(TreeViewItem item, int columnIndex)
        {
            return GetText((ThemeEditorTreeViewItem)item, columnIndex);
        }

        private static string GetText(ThemeEditorTreeViewItem item, int columnIndex)
        {
            switch ((Columns)columnIndex)
            {
                case Columns.DisplayName:
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
            if (selections.Count <= 0) return;

            var items = GetRows()
                .Where(i => selections.Contains(i.id))
                .Select(x => (ThemeEditorTreeViewItem)x)
                .ToArray();

            if (items.Length <= 0) return;

            DragAndDrop.PrepareStartDrag();
            DragAndDrop.SetGenericData(DragType, items);
            DragAndDrop.StartDrag(items.Length > 1 ? "<Multiple>" : items[0].displayName);
        }

        protected override DragAndDropVisualMode HandleDragAndDrop(DragAndDropArgs args)
        {
            if (args.performDrop)
            {
                var data = DragAndDrop.GetGenericData(DragType);
                var items = (ThemeEditorTreeViewItem[])data;

                if (items == null || items.Length <= 0) return DragAndDropVisualMode.None;

                switch (args.dragAndDropPosition)
                {
                    case DragAndDropPosition.BetweenItems:
                        var afterIndex = args.insertAtIndex;
                        foreach (var item in items)
                        {
                            var itemIndex = RootItem.children.IndexOf(item);
                            if (itemIndex < afterIndex) afterIndex--;

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

        public void SetItemIndex(ThemeEditorTreeViewItem item, int index, bool notify)
        {
            var children = RootItem.children;
            var itemIndex = RootItem.children.IndexOf(item);
            children.RemoveAt(itemIndex);
            children.Insert(index, item);
            if (notify) _itemIndexChangedSubject.OnNext((item, index));
        }

        private void SetActiveTheme(string themeId)
        {
            foreach (var treeViewItem in GetRows())
            {
                var item = (ThemeEditorTreeViewItem)treeViewItem;
                item.IsActive.Value = themeId == item.ThemeId;
            }
        }

        private void SetupColumnStates()
        {
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

            var applyColumn = new MultiColumnHeaderState.Column
            {
                headerContent = new GUIContent("Activate"),
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

        private enum Columns
        {
            DisplayName,
            ActivateButton
        }
    }
}
