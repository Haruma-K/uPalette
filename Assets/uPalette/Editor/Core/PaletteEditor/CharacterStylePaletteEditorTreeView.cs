using UnityEditor.IMGUI.Controls;
using UnityEngine;
using uPalette.Editor.Foundation.CharacterStyles;
using uPalette.Runtime.Foundation.CharacterStyles;

namespace uPalette.Editor.Core.PaletteEditor
{
    internal sealed class CharacterStylePaletteEditorTreeView : PaletteEditorTreeView<CharacterStyle>
    {
        public CharacterStylePaletteEditorTreeView(TreeViewState state) : base(state)
        {
        }

        protected override CharacterStyle DrawValueField(Rect rect, CharacterStyle value)
        {
            return CharacterStyleEditorGUILayout.DrawField(rect, value);
        }
    }
}
