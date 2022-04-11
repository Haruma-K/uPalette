using UnityEngine;
using uPalette.Runtime.Core.Model;

namespace uPalette.Runtime.Core.Synchronizer.CharacterStyleTMP
{
    public abstract class CharacterStyleTMPSynchronizer : ValueSynchronizer<Foundation.CharacterStyles.CharacterStyleTMP>
    {
        [SerializeField] private CharacterStyleTMPEntryId _entryId = new CharacterStyleTMPEntryId();

        public override EntryId EntryId => _entryId;
        
        internal override Palette<Foundation.CharacterStyles.CharacterStyleTMP> GetPalette(PaletteStore store)
        {
            return store.CharacterStyleTMPPalette;
        }
    }

    public abstract class CharacterStyleTMPSynchronizer<T> : CharacterStyleTMPSynchronizer where T : Component
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
