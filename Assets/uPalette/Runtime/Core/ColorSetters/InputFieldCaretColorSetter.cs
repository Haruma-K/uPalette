using UnityEngine;
using UnityEngine.UI;

namespace uPalette.Runtime.Core.ColorSetters
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(InputField))]
    [ColorSetter(typeof(InputField), "Caret Color")]
    public class InputFieldCaretColorSetter : ColorSetter<InputField>
    {
        protected override void Apply(Color color)
        {
            _component.customCaretColor = true;
            _component.caretColor = color;
        }

        protected override Color GetValue()
        {
            return _component.caretColor;
        }
    }
}