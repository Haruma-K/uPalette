using UnityEngine;
using uPalette.Runtime.Core.Model;

namespace uPalette.Runtime.Core.Synchronizer.CharacterStyle
{
    public abstract class CharacterStyleSynchronizer : ValueSynchronizer<Foundation.CharacterStyles.CharacterStyle>
    {
        [SerializeField] private CharacterStyleEntryId _entryId = new CharacterStyleEntryId();

        public override EntryId EntryId => _entryId;
        
        internal override Palette<Foundation.CharacterStyles.CharacterStyle> GetPalette(PaletteStore store)
        {
            return store.CharacterStylePalette;
        }
    }

    public abstract class CharacterStyleSynchronizer<T> : CharacterStyleSynchronizer where T : Component
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
