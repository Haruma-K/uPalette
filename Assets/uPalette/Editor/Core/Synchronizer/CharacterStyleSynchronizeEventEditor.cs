using UnityEditor;
using uPalette.Runtime.Core.Synchronizer.CharacterStyle;
using uPalette.Runtime.Foundation.CharacterStyles;

namespace uPalette.Editor.Core.Synchronizer
{
    [CustomEditor(typeof(CharacterStyleSynchronizeEvent), true)]
    public sealed class CharacterStyleSynchronizeEventEditor : ValueSynchronizeEventEditor<CharacterStyle>
    {
    }
}
