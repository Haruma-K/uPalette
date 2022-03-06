using System;
using UnityEngine;

namespace uPalette.Runtime.Core.Model
{
    [Serializable]
    public sealed class ColorPalette : Palette<Color>
    {
        protected override Color GetDefaultValue()
        {
            return Color.white;
        }
    }
}
