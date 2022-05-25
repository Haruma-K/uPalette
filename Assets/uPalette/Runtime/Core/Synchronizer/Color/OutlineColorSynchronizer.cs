using UnityEngine;
using UnityEngine.UI;

namespace uPalette.Runtime.Core.Synchronizer.Color
{
    [RequireComponent(typeof(Outline))]
    [ColorSynchronizer(typeof(Outline), "Color")]
    public sealed class OutlineColorSynchronizer : ColorSynchronizer<Outline>
    {
        protected internal override UnityEngine.Color GetValue()
        {
            return Component.effectColor;
        }

        protected internal override void SetValue(UnityEngine.Color value)
        {
            Component.effectColor = value;
        }

        protected override bool EqualsToCurrentValue(UnityEngine.Color value)
        {
            return Component.effectColor == value;
        }
    }
}
