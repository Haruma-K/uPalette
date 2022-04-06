using UnityEngine;
using UnityEngine.UI;

namespace uPalette.Runtime.Core.Synchronizer.Color
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Selectable))]
    [ColorSynchronizer(typeof(Selectable), "Transition Normal Color")]
    public sealed class SelectableNormalColorSetter : ColorSynchronizer<Selectable>
    {
        protected internal override UnityEngine.Color GetValue()
        {
            return Component.colors.normalColor;
        }

        protected internal override void SetValue(UnityEngine.Color value)
        {
            var colors = Component.colors;
            colors.normalColor = value;
            Component.colors = colors;
        }

        protected override bool EqualsToCurrentValue(UnityEngine.Color value)
        {
            return Component.colors.normalColor == value;
        }
    }
}
