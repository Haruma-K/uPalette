using UnityEditor;
using UnityEngine;
using uPalette.Editor.Core.PaletteEditor;
using uPalette.Runtime.Core;
using uPalette.Runtime.Core.Synchronizer;

namespace uPalette.Editor.Core.Shared
{
    public abstract class ValueSynchronizerEditor<T> : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var valueSynchronizer = (ValueSynchronizer<T>)target;

            var entryIdProperty = serializedObject.FindProperty("_entryId");
            var entryId = entryIdProperty.stringValue;
            var entryName = "(Not Found)";
            var themeName = "(Not Found)";

            var store = PaletteStore.Instance;
            if (store != null)
            {
                var palette = valueSynchronizer.GetPalette(store);
                if (palette.Entries.TryGetValue(entryId, out var entry))
                    entryName = entry.Name.Value;

                themeName = palette.ActiveTheme.Value.Name.Value;
            }

            EditorGUILayout.LabelField("Theme Name", themeName);
            EditorGUILayout.LabelField("Entry Name", entryName);
            EditorGUILayout.LabelField("Entry Id", entryId);
            if (GUILayout.Button("Open Palette Editor")) PaletteEditorWindow.Open();
        }
    }
}
