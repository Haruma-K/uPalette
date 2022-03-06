using TMPro;
using UnityEngine;

namespace uPalette.Runtime.Core.Synchronizer.CharacterStyleTMP
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    [CharacterStyleTMPSynchronizer(typeof(TextMeshProUGUI), "Character Style")]
    public sealed class TextCharacterStyleTMPSynchronizer : CharacterStyleTMPSynchronizer<TextMeshProUGUI>
    {
        private static int _latestRepaintFrame;

        protected internal override Foundation.CharacterStyles.CharacterStyleTMP GetValue()
        {
            return new Foundation.CharacterStyles.CharacterStyleTMP
            {
                font = _component.font,
                fontStyle = _component.fontStyle,
                fontSize = _component.fontSize,
                enableAutoSizing = _component.enableAutoSizing,
                characterSpacing = _component.characterSpacing,
                wordSpacing = _component.wordSpacing,
                lineSpacing = _component.lineSpacing,
                paragraphSpacing = _component.paragraphSpacing
            };
        }

        protected internal override void SetValue(Foundation.CharacterStyles.CharacterStyleTMP value)
        {
            _component.font = value.font;
            _component.fontStyle = value.fontStyle;
            _component.fontSize = value.fontSize;
            _component.enableAutoSizing = value.enableAutoSizing;
            _component.characterSpacing = value.characterSpacing;
            _component.wordSpacing = value.wordSpacing;
            _component.lineSpacing = value.lineSpacing;
            _component.paragraphSpacing = value.paragraphSpacing;
        }

        protected override bool EqualsToCurrentValue(Foundation.CharacterStyles.CharacterStyleTMP value)
        {
            if (_component.font != null && _component.font.sourceFontFile != value.font.sourceFontFile)
            {
                return false;
            }

            if (_component.fontStyle != value.fontStyle
                || _component.fontSize != value.fontSize
                || _component.enableAutoSizing != value.enableAutoSizing
                || _component.characterSpacing != value.characterSpacing
                || _component.wordSpacing != value.wordSpacing
                || _component.lineSpacing != value.lineSpacing
                || _component.paragraphSpacing != value.paragraphSpacing)
                return false;

            return true;
        }
    }
}
