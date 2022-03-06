using System;

namespace uPalette.Runtime.Core.Synchronizer.CharacterStyle
{
    public sealed class CharacterStyleSynchronizerAttribute : ValueSynchronizerAttribute
    {
        public CharacterStyleSynchronizerAttribute(Type attachTargetType, string targetPropertyDisplayName)
            : base(typeof(Foundation.CharacterStyles.CharacterStyle), attachTargetType, targetPropertyDisplayName)
        {
        }
    }
}
