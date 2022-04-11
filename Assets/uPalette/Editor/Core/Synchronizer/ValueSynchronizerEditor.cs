using UnityEditor;
using UnityEngine;
using uPalette.Editor.Core.PaletteEditor;
using uPalette.Runtime.Core.Synchronizer;

namespace uPalette.Editor.Core.Synchronizer
{
    public abstract class ValueSynchronizerEditor<T> : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var component = (ValueSynchronizer<T>)target;

            serializedObject.Update();

            var entryIdProperty = serializedObject.FindProperty("_entryId");
            using (var ccs = new EditorGUI.ChangeCheckScope())
            {
                EditorGUILayout.PropertyField(entryIdProperty, new GUIContent("Entry"));

                if (ccs.changed)
                {
                    serializedObject.ApplyModifiedProperties();
                    component.StopObserving();
                    component.StartObserving();
                }
            }


            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Open Palette Editor"))
                PaletteEditorWindow.Open();
        }
    }
}
