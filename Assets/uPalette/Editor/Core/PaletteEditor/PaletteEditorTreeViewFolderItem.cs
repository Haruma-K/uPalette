using UnityEditor.IMGUI.Controls;
#if UNITY_6000_2_OR_NEWER
using TreeViewItem = UnityEditor.IMGUI.Controls.TreeViewItem<int>;
#endif

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