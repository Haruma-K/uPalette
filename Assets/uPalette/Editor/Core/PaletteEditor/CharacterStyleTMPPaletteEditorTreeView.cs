using UnityEditor.IMGUI.Controls;
using UnityEngine;
using uPalette.Editor.Foundation.CharacterStyles;
using uPalette.Runtime.Foundation.CharacterStyles;
#if UNITY_6000_2_OR_NEWER
using TreeViewState = UnityEditor.IMGUI.Controls.TreeViewState<int>;
#endif

namespace uPalette.Editor.Core.PaletteEditor
{
    internal sealed class CharacterStyleTMPPaletteEditorTreeView : PaletteEditorTreeView<CharacterStyleTMP>
    {
        public CharacterStyleTMPPaletteEditorTreeView(TreeViewState state) : base(state)
        {
        }

        protected override CharacterStyleTMP DrawValueField(Rect rect, CharacterStyleTMP value)
        {
            return CharacterStyleTMPEditorGUILayout.DrawField(rect, value);
        }
    }
}
