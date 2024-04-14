using UnityEngine;

namespace uPalette.Runtime.Core.Synchronizer
{
    [ExecuteAlways]
    public abstract class ValueSynchronizer<T> : ValueSynchronizerBase<T>
    {
        protected internal abstract T GetValue();

        protected internal abstract void SetValue(T value);

        protected override void OnValueChanged(T value)
        {
            SetValue(value);
        }
    }
}
