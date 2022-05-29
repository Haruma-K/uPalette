using TMPro;
using UnityEngine;

namespace uPalette.Runtime.Core.Synchronizer.Color
{
    [RequireComponent(typeof(TMP_InputField))]
    [ColorSynchronizer(typeof(TMP_InputField), "Selection Color")]
    public sealed class TMPInputFieldSelectionColorSynchronizer : ColorSynchronizer<TMP_InputField>
    {
        protected internal override UnityEngine.Color GetValue()
        {
            return Component.selectionColor;
        }

        protected internal override void SetValue(UnityEngine.Color value)
        {
            Component.selectionColor = value;
        }

        protected override bool EqualsToCurrentValue(UnityEngine.Color value)
        {
            return Component.selectionColor == value;
        }
    }
}
