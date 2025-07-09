using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using uPalette.Runtime.Foundation.CharacterStyles;

namespace uPalette.Editor.Foundation.CharacterStyles
{
    public sealed class CharacterStyleTMPEditor : EditorWindow
    {
        private const float WindowWidth = 300.0f;

        private CharacterStyleTMP _characterStyle;

        private int _toolbarValue;

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            var fontDisplayName = "Font Asset";
            var fontStyleDisplayName = ObjectNames.NicifyVariableName(nameof(_characterStyle.fontStyle));
            var fontSizeDisplayName = ObjectNames.NicifyVariableName(nameof(_characterStyle.fontSize));
            var enableAutoSizingDisplayName = "Auto Size";
            var fontSizeMinDisplayName = "Min";
            var fontSizeMaxDisplayName = "Max";
            var characterWidthAdjustmentDisplayName = "WD%";
            var lineSpacingAdjustmentDisplayName = "Line";
            var characterSpacingDisplayName = "Character";
            var wordSpacingDisplayName = "Word";
            var lineSpacingDisplayName = "Line";
            var paragraphSpacingDisplayName = "Paragraph";
            var fontSharedMaterialDisplayName = "Font Material";

            _characterStyle.font = (TMP_FontAsset)EditorGUILayout.ObjectField(fontDisplayName, _characterStyle.font,
                typeof(TMP_FontAsset), false);
            _characterStyle.fontStyle = FontStylesField(_characterStyle.fontStyle);
            _characterStyle.fontSize = EditorGUILayout.FloatField(fontSizeDisplayName, _characterStyle.fontSize);
            EditorGUI.indentLevel++;
            _characterStyle.enableAutoSizing =
                EditorGUILayout.Toggle(enableAutoSizingDisplayName, _characterStyle.enableAutoSizing);
            if (_characterStyle.enableAutoSizing)
            {
                GUI.enabled = _characterStyle.enableAutoSizeOptions;
                EditorGUI.indentLevel++;
                var min = EditorGUILayout.FloatField(fontSizeMinDisplayName, _characterStyle.fontSizeMin);
                _characterStyle.fontSizeMin = Mathf.Min(min, _characterStyle.fontSizeMax);
                var max = EditorGUILayout.FloatField(fontSizeMaxDisplayName, _characterStyle.fontSizeMax);
                _characterStyle.fontSizeMax = Mathf.Max(max, _characterStyle.fontSizeMin);
                var wd = EditorGUILayout.FloatField(characterWidthAdjustmentDisplayName,
                    _characterStyle.characterWidthAdjustment);
                _characterStyle.characterWidthAdjustment = Mathf.Max(wd, 0.0f);
                var line = EditorGUILayout.FloatField(lineSpacingAdjustmentDisplayName,
                    _characterStyle.lineSpacingAdjustment);
                _characterStyle.lineSpacingAdjustment = Mathf.Min(line, 0.0f);
                EditorGUI.indentLevel--;
                GUI.enabled = true;
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.LabelField("Spacing Options (em)");
            EditorGUI.indentLevel++;
            _characterStyle.characterSpacing =
                EditorGUILayout.FloatField(characterSpacingDisplayName, _characterStyle.characterSpacing);
            _characterStyle.wordSpacing =
                EditorGUILayout.FloatField(wordSpacingDisplayName, _characterStyle.wordSpacing);
            _characterStyle.lineSpacing =
                EditorGUILayout.FloatField(lineSpacingDisplayName, _characterStyle.lineSpacing);
            _characterStyle.paragraphSpacing =
                EditorGUILayout.FloatField(paragraphSpacingDisplayName, _characterStyle.paragraphSpacing);
            EditorGUI.indentLevel--;

            // フォントマテリアル
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(fontSharedMaterialDisplayName);

            if (_characterStyle.fontSharedMaterial == null)
            {
                // マテリアルが設定されていない場合のプレースホルダー表示
                var placeholderStyle = new GUIStyle(EditorStyles.objectField);
                placeholderStyle.normal.textColor = Color.gray;

                var defaultMaterialName = "(Default)";

                if (GUILayout.Button(defaultMaterialName, placeholderStyle))
                    // ObjectFieldと同じようにマテリアル選択ウィンドウを開く
                    EditorGUIUtility.ShowObjectPicker<Material>(null,
                        false,
                        string.Empty,
                        GUIUtility.GetControlID(FocusType.Passive));

                // ObjectPickerからの選択を処理
                if (Event.current.commandName == "ObjectSelectorUpdated")
                {
                    var pickedObject = EditorGUIUtility.GetObjectPickerObject();
                    if (pickedObject is Material material)
                    {
                        _characterStyle.fontSharedMaterial = material;
                        GUI.changed = true;
                    }
                }
            }
            else
            {
                _characterStyle.fontSharedMaterial = (Material)EditorGUILayout.ObjectField(
                    _characterStyle.fontSharedMaterial,
                    typeof(Material),
                    false);
            }

            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
                OnValueChanged?.Invoke(_characterStyle);
        }

