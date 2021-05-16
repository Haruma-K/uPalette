using UnityEngine;
using UnityEngine.UI;

namespace uPalette.Runtime.Core.ColorSetters
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Selectable))]
    [ColorSetter(typeof(Selectable), "Transition Pressed Color")]
    public class SelectablePressedColorSetter : ColorSetter<Selectable>
    {
        protected override void Apply(Color color)
        {
            var colors = _component.colors;
            colors.pressedColor = color;
            _component.colors = colors;
        }
    }
}