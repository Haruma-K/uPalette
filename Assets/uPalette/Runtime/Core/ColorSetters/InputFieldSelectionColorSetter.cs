using UnityEngine;
using UnityEngine.UI;

namespace uPalette.Runtime.Core.ColorSetters
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(InputField))]
    [ColorSetter(typeof(InputField), "Selection Color")]
    public class InputFieldSelectionColorSetter : ColorSetter<InputField>
    {
        protected override void Apply(Color color)
        {
            _component.customCaretColor = true;
            _component.selectionColor = color;
        }

        protected override Color GetValue()
        {
            return _component.selectionColor;
        }
    }
}