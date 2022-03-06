using UnityEngine;
using UnityEngine.UI;

namespace uPalette.Runtime.Core.Synchronizer.Color
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(InputField))]
    [ColorSynchronizer(typeof(InputField), "Caret Color")]
    public sealed class InputFieldCaretColorSynchronizer : ColorSynchronizer<InputField>
    {
        protected internal override UnityEngine.Color GetValue()
        {
            return _component.caretColor;
        }

        protected internal override void SetValue(UnityEngine.Color color)
        {
            _component.customCaretColor = true;
            _component.caretColor = color;
        }

        protected override bool EqualsToCurrentValue(UnityEngine.Color value)
        {
            return _component.caretColor == value;
        }
    }
}
