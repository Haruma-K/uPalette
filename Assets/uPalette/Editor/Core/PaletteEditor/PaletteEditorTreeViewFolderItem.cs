using UnityEditor.IMGUI.Controls;

namespace uPalette.Editor.Core.PaletteEditor
{
    internal sealed class PaletteEditorTreeViewFolderItem : TreeViewItem
    {
        public PaletteEditorTreeViewFolderItem(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}