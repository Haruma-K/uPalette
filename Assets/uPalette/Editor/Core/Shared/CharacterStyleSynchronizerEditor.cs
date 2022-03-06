using UnityEditor;
using uPalette.Runtime.Core.Synchronizer.CharacterStyle;
using uPalette.Runtime.Foundation.CharacterStyles;

namespace uPalette.Editor.Core.Shared
{
    [CustomEditor(typeof(CharacterStyleSynchronizer), true)]
    public sealed class CharacterStyleSynchronizerEditor : ValueSynchronizerEditor<CharacterStyle>
    {
    }
}
