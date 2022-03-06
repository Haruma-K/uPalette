using UnityEngine;
using UnityEngine.UI;

namespace uPalette.Runtime.Core.Synchronizer.Color
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Selectable))]
    [ColorSynchronizer(typeof(Selectable), "Transition Selected Color")]
    public sealed class SelectableSelectedColorSynchronizer : ColorSynchronizer<Selectable>
    {
        protected internal override UnityEngine.Color GetValue()
        {
            return _component.colors.selectedColor;
        }

        protected internal override void SetValue(UnityEngine.Color color)
        {
            var colors = _component.colors;
            colors.selectedColor = color;
            _component.colors = colors;
        }

        protected override bool EqualsToCurrentValue(UnityEngine.Color value)
        {
            return _component.colors.selectedColor == value;
        }
    }
}
