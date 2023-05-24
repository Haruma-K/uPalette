using TMPro;
using UnityEngine;

namespace uPalette.Runtime.Core.Synchronizer.CharacterStyleTMP
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    [CharacterStyleTMPSynchronizer(typeof(TextMeshProUGUI), "Character Style")]
    public sealed class TextMeshProUGUICharacterStyleTMPSynchronizer : CharacterStyleTMPSynchronizer<TextMeshProUGUI>
    {
        private static int _latestRepaintFrame;

        protected internal override Foundation.CharacterStyles.CharacterStyleTMP GetValue()
        {
            return new Foundation.CharacterStyles.CharacterStyleTMP
            {
                font = Component.font,
                fontStyle = Component.fontStyle,
                enableAutoSizing = Component.enableAutoSizing,
                fontSize = Component.fontSize,
                fontSizeMin = Component.fontSizeMin,
                fontSizeMax = Component.fontSizeMax,
                characterWidthAdjustment = Component.characterWidthAdjustment,
                lineSpacingAdjustment = Component.lineSpacingAdjustment,
                characterSpacing = Component.characterSpacing,
                wordSpacing = Component.wordSpacing,
                lineSpacing = Component.lineSpacing,
                paragraphSpacing = Component.paragraphSpacing
            };
        }

        protected internal override void SetValue(Foundation.CharacterStyles.CharacterStyleTMP value)
        {
            Component.font = value.font;
            Component.fontStyle = value.fontStyle;
            Component.enableAutoSizing = value.enableAutoSizing;
            Component.fontSize = value.fontSize;
            if (value.enableAutoSizeOptions)
            {
                Component.fontSizeMin = value.fontSizeMin;
                Component.fontSizeMax = value.fontSizeMax;
                Component.characterWidthAdjustment = value.characterWidthAdjustment;
                Component.lineSpacingAdjustment = value.lineSpacingAdjustment;
            }
            Component.characterSpacing = value.characterSpacing;
            Component.wordSpacing = value.wordSpacing;
            Component.lineSpacing = value.lineSpacing;
            Component.paragraphSpacing = value.paragraphSpacing;
        }

        protected override bool EqualsToCurrentValue(Foundation.CharacterStyles.CharacterStyleTMP value)
        {
            if (Component.font != null && Component.font.sourceFontFile != value.font.sourceFontFile)
            {
                return false;
            }

            var isNotEquals = Component.fontStyle != value.fontStyle
                              || Component.enableAutoSizing != value.enableAutoSizing
                              || Component.fontSize != value.fontSize
                              || Component.characterSpacing != value.characterSpacing
                              || Component.wordSpacing != value.wordSpacing
                              || Component.lineSpacing != value.lineSpacing
                              || Component.paragraphSpacing != value.paragraphSpacing;

            if (value.enableAutoSizeOptions)
                isNotEquals = isNotEquals
                              || Component.fontSizeMin != value.fontSizeMin
                              || Component.fontSizeMax != value.fontSizeMax
                              || Component.characterWidthAdjustment != value.characterWidthAdjustment
                              || Component.lineSpacingAdjustment != value.lineSpacingAdjustment;

            return !isNotEquals;
        }
    }
}
