using System;
using UnityEditor.IMGUI.Controls;
using uPalette.Runtime.Foundation.CharacterStyles;

namespace uPalette.Editor.Core.PaletteEditor
{
    [Serializable]
    internal sealed class CharacterStylePaletteEditorWindowContentsView
        : PaletteEditorWindowContentsView<CharacterStyle>
    {
        protected override PaletteEditorTreeView<CharacterStyle> CreateTreeView(TreeViewState state)
        {
            return new CharacterStylePaletteEditorTreeView(state);
        }
    }
}
