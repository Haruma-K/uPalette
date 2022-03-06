using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace uPalette.Runtime.Core.Synchronizer.CharacterStyle
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Text))]
    [CharacterStyleSynchronizer(typeof(Text), "Character Style")]
    public sealed class TextCharacterStyleSynchronizer : CharacterStyleSynchronizer<Text>
    {
        protected internal override Foundation.CharacterStyles.CharacterStyle GetValue()
        {
            return new Foundation.CharacterStyles.CharacterStyle
            {
                font = _component.font,
                fontStyle = _component.fontStyle,
                fontSize = _component.fontSize,
                lineSpacing = _component.lineSpacing
            };
        }

        private static int _latestRepaintFrame;

        protected internal override void SetValue(Foundation.CharacterStyles.CharacterStyle value)
        {
            _component.font = value.font;
            _component.fontStyle = value.fontStyle;
            _component.fontSize = value.fontSize;
            _component.lineSpacing = value.lineSpacing;
        }

        protected override bool EqualsToCurrentValue(Foundation.CharacterStyles.CharacterStyle value)
        {
            if (_component.font != value.font
                || _component.fontStyle != value.fontStyle
                || _component.fontSize != value.fontSize
                || _component.lineSpacing != value.lineSpacing)
                return false;

            return true;
        }
    }
}
