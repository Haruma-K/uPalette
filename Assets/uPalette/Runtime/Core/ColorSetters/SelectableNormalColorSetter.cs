using UnityEngine;
using UnityEngine.UI;

namespace uPalette.Runtime.Core.ColorSetters
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Selectable))]
    [ColorSetter(typeof(Selectable), "Transition Normal Color")]
    public class SelectableNormalColorSetter : ColorSetter<Selectable>
    {
        protected override void Apply(Color color)
        {
            var colors = _component.colors;
            colors.normalColor = color;
            _component.colors = colors;
        }
    }
}