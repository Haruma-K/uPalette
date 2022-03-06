using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

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
