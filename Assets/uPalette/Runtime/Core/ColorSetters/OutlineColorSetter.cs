using UnityEngine;
using UnityEngine.UI;

namespace uPalette.Runtime.Core.ColorSetters
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Outline))]
    [ColorSetter(typeof(Outline), "Color")]
    public class OutlineColorSetter : ColorSetter
    {
        [SerializeField] [HideInInspector] private Outline _outline;

        private void Awake()
        {
            if (Application.isEditor)
            {
                _outline = GetComponent<Outline>();
            }
        }

        protected override void Apply(Color color)
        {
            _outline.effectColor = color;
        }

        protected override Color GetValue()
        {
            return _outline.effectColor;
        }
    }
}