using UnityEngine;
using uPalette.Runtime.Core.Model;

namespace uPalette.Runtime.Core.Synchronizer.Gradient
{
    public abstract class GradientSynchronizer : ValueSynchronizer<UnityEngine.Gradient>
    {
        internal override Palette<UnityEngine.Gradient> GetPalette(PaletteStore store)
        {
            return store.GradientPalette;
        }
    }

    public abstract class GradientSynchronizer<T> : GradientSynchronizer where T : Component
    {
        [SerializeField] [HideInInspector] protected T _component;

        protected virtual void Awake()
        {
            if (Application.isEditor) _component = GetComponent<T>();
        }
    }
}
