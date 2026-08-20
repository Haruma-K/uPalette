using System;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
#if UNITY_6000_2_OR_NEWER
using TreeViewState = UnityEditor.IMGUI.Controls.TreeViewState<int>;
#endif

namespace uPalette.Editor.Core.PaletteEditor
{
    [Serializable]
    internal sealed class GradientPaletteEditorWindowContentsView : PaletteEditorWindowContentsView<Gradient>
    {
        protected override PaletteEditorTreeView<Gradient> CreateTreeView(TreeViewState state)
        {
            return new GradientPaletteEditorTreeView(state);
        }
    }
}
