using UnityEngine;
using UnityEngine.UI;

namespace uPalette.Runtime.Core.ColorSetters
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Selectable))]
    [ColorSetter(typeof(Selectable), "Transition Selected Color")]
    public class SelectableSelectedColorSetter : ColorSetter<Selectable>
    {
        protected override void Apply(Color color)
        {
            var colors = _component.colors;
            colors.selectedColor = color;
            _component.colors = colors;
        }
    }
}