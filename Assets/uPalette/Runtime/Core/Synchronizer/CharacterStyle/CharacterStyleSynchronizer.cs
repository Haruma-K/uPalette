using UnityEngine;
using uPalette.Runtime.Core.Model;

namespace uPalette.Runtime.Core.Synchronizer.CharacterStyle
{
    public abstract class CharacterStyleSynchronizer : ValueSynchronizer<Foundation.CharacterStyles.CharacterStyle>
    {
        internal override Palette<Foundation.CharacterStyles.CharacterStyle> GetPalette(PaletteStore store)
        {
            return store.CharacterStylePalette;
        }
    }

    public abstract class CharacterStyleSynchronizer<T> : CharacterStyleSynchronizer where T : Component
    {
        [SerializeField] [HideInInspector] protected T _component;

        protected virtual void Awake()
        {
            if (Application.isEditor)
                _component = GetComponent<T>();
        }
    }
}
