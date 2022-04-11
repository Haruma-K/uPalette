using UnityEditor;
using UnityEngine;
using uPalette.Editor.Core.PaletteEditor;
using uPalette.Runtime.Core.Synchronizer;

namespace uPalette.Editor.Core.Synchronizer
{
    public abstract class ValueSynchronizeEventEditor<T> : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var component = (ValueSynchronizeEvent<T>)target;

            serializedObject.Update();

            var entryIdProperty = serializedObject.FindProperty("_entryId");
            var valueChangedProperty = serializedObject.FindProperty("_valueChanged");
            using (var ccs = new EditorGUI.ChangeCheckScope())
            {
                EditorGUILayout.PropertyField(entryIdProperty, new GUIContent("Entry"));

                if (ccs.changed)
                {
                    component.StopObserving();
                    component.StartObserving();
                }
            }

            EditorGUILayout.PropertyField(valueChangedProperty);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Open Palette Editor"))
                PaletteEditorWindow.Open();
        }
    }
}
