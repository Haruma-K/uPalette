using TMPro;
using UnityEngine;

namespace uPalette.Runtime.Core.Synchronizer.Color
{
    [RequireComponent(typeof(TMP_InputField))]
    [ColorSynchronizer(typeof(TMP_InputField), "Caret Color")]
    public sealed class TMPInputFieldCaretColorSynchronizer : ColorSynchronizer<TMP_InputField>
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
