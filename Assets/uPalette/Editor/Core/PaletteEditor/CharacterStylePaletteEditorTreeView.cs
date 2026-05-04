using UnityEditor.IMGUI.Controls;
using UnityEngine;
using uPalette.Editor.Foundation.CharacterStyles;
using uPalette.Runtime.Foundation.CharacterStyles;
#if UNITY_6000_2_OR_NEWER
using TreeViewState = UnityEditor.IMGUI.Controls.TreeViewState<int>;
#endif

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
