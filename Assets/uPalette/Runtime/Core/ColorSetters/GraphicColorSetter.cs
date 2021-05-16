using UnityEngine;
using UnityEngine.UI;

namespace uPalette.Runtime.Core.ColorSetters
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Graphic))]
    [ColorSetter(typeof(Graphic), "Color")]
    public class GraphicColorSetter : ColorSetter<Graphic>
    {
        protected override void Apply(Color color)
        {
            _component.color = color;
        }
    }
}