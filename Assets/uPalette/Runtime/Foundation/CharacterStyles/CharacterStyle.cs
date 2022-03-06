using System;
using UnityEngine;

namespace uPalette.Runtime.Foundation.CharacterStyles
{
    [Serializable]
    public struct CharacterStyle
    {
        public Font font;
        public FontStyle fontStyle;
        public int fontSize;
        public float lineSpacing;

        public static CharacterStyle Default =>
            new CharacterStyle
            {
                font = Resources.GetBuiltinResource<Font>("Arial.ttf"),
                fontStyle = FontStyle.Normal,
                fontSize = 14,
                lineSpacing = 1
            };
    }
}
