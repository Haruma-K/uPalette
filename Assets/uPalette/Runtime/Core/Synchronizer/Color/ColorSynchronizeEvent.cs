using UnityEngine;
using uPalette.Runtime.Core.Model;

namespace uPalette.Runtime.Core.Synchronizer.Color
{
    public sealed class ColorSynchronizeEvent : ValueSynchronizeEvent<UnityEngine.Color>
    {
        [SerializeField] private ColorEntryId _entryId = new ColorEntryId();

        public override EntryId EntryId => _entryId;

        internal override Palette<UnityEngine.Color> GetPalette(PaletteStore store)
        {
            return store.ColorPalette;
        }
    }
}
