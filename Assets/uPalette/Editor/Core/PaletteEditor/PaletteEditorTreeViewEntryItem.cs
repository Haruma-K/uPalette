using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using uPalette.Runtime.Foundation.TinyRx;
using uPalette.Runtime.Foundation.TinyRx.ObservableCollection;
using uPalette.Runtime.Foundation.TinyRx.ObservableProperty;

namespace uPalette.Editor.Core.PaletteEditor
{
    internal sealed class PaletteEditorTreeViewEntryItem<T> : TreeViewItem
    {
        private readonly ObservableDictionary<string, ObservableProperty<T>> _values =
            new ObservableDictionary<string, ObservableProperty<T>>();

        private readonly ObservableProperty<string> _name;

        private readonly Subject<Empty> _applyButtonClickedSubject = new Subject<Empty>();

        public PaletteEditorTreeViewEntryItem(string entryId, string name, Dictionary<string, T> values)
        {
            EntryId = entryId;
            _name = new ObservableProperty<string>(name);
            foreach (var value in values) _values.Add(value.Key, new ObservableProperty<T>(value.Value));
        }

        public IReadOnlyObservableProperty<string> Name => _name;
        public string EntryId { get; }
        public IReadOnlyObservableDictionary<string, ObservableProperty<T>> Values => _values;
        public IObservable<Empty> ApplyButtonClickedAsObservable => _applyButtonClickedSubject;

        public void SetName(string name, bool notifyChange)
        {
            if (notifyChange)
                _name.Value = name;
            else
                _name.SetValueAndNotNotify(name);

            displayName = name;
        }

        public bool HasValue(string themeId)
        {
            return _values.ContainsKey(themeId);
        }

        public void AddValue(string themeId, T value)
        {
            _values.Add(themeId, new ObservableProperty<T>(value));
        }

        public bool RemoveValue(string themeId)
        {
            return _values.Remove(themeId);
        }

        public void ClearValues()
        {
            _values.Clear();
        }

        public void OnApplyButtonClicked()
        {
            _applyButtonClickedSubject.OnNext(Empty.Default);
        }
    }
}
