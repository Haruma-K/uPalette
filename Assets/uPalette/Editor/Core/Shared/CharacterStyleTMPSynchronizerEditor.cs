using UnityEditor;
using uPalette.Runtime.Core.Synchronizer.CharacterStyleTMP;
using uPalette.Runtime.Foundation.CharacterStyles;

namespace uPalette.Editor.Core.Shared
{
    [CustomEditor(typeof(CharacterStyleTMPSynchronizer), true)]
    public sealed class CharacterStyleTMPSynchronizerEditor : ValueSynchronizerEditor<CharacterStyleTMP>
    {
    }
}
