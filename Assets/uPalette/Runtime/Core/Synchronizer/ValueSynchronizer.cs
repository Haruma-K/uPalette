using System;
using UnityEngine;
using uPalette.Runtime.Core.Model;
using uPalette.Runtime.Foundation.TinyRx;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace uPalette.Runtime.Core.Synchronizer
{
    [ExecuteAlways]
    public abstract class ValueSynchronizer<T> : MonoBehaviour
    {
        [SerializeField] private EntryIdentifier _entryId; // For compatibility, entry id is wrapped in EntryIdentifier.
        private readonly CompositeDisposable _observingDisposables = new CompositeDisposable();

        public string EntryId => _entryId.Value;

        protected virtual void OnEnable()
        {
            StartObserving();
        }

        protected virtual void OnDisable()
        {
            StopObserving();
        }

        internal abstract Palette<T> GetPalette(PaletteStore store);

        protected internal abstract T GetValue();

        protected internal abstract void SetValue(T value);

        protected abstract bool EqualsToCurrentValue(T value);

        public void SetEntryId(string entryId)
        {
            StopObserving();
            _entryId.Value = entryId;
            StartObserving();
        }

        private void StartObserving()
        {
            if (string.IsNullOrEmpty(_entryId.Value))
                return;

            var store = PaletteStore.Instance;
            if (store == null)
            {
                Debug.LogWarning(
                    $"{GetType().FullName} tried to get {nameof(Entry<T>)} (ID: {_entryId}), but {nameof(PaletteStore)} was not found.");
                return;
            }

            var palette = GetPalette(store);

            if (!palette.TryGetActiveValue(_entryId.Value, out var value))
            {
                var errorMessage =
                    $"{GetType().FullName} tried to get {nameof(Entry<T>)} (ID: {_entryId}), but could not find it.";
                switch (store.MissingEntryErrorLevel)
                {
                    case MissingEntryErrorLevel.None:
                        return;
                    case MissingEntryErrorLevel.Warning:
                        Debug.LogWarning(errorMessage);
                        return;
                    case MissingEntryErrorLevel.Error:
                        Debug.LogError(errorMessage);
                        return;
                    case MissingEntryErrorLevel.Exception:
                        throw new Exception(errorMessage);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            value.Subscribe(x =>
            {
                if (!EqualsToCurrentValue(x))
                {
                    SetValue(x);
#if UNITY_EDITOR
                    EditorApplication.QueuePlayerLoopUpdate();
#endif
                }
            }).DisposeWith(_observingDisposables);

            if (!EqualsToCurrentValue(value.Value))
                SetValue(value.Value);
        }

        private void StopObserving()
        {
            _observingDisposables?.Clear();
        }

        [Serializable]
        private struct EntryIdentifier
        {
            [SerializeField] private string _value;

            public string Value
            {
                get => _value;
                set => _value = value;
            }
        }
    }
}
