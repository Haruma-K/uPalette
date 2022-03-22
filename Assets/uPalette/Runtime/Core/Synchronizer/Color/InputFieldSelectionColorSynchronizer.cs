using UnityEngine;
using UnityEngine.UI;

namespace uPalette.Runtime.Core.Synchronizer.Color
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(InputField))]
    [ColorSynchronizer(typeof(InputField), "Selection Color")]
    public sealed class InputFieldSelectionColorSynchronizer : ColorSynchronizer<InputField>
    {
        protected internal override UnityEngine.Color GetValue()
        {
            return _component.selectionColor;
        }

        protected internal override void SetValue(UnityEngine.Color value)
        {
            _component.customCaretColor = true;
            _component.selectionColor = value;
        }

        protected override bool EqualsToCurrentValue(UnityEngine.Color value)
        {
            return _component.selectionColor == value;
        }
    }
}