        private FontStyles FontStylesField(FontStyles styles)
        {
            var styleRect = GUILayoutUtility.GetRect(WindowWidth, EditorGUIUtility.singleLineHeight);
            styleRect.xMin += 2;
            styleRect.xMax -= 2;
            var styleLabelRect = styleRect;
            styleLabelRect.width = EditorGUIUtility.labelWidth;
            var stylePropertyRect = styleRect;
            stylePropertyRect.xMin += styleLabelRect.width + 3;
            var styleMaskRect = stylePropertyRect;
            styleMaskRect.width /= 2;
            var styleCaseRect = stylePropertyRect;
            styleCaseRect.xMin += styleMaskRect.width + 4;

            GUI.Label(styleLabelRect, "Font Style");

            var newStyles = styles;
            newStyles &= ~FontStyles.Bold;
            newStyles &= ~FontStyles.Italic;
            newStyles &= ~FontStyles.Underline;
            newStyles &= ~FontStyles.Strikethrough;
            newStyles &= ~FontStyles.LowerCase;
            newStyles &= ~FontStyles.UpperCase;
            newStyles &= ~FontStyles.SmallCaps;

            var fontStyleFlags = FontStylesToFontStyleFlags(styles);
            fontStyleFlags = (FontStyleFlags)EditorGUI.EnumFlagsField(styleMaskRect, fontStyleFlags);
            newStyles |= FontStyleFlagsToFontStyles(fontStyleFlags);

            var fontStyleCases = FontStylesToFontStyleCases(styles);
            fontStyleCases = (FontStyleCase)EditorGUI.EnumPopup(styleCaseRect, fontStyleCases);
            newStyles |= FontStyleCasesToFontStyles(fontStyleCases);

            return newStyles;
        }

        public event Action<CharacterStyleTMP> OnValueChanged;

        private void Setup(CharacterStyleTMP characterStyle)
        {
            _characterStyle = characterStyle;
            titleContent = new GUIContent(ObjectNames.NicifyVariableName(nameof(CharacterStyleTMP)));
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
            // Enable Auto Sizing
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.standardVerticalSpacing;
            // Min Font Size
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.standardVerticalSpacing;
            // Max Font Size
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.standardVerticalSpacing;
            // Character Width Adjustment
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.standardVerticalSpacing;
            // Line Spacing Adjustment
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.standardVerticalSpacing;
            
            // Spacing Option Titles
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.standardVerticalSpacing;
            // Character Spacing
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.standardVerticalSpacing;
            // Word Spacing
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.standardVerticalSpacing;
            // Line Spacing
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.standardVerticalSpacing;
            // Paragraph Spacing
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.standardVerticalSpacing;
            // Font Material
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.standardVerticalSpacing;
            // Padding
            height += 10;

            return height;
        }

        private FontStyleFlags FontStylesToFontStyleFlags(FontStyles styles)
        {
            var result = FontStyleFlags.Normal;
            if ((styles & FontStyles.Bold) != 0) result |= FontStyleFlags.Bold;
            if ((styles & FontStyles.Italic) != 0) result |= FontStyleFlags.Italic;
            if ((styles & FontStyles.Underline) != 0) result |= FontStyleFlags.Underline;
            if ((styles & FontStyles.Strikethrough) != 0) result |= FontStyleFlags.Strikethrough;

            return result;
        }

        private FontStyles FontStyleFlagsToFontStyles(FontStyleFlags flags)
        {
            var result = FontStyles.Normal;
            if ((flags & FontStyleFlags.Bold) != 0) result |= FontStyles.Bold;
            if ((flags & FontStyleFlags.Italic) != 0) result |= FontStyles.Italic;
            if ((flags & FontStyleFlags.Underline) != 0) result |= FontStyles.Underline;
            if ((flags & FontStyleFlags.Strikethrough) != 0) result |= FontStyles.Strikethrough;

            return result;
        }

        private FontStyleCase FontStylesToFontStyleCases(FontStyles styles)
        {
            if ((styles & FontStyles.LowerCase) != 0) return FontStyleCase.LowerCase;
            if ((styles & FontStyles.UpperCase) != 0) return FontStyleCase.UpperCase;
            if ((styles & FontStyles.SmallCaps) != 0) return FontStyleCase.SmallCaps;

            return FontStyleCase.None;
        }

        private FontStyles FontStyleCasesToFontStyles(FontStyleCase @case)
        {
            var result = FontStyles.Normal;
            switch (@case)
            {
                case FontStyleCase.LowerCase:
                    result |= FontStyles.LowerCase;
                    return result;
                case FontStyleCase.UpperCase:
                    result |= FontStyles.UpperCase;
                    return result;
                case FontStyleCase.SmallCaps:
                    result |= FontStyles.SmallCaps;
                    return result;
                case FontStyleCase.None:
                    return result;
                default:
                    throw new ArgumentOutOfRangeException(nameof(@case), @case, null);
            }
        }

        public static CharacterStyleTMPEditor Open(CharacterStyleTMP characterStyle)
        {
            var window = CreateInstance<CharacterStyleTMPEditor>();
            window.Setup(characterStyle);
            window.ShowAuxWindow();
            return window;
        }

        [Flags]
        private enum FontStyleFlags
        {
            Normal = 0,
            Bold = 1,
            Italic = 1 << 2,
            Underline = 1 << 3,
            Strikethrough = 1 << 4
        }

        private enum FontStyleCase
        {
            None,
            LowerCase,
            UpperCase,
            SmallCaps
        }
    }
}
