using UnityEngine;
using UnityEngine.UI;

namespace uPalette.Runtime.Core.Synchronizer.Color
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Selectable))]
    [ColorSynchronizer(typeof(Selectable), "Transition Highlighted Color")]
    public sealed class SelectableHighlightedColorSynchronizer : ColorSynchronizer<Selectable>
    {
        protected internal override UnityEngine.Color GetValue()
        {
            return Component.colors.highlightedColor;
        }

        protected internal override void SetValue(UnityEngine.Color value)
        {
            var colors = Component.colors;
            colors.highlightedColor = value;
            Component.colors = colors;
        }

        protected override bool EqualsToCurrentValue(UnityEngine.Color value)
        {
            return Component.colors.highlightedColor == value;
        }
    }
}
