using UnityEditor;
using uPalette.Runtime.Core.Synchronizer.CharacterStyleTMP;
using uPalette.Runtime.Foundation.CharacterStyles;

namespace uPalette.Editor.Core.Synchronizer
{
    [CustomEditor(typeof(CharacterStyleTMPSynchronizeEvent), true)]
    public sealed class CharacterStyleTMPSynchronizeEventEditor : ValueSynchronizeEventEditor<CharacterStyleTMP>
    {
    }
}
