using System;

namespace uPalette.Runtime.Core.Synchronizer.CharacterStyleTMP
{
    public sealed class CharacterStyleTMPSynchronizerAttribute : ValueSynchronizerAttribute
    {
        public CharacterStyleTMPSynchronizerAttribute(Type attachTargetType, string targetPropertyDisplayName)
            : base(typeof(Foundation.CharacterStyles.CharacterStyleTMP), attachTargetType, targetPropertyDisplayName)
        {
        }
    }
}
