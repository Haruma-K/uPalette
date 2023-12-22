using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using uPalette.Editor.Foundation.EasyTreeView;
using uPalette.Runtime.Foundation.TinyRx;

namespace uPalette.Editor.Core.PaletteEditor
{
    [Serializable]
    internal abstract class PaletteEditorWindowContentsView<T> : IPaletteEditorWindowContentsView, IDisposable
    {
        [SerializeField] private TreeViewState _state = new TreeViewState();
        private readonly Subject<Empty> _rightClickCreateMenuClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _rightClickHighlightMenuClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _rightClickRemoveMenuClickedSubject = new Subject<Empty>();

        private TreeViewSearchField _searchField;

        public PaletteEditorTreeView<T> TreeView { get; private set; }

        public IObservable<Empty> RightClickCreateMenuClickedAsObservable => _rightClickCreateMenuClickedSubject;
        public IObservable<Empty> RightClickRemoveMenuClickedAsObservable => _rightClickRemoveMenuClickedSubject;
        public IObservable<Empty> RightClickHighlightMenuClickedAsObservable => _rightClickHighlightMenuClickedSubject;

        public void Dispose()
        {
            _rightClickCreateMenuClickedSubject.Dispose();
            _rightClickRemoveMenuClickedSubject.Dispose();
            _rightClickHighlightMenuClickedSubject.Dispose();
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
            TreeView = CreateTreeView(_state);
            _searchField = new TreeViewSearchField(TreeView, 0);
            AddContextMenuToTreeView(TreeView);
        }

        public void SetFolderMode(bool folderMode, bool reload)
        {
            TreeView.SetFolderMode(folderMode, reload);
        }

        private void AddContextMenuToTreeView(TreeViewBase treeView)
        {
            treeView.RightClickMenuRequested = () =>
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Create"), false,
                    () => _rightClickCreateMenuClickedSubject.OnNext(Empty.Default));
                if (treeView.HasSelection())
                {
                    menu.AddItem(new GUIContent("Remove"), false,
                        () => _rightClickRemoveMenuClickedSubject.OnNext(Empty.Default));
                    menu.AddItem(new GUIContent("Highlight"), false,
                        () => _rightClickHighlightMenuClickedSubject.OnNext(Empty.Default));
                }
                else
                {
                    menu.AddDisabledItem(new GUIContent("Remove"));
                    menu.AddDisabledItem(new GUIContent("Highlight"));
                }
                return menu;
            };
        }

        protected abstract PaletteEditorTreeView<T> CreateTreeView(TreeViewState state);
    }
}
