using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using uPalette.Editor.Foundation.EasyTreeView;
using uPalette.Runtime.Core;
using uPalette.Runtime.Foundation.Observable;

namespace uPalette.Editor.Core
{
    public class ColorEntryEditorTreeView : TreeViewBase, IDisposable
    {
        private readonly Dictionary<ColorEntry, int> _itemToIdMap = new Dictionary<ColorEntry, int>();

        private readonly Subject<ColorEntryEditorTreeViewItem> _onApplyButtonClickedSubject =
            new Subject<ColorEntryEditorTreeViewItem>();

        private int _currentItemId;

        public ColorEntryEditorTreeView(TreeViewState state) : base(state)
        {
            showAlternatingRowBackgrounds = true;

            rowHeight = EditorGUIUtility.singleLineHeight + 8;

            Reload();
        }

        public bool IsApplyButtonEnabled { get; set; }

        public IObservable<ColorEntryEditorTreeViewItem> OnApplyButtonClickedAsObservable =>
            _onApplyButtonClickedSubject;

        protected override MultiColumnHeaderState.Column[] ColumnStates
        {
            get
            {
                var nameColumn = new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Name"),
                    headerTextAlignment = TextAlignment.Center,
                    canSort = true,
                    width = 130,
                    minWidth = 50,
                    autoResize = false,
                    allowToggleVisibility = false
                };
                var descriptionColumn = new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Color"),
                    headerTextAlignment = TextAlignment.Center,
                    canSort = false,
                    width = 150,
                    minWidth = 50,
                    autoResize = false,
                    allowToggleVisibility = false
                };
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
                return new[] {nameColumn, descriptionColumn, applyColumn};
            }
        }

        public void Dispose()
        {
        }

        public ColorEntryEditorTreeViewItem AddItem(ColorEntry entry)
        {
            var item = new ColorEntryEditorTreeViewItem(entry.ID)
            {
                id = _currentItemId++
            };
            item.Name.SetValueAndNotNotify(entry.Name.Value);
            item.Color.SetValueAndNotNotify(entry.Value.Value);
            _itemToIdMap.Add(entry, item.id);
            AddItemAndSetParent(item, -1);
            return item;
        }

        public void RemoveItem(ColorEntry entry, bool invokeCallback = true)
        {
            var id = _itemToIdMap[entry];
            _itemToIdMap.Remove(entry);
            RemoveItem(id, invokeCallback);
        }

        protected override void CellGUI(int columnIndex, Rect cellRect, RowGUIArgs args)
        {
            cellRect.height -= 4;
            cellRect.y += 2;
            var item = (ColorEntryEditorTreeViewItem) args.item;
            switch ((Columns) columnIndex)
            {
                case Columns.DisplayName:
                    
                    item.Name.Value = EditorGUI.TextField(cellRect, item.Name.Value);
                    break;
                case Columns.Color:
                    item.Color.Value = EditorGUI.ColorField(cellRect, item.Color.Value);
                    break;
                case Columns.Apply:
                    GUI.enabled = IsApplyButtonEnabled;
                    if (GUI.Button(cellRect, new GUIContent("Apply")))
                    {
                        _onApplyButtonClickedSubject.OnNext(item);
                    }

                    GUI.enabled = true;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        protected override IOrderedEnumerable<TreeViewItem> OrderItems(IList<TreeViewItem> items, int keyColumnIndex,
            bool ascending)
        {
            string KeySelector(TreeViewItem x)
            {
                return GetText((ColorEntryEditorTreeViewItem) x, keyColumnIndex);
            }

            return ascending
                ? items.OrderBy(KeySelector, Comparer<string>.Create(EditorUtility.NaturalCompare))
                : items.OrderByDescending(KeySelector, Comparer<string>.Create(EditorUtility.NaturalCompare));
        }

        protected override string GetTextForSearch(TreeViewItem item, int columnIndex)
        {
            return GetText((ColorEntryEditorTreeViewItem) item, columnIndex);
        }

        private static string GetText(ColorEntryEditorTreeViewItem item, int columnIndex)
        {
            switch ((Columns) columnIndex)
            {
                case Columns.DisplayName:
                    return item.Name.Value;
                default:
                    throw new NotImplementedException();
            }
        }

        private enum Columns
        {
            DisplayName,
            Color,
            Apply
        }
    }
}