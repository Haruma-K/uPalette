using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using uPalette.Runtime.Foundation.CharacterStyles;

namespace uPalette.Editor.Foundation.CharacterStyles
{
    public static class CharacterStyleTMPEditorGUILayout
    {
        private static readonly Dictionary<int, CharacterStyleTMP> _changedValues =
            new Dictionary<int, CharacterStyleTMP>();

        public static CharacterStyleTMP DrawField(Rect rect, CharacterStyleTMP value)
        {
            var controlId = GUIUtility.GetControlID(FocusType.Passive);
            var previewGuiStyle = new GUIStyle(EditorStyles.label);
            var fontName = "(Default)";
            if (value.font != null)
            {
                previewGuiStyle.font = value.font.sourceFontFile;
                fontName = value.font.name;
            }

            previewGuiStyle.fontStyle = FontStylesToFontStyle(value.fontStyle);
            previewGuiStyle.fontSize = 14;
            previewGuiStyle.richText = true;
            var previewRect = rect;
            previewRect.xMin += 4;
            previewRect.width = 26;
            var textRect = rect;
            textRect.xMin += 30;

            var previewLabel = "Ag";
            if ((value.fontStyle & FontStyles.LowerCase) != 0)
                previewLabel = "ag";
            else if ((value.fontStyle & FontStyles.UpperCase) != 0)
                previewLabel = "AG";
            else if ((value.fontStyle & FontStyles.SmallCaps) != 0)
                previewLabel = "A<size=11>G</size>";

            if (GUI.Button(rect, string.Empty, EditorStyles.objectField))
            {
                var characterStyleTMPEditor = CharacterStyleTMPEditor.Open(value);
                characterStyleTMPEditor.OnValueChanged += characterStyle =>
                {
                    _changedValues[controlId] = characterStyle;

                    // The value has changed, but the focus is still on the CharacterStyleEditor.
                    // In order to return the updated CharacterStyleField value, the GUI that calls it needs to be repainted.
                    // To do this, repaint all the windows.
                    foreach (var window in Resources.FindObjectsOfTypeAll<EditorWindow>())
                        window.Repaint();
                };
            }

            if ((value.fontStyle & FontStyles.Underline) != 0)
            {
                var underLineRect = previewRect;
                underLineRect.y += underLineRect.height - 4;
                underLineRect.height = 1;
                underLineRect.width -= 4;
                EditorGUI.DrawRect(underLineRect, GUI.skin.label.normal.textColor);
            }

            EditorGUI.LabelField(previewRect, previewLabel, previewGuiStyle);
            EditorGUI.LabelField(textRect, $"{fontName} - {value.fontSize}pt");

            if ((value.fontStyle & FontStyles.Strikethrough) != 0)
            {
                var strikethroughLineRect = previewRect;
                strikethroughLineRect.y += strikethroughLineRect.height / 2;
                strikethroughLineRect.height = 1;
                strikethroughLineRect.width -= 4;
                EditorGUI.DrawRect(strikethroughLineRect, GUI.skin.label.normal.textColor);
            }

            if (_changedValues.TryGetValue(controlId, out var changedValue))
            {
                GUI.changed = true;
                value = changedValue;
                _changedValues.Remove(controlId);
            }

            return value;
        }

        private static FontStyle FontStylesToFontStyle(FontStyles styles)
        {
            if ((styles & FontStyles.Bold) != 0 && (styles & FontStyles.Italic) != 0)
                return FontStyle.BoldAndItalic;

            if ((styles & FontStyles.Bold) != 0)
                return FontStyle.Bold;

            if ((styles & FontStyles.Italic) != 0)
                return FontStyle.Italic;

            return FontStyle.Normal;
        }
    }
}
