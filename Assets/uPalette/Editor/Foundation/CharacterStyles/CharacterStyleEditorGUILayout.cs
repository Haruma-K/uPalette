using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using uPalette.Runtime.Foundation.CharacterStyles;

namespace uPalette.Editor.Foundation.CharacterStyles
{
    public static class CharacterStyleEditorGUILayout
    {
        private static readonly Dictionary<int, CharacterStyle> _changedValues = new Dictionary<int, CharacterStyle>();

        public static CharacterStyle DrawField(Rect rect, CharacterStyle value)
        {
            var controlId = GUIUtility.GetControlID(FocusType.Passive);
            var previewGuiStyle = new GUIStyle(EditorStyles.label);
            var fontName = "(Default)";
            if (value.font != null)
            {
                previewGuiStyle.font = value.font;
                fontName = value.font.name;
            }

            previewGuiStyle.fontStyle = value.fontStyle;
            previewGuiStyle.fontSize = 14;
            var previewRect = rect;
            previewRect.xMin += 4;
            previewRect.width = 26;
            var textRect = rect;
            textRect.xMin += 30;

            if (GUI.Button(rect, string.Empty, EditorStyles.objectField))
            {
                var characterStyleEditor = CharacterStyleEditor.Open(value);
                characterStyleEditor.OnValueChanged += characterStyle =>
                {
                    _changedValues[controlId] = characterStyle;

                    // The value has changed, but the focus is still on the CharacterStyleEditor.
                    // In order to return the updated CharacterStyleField value, the GUI that calls it needs to be repainted.
                    // To do this, repaint all the windows.
                    foreach (var window in Resources.FindObjectsOfTypeAll<EditorWindow>())
                        window.Repaint();
                };
            }

            EditorGUI.LabelField(previewRect, "Ag", previewGuiStyle);
            EditorGUI.LabelField(textRect, $"{fontName} - {value.fontSize}pt");

            if (_changedValues.TryGetValue(controlId, out var changedValue))
            {
                GUI.changed = true;
                value = changedValue;
                _changedValues.Remove(controlId);

                // At this point, the CharacterStyleEditor will be focused.
                // Repaint to update the editor window such as SceneView.
                foreach (var window in Resources.FindObjectsOfTypeAll<EditorWindow>())
                    window.Repaint();
            }

            return value;
        }
    }
}
