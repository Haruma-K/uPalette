using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using uPalette.Runtime.Foundation.Observable;

namespace uPalette.Runtime.Core
{
    public abstract class ColorSetter<T> : ColorSetter where T : Component
    {
        [SerializeField] [HideInInspector] protected T _component;
        
        protected virtual void Awake()
        {
            if (Application.isEditor)
            {
                _component = GetComponent<T>();
            }
        }
    }
    
    [ExecuteAlways]
    public abstract class ColorSetter : MonoBehaviour
    {
        [SerializeField] internal EntryId _entryId = new EntryId();
        private UPaletteApplication _application;
        private IDisposable _disposable;
        
        protected virtual void Start()
        {
            var entry = _application.UPaletteStore.Entries.FirstOrDefault(x => x.ID.Equals(_entryId.Value));
            if (entry != null)
            {
                Apply(entry.Value.Value);
            }
        }

        protected virtual void OnEnable()
        {
            _application = UPaletteApplication.RequestInstance();
#if UNITY_EDITOR
            RefreshEntry();
#endif
        }

        protected virtual void OnDisable()
        {
            UPaletteApplication.ReleaseInstance();
            _disposable?.Dispose();
        }

        protected abstract void Apply(Color color);

        [Serializable]
        internal class EntryId
        {
            [SerializeField] private string _value;
            public string Value => _value;
        }

#if UNITY_EDITOR
        public void SetEntry(string entryId)
        {
            var so = new SerializedObject(this);
            so.Update();
            so.FindProperty("_entryId._value").stringValue = entryId;
            so.ApplyModifiedProperties();
            RefreshEntry();
        }
        
        public void SetEntry(ColorEntry entry)
        {
            var so = new SerializedObject(this);
            so.Update();
            so.FindProperty("_entryId._value").stringValue = entry.ID;
            so.ApplyModifiedProperties();
            RefreshEntry();
        }

        private void RefreshEntry()
        {
            if (string.IsNullOrEmpty(_entryId.Value))
            {
                return;
            }

            var entry = _application.UPaletteStore.Entries.FirstOrDefault(x => x.ID.Equals(_entryId.Value));
            if (entry == null)
            {
                return;
            }

            _disposable?.Dispose();
            _disposable = entry.Value.Subscribe(x =>
            {
                Apply(x);
                EditorApplication.QueuePlayerLoopUpdate();
            });

            Apply(entry.Value.Value);
        }
#endif
    }
}