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
        [SerializeField] [HideInInspector] private T _component;

        protected T Component
        {
            get
            {
                if (_component == null)
                {
                    _component = GetComponent<T>();
                }

                return _component;
            }
        }
    }
}
