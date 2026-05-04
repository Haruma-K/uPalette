using System;
using UnityEditor.IMGUI.Controls;
using uPalette.Runtime.Foundation.CharacterStyles;
#if UNITY_6000_2_OR_NEWER
using TreeViewState = UnityEditor.IMGUI.Controls.TreeViewState<int>;
#endif

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
