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

            var iter = serializedObject.GetIterator();
            iter.NextVisible(true);
            while (iter.NextVisible(false))
                if (iter.name == "_entryId")
                {
                    using var ccs = new EditorGUI.ChangeCheckScope();
                    EditorGUILayout.PropertyField(iter, new GUIContent("Entry"));

                    if (!ccs.changed)
                        continue;

                    serializedObject.ApplyModifiedProperties();
                    component.StopObserving();
                    component.StartObserving();
                }
                else
                {
                    EditorGUILayout.PropertyField(iter, true);
                }


            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Open Palette Editor"))
                PaletteEditorWindow.Open();
        }
    }
}