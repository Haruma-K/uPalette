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
    public abstract class ValueSynchronizerBase<T> : MonoBehaviour
    {
        private readonly CompositeDisposable _observingDisposables = new CompositeDisposable();
        public abstract EntryId EntryId { get; }

        protected virtual void OnEnable()
        {
            StartObserving();
        }

        protected virtual void OnDisable()
        {
            StopObserving();
        }

        internal abstract Palette<T> GetPalette(PaletteStore store);

        public void SetEntryId(string entryId)
        {
            StopObserving();
#if UNITY_EDITOR
            var so = new SerializedObject(this);
            var entryIdProperty = so.FindProperty("_entryId._value");
            entryIdProperty.stringValue = entryId;
            so.ApplyModifiedProperties();
#else
            EntryId.Value = entryId;
#endif
            if (isActiveAndEnabled)
                StartObserving();
        }

        internal void StartObserving()
        {
            if (string.IsNullOrEmpty(EntryId.Value))
                return;

            var store = PaletteStore.Instance;
            if (store == null)
            {
                Debug.LogWarning(
                    $"{GetType().FullName} tried to get {nameof(Entry<T>)} (ID: {EntryId.Value}), but {nameof(PaletteStore)} was not found.");
                return;
            }

            var palette = GetPalette(store);

            if (!palette.TryGetActiveValue(EntryId.Value, out var value))
            {
                var errorMessage =
                    $"{GetType().FullName} tried to get {nameof(Entry<T>)} (ID: {EntryId.Value}), but could not find it.";
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
                OnValueChanged(x);
#if UNITY_EDITOR
                EditorApplication.QueuePlayerLoopUpdate();
#endif
            }).DisposeWith(_observingDisposables);

            OnValueChanged(value.Value);
        }

        protected abstract void OnValueChanged(T value);

        internal void StopObserving()
        {
            _observingDisposables?.Clear();
        }
    }
}
