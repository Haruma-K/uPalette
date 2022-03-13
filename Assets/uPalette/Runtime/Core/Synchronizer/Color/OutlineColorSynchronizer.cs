using UnityEngine;
using UnityEngine.UI;

namespace uPalette.Runtime.Core.Synchronizer.Color
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Outline))]
    [ColorSynchronizer(typeof(Outline), "Color")]
    public sealed class OutlineColorSynchronizer : ColorSynchronizer<Outline>
    {
        protected internal override UnityEngine.Color GetValue()
        {
            return _component.effectColor;
        }

        protected internal override void SetValue(UnityEngine.Color value)
        {
            _component.effectColor = value;
        }

        protected override bool EqualsToCurrentValue(UnityEngine.Color value)
        {
            return _component.effectColor == value;
        }
    }
}
