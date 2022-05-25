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
                fontSize = Component.fontSize,
                enableAutoSizing = Component.enableAutoSizing,
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
            Component.fontSize = value.fontSize;
            Component.enableAutoSizing = value.enableAutoSizing;
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

            if (Component.fontStyle != value.fontStyle
                || Component.fontSize != value.fontSize
                || Component.enableAutoSizing != value.enableAutoSizing
                || Component.characterSpacing != value.characterSpacing
                || Component.wordSpacing != value.wordSpacing
                || Component.lineSpacing != value.lineSpacing
                || Component.paragraphSpacing != value.paragraphSpacing)
                return false;

            return true;
        }
    }
}
