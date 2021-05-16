using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using uPalette.Editor.Foundation.EasyTreeView;
using uPalette.Runtime.Foundation.Observable;

namespace uPalette.Editor.Core
{
    public class UPaletteEditorWindow : EditorWindow
    {
        public const string Title = "uPalette";
        [SerializeField] private TreeViewState _treeViewState;

        private readonly Subject<Empty> _createButtonClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _redoShortcutExecutedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _rightClickRemoveMenuClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _rightClickHighlightMenuClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _undoShortcutExecutedSubject = new Subject<Empty>();
        private UPaletteEditorApplication _application;

        private TreeViewSearchField _searchField;
        internal ColorEntryEditorTreeView TreeView { get; private set; }

        public IObservable<Empty> CreateButtonClickedAsObservable => _createButtonClickedSubject;
        public IObservable<Empty> RightClickHighlightMenuClickedAsObservable => _rightClickHighlightMenuClickedSubject;
        public IObservable<Empty> RightClickRemoveMenuClickedAsObservable => _rightClickRemoveMenuClickedSubject;
        public IObservable<Empty> UndoShortcutExecutedAsObservable => _undoShortcutExecutedSubject;
        public IObservable<Empty> RedoShortcutExecutedAsObservable => _redoShortcutExecutedSubject;

        private void OnEnable()
        {
            if (_treeViewState == null)
            {
                _treeViewState = new TreeViewState();
            }

            // Create the tree view.
            TreeView = new ColorEntryEditorTreeView(_treeViewState);

            // Create the search field.
            _searchField = new TreeViewSearchField(TreeView, 0);

            // Add the right click menu.
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Remove"), false,
                () => _rightClickRemoveMenuClickedSubject.OnNext(Empty.Default));
            menu.AddItem(new GUIContent("Highlight"), false,
                () => _rightClickHighlightMenuClickedSubject.OnNext(Empty.Default));
            menu.ShowAsContext();
            TreeView.RightClickMenu = menu;

            _application = UPaletteEditorApplication.RequestInstance();
            _application.ColorEntryEditorController.Setup(this);
            _application.ColorEntryEditorPresenter.Setup(this);
        }

        private void OnDisable()
        {
            _application.ColorEntryEditorController.Clear();
            _application.ColorEntryEditorPresenter.Clear();
            UPaletteEditorApplication.ReleaseInstance();
        }

        private void OnDestroy()
        {
            _createButtonClickedSubject.Dispose();
            _rightClickRemoveMenuClickedSubject.Dispose();
            _undoShortcutExecutedSubject.Dispose();
            _redoShortcutExecutedSubject.Dispose();

            TreeView.Dispose();
        }

        private void OnGUI()
        {
            // Shortcut
            var e = Event.current;
            if (GetEventAction(e) && e.type == EventType.KeyDown && e.keyCode == KeyCode.Z)
            {
                _undoShortcutExecutedSubject.OnNext(Empty.Default);
                e.Use();
            }

            if (GetEventAction(e) && e.type == EventType.KeyDown && e.keyCode == KeyCode.Y)
            {
                _redoShortcutExecutedSubject.OnNext(Empty.Default);
                e.Use();
            }

            // Toolbar
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                if (GUILayout.Button("Create", EditorStyles.toolbarButton, GUILayout.Width(80)))
                {
                    _createButtonClickedSubject.OnNext(Empty.Default);
                }

                GUILayout.Space(100);
                GUILayout.FlexibleSpace();
                _searchField.OnToolbarGUI();
            }

            // Draw the tree view.
            var toolbarHeight = EditorStyles.toolbar.fixedHeight;
            var treeViewRect = EditorGUILayout.GetControlRect(false);
            treeViewRect.height = position.height - toolbarHeight;
            TreeView.OnGUI(treeViewRect);
        }

        [MenuItem("Window/uPalette")]
        public static void Open()
        {
            GetWindow<UPaletteEditorWindow>(Title);
        }

        public void SetTitleState(bool isDirty)
        {
            var currentTitle = Title;
            if (isDirty)
            {
                currentTitle = $"* {Title}";
            }

            titleContent = new GUIContent(currentTitle);
        }

        private bool GetEventAction(Event e)
        {
#if UNITY_EDITOR_WIN
            return e.control;
#else
            return e.command;
#endif
        }
    }
}