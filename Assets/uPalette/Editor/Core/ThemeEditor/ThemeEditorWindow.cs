using System;
using UnityEditor;
using UnityEngine;
using uPalette.Editor.Core.Shared;
using uPalette.Runtime.Foundation.TinyRx;

namespace uPalette.Editor.Core.ThemeEditor
{
    internal sealed class ThemeEditorWindow : EditorWindow
    {
        public enum Mode
        {
            Empty,
            Contents
        }

        public const string Title = "Theme Editor";

        [SerializeField] private ThemeEditorWindowContentsView _colorContentsView = new ThemeEditorWindowContentsView();

        [SerializeField] private ThemeEditorWindowContentsView _gradientContentsView = new ThemeEditorWindowContentsView();

        [SerializeField]
        private ThemeEditorWindowContentsView _characterStyleContentsView = new ThemeEditorWindowContentsView();
        
        [SerializeField]
        private ThemeEditorWindowContentsView _characterStyleTMPContentsView = new ThemeEditorWindowContentsView();

        private readonly Subject<Empty> _createButtonClickedSubject = new Subject<Empty>();

        private readonly ThemeEditorWindowEmptyView _emptyView = new ThemeEditorWindowEmptyView();
        private readonly Subject<Empty> _redoShortcutExecutedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _removeShortcutExecutedSubject = new Subject<Empty>();
        private readonly Subject<PaletteType> _selectedPaletteTypeChangedSubject = new Subject<PaletteType>();
        private readonly Subject<Empty> _undoShortcutExecutedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _lostFocusSubject = new Subject<Empty>();

        private PaletteType _activePaletteType;
        private Mode _mode;

        private ThemeEditorWindowContentsView _activeWindowContentsView;

        private UPaletteEditorApplication _application;
        private GUIContent _toolbarPlusIconContent;
        public IObservable<Empty> CreateButtonClickedAsObservable => _createButtonClickedSubject;
        public IObservable<Empty> UndoShortcutExecutedAsObservable => _undoShortcutExecutedSubject;
        public IObservable<Empty> RedoShortcutExecutedAsObservable => _redoShortcutExecutedSubject;
        public IObservable<Empty> RemoveShortcutExecutedAsObservable => _removeShortcutExecutedSubject;
        public IObservable<Empty> LostFocusAsObservable => _lostFocusSubject;
        public IObservable<PaletteType> SelectedPaletteTypeChangedAsObservable => _selectedPaletteTypeChangedSubject;

        public ThemeEditorWindowContentsView ColorContentsView => _colorContentsView;
        public ThemeEditorWindowContentsView GradientContentsView => _gradientContentsView;
        public ThemeEditorWindowContentsView CharacterStyleContentsView => _characterStyleContentsView;
        public ThemeEditorWindowContentsView CharacterStyleTMPContentsView => _characterStyleTMPContentsView;
        public ThemeEditorWindowEmptyView EmptyView => _emptyView;

        private void OnEnable()
        {
            var application = UPaletteEditorApplication.RequestInstance();
            _application = application;
            _toolbarPlusIconContent = EditorGUIUtility.IconContent("d_Toolbar Plus");

            _colorContentsView.Setup();
            _gradientContentsView.Setup();
            _characterStyleContentsView.Setup();
            _characterStyleTMPContentsView.Setup();

            application.SetupThemeEditor(this);
        }

        private void OnDisable()
        {
            _application.CleanupThemeEditor();
            UPaletteEditorApplication.ReleaseInstance();
        }

        private void OnLostFocus()
        {
            _lostFocusSubject.OnNext(Empty.Default);
        }

        private void OnDestroy()
        {
            _createButtonClickedSubject.Dispose();
            _undoShortcutExecutedSubject.Dispose();
            _redoShortcutExecutedSubject.Dispose();

            _colorContentsView.Dispose();
            _gradientContentsView.Dispose();
            _characterStyleContentsView.Dispose();
            _characterStyleTMPContentsView.Dispose();
            _emptyView.Dispose();
        }

        private void OnGUI()
        {
            switch (_mode)
            {
                case Mode.Empty:
                    DrawEmptyView();
                    break;
                case Mode.Contents:
                    DrawContentView();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DrawEmptyView()
        {
            _emptyView.OnGUI();
        }

        private void DrawContentView()
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

            if (GetEventAction(e) && e.type == EventType.KeyDown && e.keyCode == KeyCode.Delete)
            {
                _removeShortcutExecutedSubject.OnNext(Empty.Default);
                e.Use();
            }

            // Toolbar
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                using (var ccs = new EditorGUI.ChangeCheckScope())
                {
                    var selectedType = (PaletteType)EditorGUILayout.EnumPopup(_activePaletteType,
                        EditorStyles.toolbarPopup, GUILayout.MaxWidth(80));
                    if (ccs.changed) _selectedPaletteTypeChangedSubject.OnNext(selectedType);
                }

                GUILayout.Space(100);
                GUILayout.FlexibleSpace();

                _activeWindowContentsView.DrawSearchFieldToolbarGUI();

                if (GUILayout.Button(_toolbarPlusIconContent, EditorStyles.toolbarButton))
                    _createButtonClickedSubject.OnNext(Empty.Default);
            }

            // Draw the tree view.
            var toolbarHeight = EditorStyles.toolbar.fixedHeight;
            var treeViewRect = EditorGUILayout.GetControlRect(false);
            treeViewRect.height = position.height - toolbarHeight;
            _activeWindowContentsView.DrawTreeViewGUI(treeViewRect);
        }

        public void SetMode(Mode mode)
        {
            _mode = mode;
        }

        public void SetActiveContentView(PaletteType paletteType)
        {
            switch (paletteType)
            {
                case PaletteType.Color:
                    _activeWindowContentsView = _colorContentsView;
                    break;
                case PaletteType.Gradient:
                    _activeWindowContentsView = _gradientContentsView;
                    break;
                case PaletteType.CharacterStyle:
                    _activeWindowContentsView = _characterStyleContentsView;
                    break;
                case PaletteType.CharacterStyleTMP:
                    _activeWindowContentsView = _characterStyleTMPContentsView;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(paletteType), paletteType, null);
            }

            _activePaletteType = paletteType;
            
            Repaint();
        }

        [MenuItem("Window/uPalette/Theme Editor")]
        public static void Open()
        {
            var window = GetWindow<ThemeEditorWindow>();
            window.titleContent = new GUIContent(Title);
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
