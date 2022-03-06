using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using uPalette.Editor.Foundation.EasyTreeView;
using uPalette.Runtime.Foundation.TinyRx;

namespace uPalette.Editor.Core.ThemeEditor
{
    [Serializable]
    internal class ThemeEditorWindowContentsView : IDisposable
    {
        [SerializeField] private TreeViewState _state = new TreeViewState();
        private readonly Subject<Empty> _rightClickCreateMenuClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _rightClickRemoveMenuClickedSubject = new Subject<Empty>();

        private TreeViewSearchField _searchField;

        public ThemeEditorTreeView TreeView { get; private set; }

        public IObservable<Empty> RightClickCreateMenuClickedAsObservable => _rightClickCreateMenuClickedSubject;
        public IObservable<Empty> RightClickRemoveMenuClickedAsObservable => _rightClickRemoveMenuClickedSubject;

        public void Dispose()
        {
            _rightClickCreateMenuClickedSubject.Dispose();
            _rightClickRemoveMenuClickedSubject.Dispose();
        }

        public void DrawSearchFieldToolbarGUI()
        {
            _searchField.OnToolbarGUI();
        }

        public void DrawTreeViewGUI(Rect rect)
        {
            TreeView.OnGUI(rect);
        }

        public void Setup()
        {
            TreeView = new ThemeEditorTreeView(_state);
            _searchField = new TreeViewSearchField(TreeView, 0);
            AddContextMenuToTreeView(TreeView);
        }

        private void AddContextMenuToTreeView(TreeViewBase treeView)
        {
            treeView.RightClickMenuRequested = () =>
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Create"), false,
                    () => _rightClickCreateMenuClickedSubject.OnNext(Empty.Default));
                if (treeView.HasSelection() && treeView.GetRows().Count >= 2)
                    menu.AddItem(new GUIContent("Remove"), false,
                        () => _rightClickRemoveMenuClickedSubject.OnNext(Empty.Default));
                else
                    menu.AddDisabledItem(new GUIContent("Remove"));
                return menu;
            };
        }
    }
}
