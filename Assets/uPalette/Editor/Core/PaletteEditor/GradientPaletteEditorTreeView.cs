using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
#if UNITY_6000_2_OR_NEWER
using TreeViewState = UnityEditor.IMGUI.Controls.TreeViewState<int>;
#endif

namespace uPalette.Editor.Core.PaletteEditor
{
    internal sealed class GradientPaletteEditorTreeView : PaletteEditorTreeView<Gradient>
    {
        public GradientPaletteEditorTreeView(TreeViewState state) : base(state)
        {
        }

        protected override Gradient DrawValueField(Rect rect, Gradient value)
        {
            return EditorGUI.GradientField(rect, value);
        }
    }
}
