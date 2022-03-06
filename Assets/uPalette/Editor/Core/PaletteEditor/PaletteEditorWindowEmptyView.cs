using System;
using UnityEditor;
using UnityEngine;
using uPalette.Runtime.Core;
using uPalette.Runtime.Foundation.TinyRx;

namespace uPalette.Editor.Core.PaletteEditor
{
    internal sealed class PaletteEditorWindowEmptyView
    {
        private readonly Subject<Empty> _createButtonClickedSubject = new Subject<Empty>();

        public IObservable<Empty> CreateButtonClickedAsObservable => _createButtonClickedSubject;

        public void OnGUI()
        {
            var storeDisplayName = ObjectNames.NicifyVariableName(nameof(PaletteStore));
            var editorDisplayName = ObjectNames.NicifyVariableName(nameof(PaletteEditor));
            using (new EditorGUILayout.VerticalScope())
            {
                GUILayout.FlexibleSpace();
                using (new EditorGUILayout.HorizontalScope())
                {
                    var style = new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.MiddleCenter
                    };
                    EditorGUILayout.LabelField($"Create {storeDisplayName} to use {editorDisplayName}.", style,
                        GUILayout.ExpandWidth(true));
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button($"Create {storeDisplayName}"))
                        _createButtonClickedSubject.OnNext(Empty.Default);

                    GUILayout.FlexibleSpace();
                }

                GUILayout.FlexibleSpace();
            }
        }

        public void Dispose()
        {
            _createButtonClickedSubject.Dispose();
        }
    }
}
