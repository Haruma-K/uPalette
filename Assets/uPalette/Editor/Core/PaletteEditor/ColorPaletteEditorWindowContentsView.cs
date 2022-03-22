using System;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace uPalette.Editor.Core.PaletteEditor
{
    [Serializable]
    internal sealed class ColorPaletteEditorWindowContentsView : PaletteEditorWindowContentsView<Color>
    {
        protected override PaletteEditorTreeView<Color> CreateTreeView(TreeViewState state)
        {
            return new ColorPaletteEditorTreeView(state);
        }
    }
}
