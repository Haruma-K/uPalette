using UnityEngine;
using uPalette.Runtime.Core.Model;

namespace uPalette.Runtime.Core.Synchronizer.CharacterStyleTMP
{
    public abstract class CharacterStyleTMPSynchronizer : ValueSynchronizer<Foundation.CharacterStyles.CharacterStyleTMP>
    {
        internal override Palette<Foundation.CharacterStyles.CharacterStyleTMP> GetPalette(PaletteStore store)
        {
            return store.CharacterStyleTMPPalette;
        }
    }

    public abstract class CharacterStyleTMPSynchronizer<T> : CharacterStyleTMPSynchronizer where T : Component
    {
        [SerializeField] [HideInInspector] protected T _component;

        protected virtual void Awake()
        {
            if (Application.isEditor)
                _component = GetComponent<T>();
        }
    }
}
