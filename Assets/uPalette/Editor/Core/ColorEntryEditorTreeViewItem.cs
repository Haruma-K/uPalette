using UnityEditor.IMGUI.Controls;
using UnityEngine;
using uPalette.Runtime.Foundation.Observable.ObservableProperty;

namespace uPalette.Editor.Core
{
    public class ColorEntryEditorTreeViewItem : TreeViewItem
    {
        public ColorEntryEditorTreeViewItem(string entryId)
        {
            EntryId = entryId;
        }

        public string EntryId { get; }
        public ObservableProperty<string> Name { get; } = new ObservableProperty<string>();
        public ObservableProperty<Color> Color { get; } = new ObservableProperty<Color>();
    }
}