using UnityEngine;
using uPalette.Runtime.Core.Model;

namespace uPalette.Runtime.Core.Synchronizer.CharacterStyleTMP
{
    public sealed class
        CharacterStyleTMPSynchronizeEvent : ValueSynchronizeEvent<Foundation.CharacterStyles.CharacterStyleTMP>
    {
        [SerializeField] private CharacterStyleTMPEntryId _entryId = new CharacterStyleTMPEntryId();

        public override EntryId EntryId => _entryId;

        internal override Palette<Foundation.CharacterStyles.CharacterStyleTMP> GetPalette(PaletteStore store)
        {
            return store.CharacterStyleTMPPalette;
        }
    }
}
