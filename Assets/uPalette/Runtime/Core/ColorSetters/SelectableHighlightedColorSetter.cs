using UnityEngine;
using UnityEngine.UI;

namespace uPalette.Runtime.Core.ColorSetters
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Selectable))]
    [ColorSetter(typeof(Selectable), "Transition Highlighted Color")]
    public class SelectableHighlightedColorSetter : ColorSetter<Selectable>
    {
        protected override void Apply(Color color)
        {
            var colors = _component.colors;
            colors.highlightedColor = color;
            _component.colors = colors;
        }

        protected override Color GetValue()
        {
            return _component.colors.highlightedColor;
        }
    }
}