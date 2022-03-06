using UnityEngine;
using uPalette.Runtime.Core.Model;

namespace uPalette.Runtime.Core.Synchronizer.Color
{
    public abstract class ColorSynchronizer : ValueSynchronizer<UnityEngine.Color>
    {
        internal override Palette<UnityEngine.Color> GetPalette(PaletteStore store)
        {
            return store.ColorPalette;
        }
    }
    
    public abstract class ColorSynchronizer<T> : ColorSynchronizer where T : Component
    {
        [SerializeField] [HideInInspector] protected T _component;
        
        protected virtual void Awake()
        {
            if (Application.isEditor)
            {
                _component = GetComponent<T>();
            }
        }
    }
}
