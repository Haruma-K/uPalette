using UnityEditor.IMGUI.Controls;
using UnityEngine;
using uPalette.Runtime.Foundation.TinyRx.ObservableProperty;

namespace uPalette.Editor.Core.ThemeEditor
{
    internal sealed class ThemeEditorTreeViewItem : TreeViewItem
    {
        private readonly ObservableProperty<string> _name = new ObservableProperty<string>();
        private readonly ObservableProperty<bool> _isActive;

        public ThemeEditorTreeViewItem(string themeId, string name, bool isActive)
        {
            ThemeId = themeId;
            _isActive = new ObservableProperty<bool>(isActive);
            SetName(name, false);
        }

        public IReadOnlyObservableProperty<string> Name => _name;
        public IObservableProperty<bool> IsActive => _isActive;
        public string ThemeId { get; }

        public void SetName(string name, bool notifyChange)
        {
            if (notifyChange)
                _name.Value = name;
            else
                _name.SetValueAndNotNotify(name);

            displayName = name;
        }
    }
}
