using System;
using uPalette.Runtime.Foundation.CharacterStyles;

namespace uPalette.Runtime.Core.Model
{
    [Serializable]
    public sealed class CharacterStylePalette : Palette<CharacterStyle>
    {
        protected override CharacterStyle GetDefaultValue()
        {
            return CharacterStyle.Default;
        }
    }
}
