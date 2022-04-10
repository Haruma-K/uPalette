using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
#endif

namespace uPalette.Runtime.Core.Synchronizer
{
    [ExecuteAlways]
    public abstract class ValueSynchronizeEvent<T> : ValueSynchronizerBase<T>
    {
        [SerializeField] private UnityEvent<T> _valueChanged;

        public UnityEvent<T> ValueChanged => _valueChanged;
        
        protected override void OnValueChanged(T value)
        {
            _valueChanged.Invoke(value);
        }
    }
}
