using System.Linq;
using UnityEditor;
using UnityEngine;
using uPalette.Runtime.Core;

namespace uPalette.Editor.Core
{
    [CustomPropertyDrawer(typeof(ColorSetter.EntryId))]
    public class EntryIdDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var application = UPaletteApplication.RequestInstance();

            try
            {
                using (new EditorGUI.PropertyScope(position, label, property))
                {
                    var valueProp = property.FindPropertyRelative("_value");
                    var entryId = valueProp.stringValue;
                    var entries = application.UPaletteStore.Entries;
                    var entry = entries.FirstOrDefault(x => x.ID.Equals(entryId));
                    EditorGUI.LabelField(position, new GUIContent("Color"),
                        new GUIContent(entry?.Name.Value ?? "None"));
                }
            }
            finally
            {
                UPaletteApplication.ReleaseInstance();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}