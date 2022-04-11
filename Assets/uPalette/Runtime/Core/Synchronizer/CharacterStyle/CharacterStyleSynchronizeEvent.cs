using UnityEngine;
using uPalette.Runtime.Core.Model;

namespace uPalette.Runtime.Core.Synchronizer.CharacterStyle
{
    public sealed class
        CharacterStyleSynchronizeEvent : ValueSynchronizeEvent<Foundation.CharacterStyles.CharacterStyle>
    {
        [SerializeField] private CharacterStyleEntryId _entryId = new CharacterStyleEntryId();

        public override EntryId EntryId => _entryId;

        internal override Palette<Foundation.CharacterStyles.CharacterStyle> GetPalette(PaletteStore store)
        {
            return store.CharacterStylePalette;
        }
    }
}
