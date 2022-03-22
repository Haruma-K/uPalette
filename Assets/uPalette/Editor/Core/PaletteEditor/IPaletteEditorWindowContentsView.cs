using UnityEngine;

namespace uPalette.Editor.Core.PaletteEditor
{
    internal interface IPaletteEditorWindowContentsView
    {
        void DrawSearchFieldToolbarGUI();

        void DrawTreeViewGUI(Rect rect);
    }
}
