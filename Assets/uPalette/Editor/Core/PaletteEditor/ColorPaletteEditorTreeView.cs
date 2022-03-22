using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace uPalette.Editor.Core.PaletteEditor
{
    internal sealed class ColorPaletteEditorTreeView : PaletteEditorTreeView<Color>
    {
        public ColorPaletteEditorTreeView(TreeViewState state) : base(state)
        {
        }

        protected override Color DrawValueField(Rect rect, Color value)
        {
            return EditorGUI.ColorField(rect, value);
        }
    }
}
