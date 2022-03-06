using System;
using UnityEditor;
using UnityEngine;
using uPalette.Runtime.Foundation.CharacterStyles;

namespace uPalette.Editor.Foundation.CharacterStyles
{
    public sealed class CharacterStyleEditor : EditorWindow
    {
        private const float WindowWidth = 300.0f;

        private CharacterStyle _characterStyle;

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            var fontDisplayName = ObjectNames.NicifyVariableName(nameof(_characterStyle.font));
            var fontStyleDisplayName = ObjectNames.NicifyVariableName(nameof(_characterStyle.fontStyle));
            var fontSizeDisplayName = ObjectNames.NicifyVariableName(nameof(_characterStyle.fontSize));
            var lineSpacingDisplayName = ObjectNames.NicifyVariableName(nameof(_characterStyle.lineSpacing));

            _characterStyle.font =
                (Font)EditorGUILayout.ObjectField(fontDisplayName, _characterStyle.font, typeof(Font), false);
            _characterStyle.fontStyle =
                (FontStyle)EditorGUILayout.EnumPopup(fontStyleDisplayName, _characterStyle.fontStyle);
            _characterStyle.fontSize = EditorGUILayout.IntField(fontSizeDisplayName, _characterStyle.fontSize);
            _characterStyle.lineSpacing =
                EditorGUILayout.FloatField(lineSpacingDisplayName, _characterStyle.lineSpacing);

            if (EditorGUI.EndChangeCheck())
                OnValueChanged?.Invoke(_characterStyle);
        }

        public event Action<CharacterStyle> OnValueChanged;

        private void Setup(CharacterStyle characterStyle)
        {
            _characterStyle = characterStyle;
            titleContent = new GUIContent(ObjectNames.NicifyVariableName(nameof(CharacterStyle)));
            var size = new Vector2(WindowWidth, GetHeight());
            minSize = maxSize = size;
        }

        private static float GetHeight()
        {
            var height = 0.0f;

            // Font
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.standardVerticalSpacing;
            // Font Style
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.standardVerticalSpacing;
            // Font Size
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.standardVerticalSpacing;
            // Line Spacing
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.standardVerticalSpacing;
            // Padding
            height += 10;

            return height;
        }

        public static CharacterStyleEditor Open(CharacterStyle characterStyle)
        {
            var window = CreateInstance<CharacterStyleEditor>();
            window.Setup(characterStyle);
            window.ShowAuxWindow();
            return window;
        }
    }
}
