using UnityEngine;
using UnityEngine.UI;

namespace uPalette.Runtime.Core.Synchronizer.Color
{
    [RequireComponent(typeof(InputField))]
    [ColorSynchronizer(typeof(InputField), "Caret Color")]
    public sealed class InputFieldCaretColorSynchronizer : ColorSynchronizer<InputField>
    {
        protected internal override UnityEngine.Color GetValue()
        {
            return Component.caretColor;
        }

        protected internal override void SetValue(UnityEngine.Color value)
        {
            Component.customCaretColor = true;
            Component.caretColor = value;
        }

        protected override bool EqualsToCurrentValue(UnityEngine.Color value)
        {
            return Component.caretColor == value;
        }
    }
}
