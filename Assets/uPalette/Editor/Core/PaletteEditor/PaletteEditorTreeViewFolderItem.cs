using UnityEditor.IMGUI.Controls;

namespace uPalette.Editor.Core.PaletteEditor
{
    internal sealed class PaletteEditorTreeViewFolderItem : TreeViewItem
    {
        public PaletteEditorTreeViewFolderItem(string folderPath)
        {
            FolderPath = folderPath;
        }

        public string FolderPath { get; }
    }
}