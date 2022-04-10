using UnityEngine;
using uPalette.Runtime.Core.Model;

namespace uPalette.Runtime.Core.Synchronizer.Gradient
{
    public abstract class GradientSynchronizer : ValueSynchronizer<UnityEngine.Gradient>
    {
        [SerializeField] private GradientEntryId _entryId = new GradientEntryId();

        public override EntryId EntryId => _entryId;
        
        internal override Palette<UnityEngine.Gradient> GetPalette(PaletteStore store)
        {
            return store.GradientPalette;
        }
    }

    public abstract class GradientSynchronizer<T> : GradientSynchronizer where T : Component
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
