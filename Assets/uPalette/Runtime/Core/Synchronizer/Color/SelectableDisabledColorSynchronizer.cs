using UnityEngine;
using UnityEngine.UI;

namespace uPalette.Runtime.Core.Synchronizer.Color
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Selectable))]
    [ColorSynchronizer(typeof(Selectable), "Transition Disabled Color")]
    public sealed class SelectableDisabledColorSynchronizer : ColorSynchronizer<Selectable>
    {
        protected internal override UnityEngine.Color GetValue()
        {
            return Component.colors.disabledColor;
        }

        protected internal override void SetValue(UnityEngine.Color value)
        {
            var colors = Component.colors;
            colors.disabledColor = value;
            Component.colors = colors;
        }

        protected override bool EqualsToCurrentValue(UnityEngine.Color value)
        {
            return Component.colors.disabledColor == value;
        }
    }
}
