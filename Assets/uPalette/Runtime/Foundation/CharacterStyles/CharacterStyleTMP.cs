using System;
using TMPro;
using UnityEngine;

namespace uPalette.Runtime.Foundation.CharacterStyles
{
    [Serializable]
    public struct CharacterStyleTMP
    {
        public TMP_FontAsset font;
        public FontStyles fontStyle;
        public float fontSize;
        public bool enableAutoSizing;

        // For backward compatibility.
        // Synchronize "Auto Size Options" of TextMesh Pro.
        [SerializeField] internal bool enableAutoSizeOptions;

        public float fontSizeMin;
        public float fontSizeMax;
        public float characterWidthAdjustment;
        public float lineSpacingAdjustment;
        public float characterSpacing;
        public float wordSpacing;
        public float lineSpacing;
        public float paragraphSpacing;
        public Material fontSharedMaterial;

        public static CharacterStyleTMP Default
        {
            get
            {
                var font = TMP_Settings.defaultFontAsset == null
                    ? Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF")
                    : TMP_Settings.defaultFontAsset;

                return new CharacterStyleTMP
                {
                    font = font,
                    fontStyle = 0,
                    enableAutoSizing = false,
                    fontSize = TMP_Settings.defaultFontSize,
                    enableAutoSizeOptions = true,
                    fontSizeMin = TMP_Settings.defaultFontSize * TMP_Settings.defaultTextAutoSizingMinRatio,
                    fontSizeMax = TMP_Settings.defaultFontSize * TMP_Settings.defaultTextAutoSizingMaxRatio,
                    characterWidthAdjustment = 0.0f,
                    lineSpacingAdjustment = 0.0f,
                    characterSpacing = 0.0f,
                    wordSpacing = 0.0f,
                    lineSpacing = 0.0f,
                    paragraphSpacing = 0.0f,
                    fontSharedMaterial = null
                };
            }
        }
    }
}
