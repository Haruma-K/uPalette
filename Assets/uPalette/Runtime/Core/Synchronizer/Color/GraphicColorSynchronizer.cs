using UnityEngine;
using UnityEngine.UI;

namespace uPalette.Runtime.Core.Synchronizer.Color
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Graphic))]
    [ColorSynchronizer(typeof(Graphic), "Color")]
    public sealed class GraphicColorSynchronizer : ColorSynchronizer<Graphic>
    {
        protected internal override UnityEngine.Color GetValue()
        {
            return _component.color;
        }

        protected internal override void SetValue(UnityEngine.Color color)
        {
            _component.color = color;
        }

        protected override bool EqualsToCurrentValue(UnityEngine.Color value)
        {
            return _component.color == value;
        }
    }
}
