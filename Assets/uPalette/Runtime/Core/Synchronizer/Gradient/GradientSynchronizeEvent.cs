using UnityEngine;
using uPalette.Runtime.Core.Model;

namespace uPalette.Runtime.Core.Synchronizer.Gradient
{
    public sealed class GradientSynchronizeEvent : ValueSynchronizeEvent<UnityEngine.Gradient>
    {
        [SerializeField] private GradientEntryId _entryId = new GradientEntryId();

        public override EntryId EntryId => _entryId;

        internal override Palette<UnityEngine.Gradient> GetPalette(PaletteStore store)
        {
            return store.GradientPalette;
        }
    }
}
