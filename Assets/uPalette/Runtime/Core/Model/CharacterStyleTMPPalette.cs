using System;
using uPalette.Runtime.Foundation.CharacterStyles;

namespace uPalette.Runtime.Core.Model
{
    [Serializable]
    public sealed class CharacterStyleTMPPalette : Palette<CharacterStyleTMP>
    {
        protected override CharacterStyleTMP GetDefaultValue()
        {
            return CharacterStyleTMP.Default;
        }
    }
}
