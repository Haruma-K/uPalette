using System;
using UnityEditor.IMGUI.Controls;
using uPalette.Runtime.Foundation.CharacterStyles;

namespace uPalette.Editor.Core.PaletteEditor
{
    [Serializable]
    internal sealed class CharacterStyleTMPPaletteEditorWindowContentsView
        : PaletteEditorWindowContentsView<CharacterStyleTMP>
    {
        protected override PaletteEditorTreeView<CharacterStyleTMP> CreateTreeView(TreeViewState state)
        {
            return new CharacterStyleTMPPaletteEditorTreeView(state);
        }
    }
}
