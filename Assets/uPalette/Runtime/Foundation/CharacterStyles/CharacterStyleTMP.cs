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
        public float characterSpacing;
        public float wordSpacing;
        public float lineSpacing;
        public float paragraphSpacing;

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
                    fontSize = 36.0f,
                    enableAutoSizing = false,
                    characterSpacing = 0.0f,
                    wordSpacing = 0.0f,
                    lineSpacing = 0.0f,
                    paragraphSpacing = 0.0f
                };
            }
        }
    }
}
