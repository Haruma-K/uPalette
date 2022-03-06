using System;
using UnityEngine;

namespace uPalette.Runtime.Core.Model
{
    [Serializable]
    public sealed class GradientPalette : Palette<Gradient>
    {
        protected override Gradient GetDefaultValue()
        {
            var gradient = new Gradient();

            var colorKeys = new GradientColorKey[2];
            colorKeys[0].color = Color.white;
            colorKeys[0].time = 0.0f;
            colorKeys[1].color = Color.white;
            colorKeys[1].time = 1.0f;

            var alphaKeys = new GradientAlphaKey[2];
            alphaKeys[0].alpha = 1.0f;
            alphaKeys[0].time = 0.0f;
            alphaKeys[1].alpha = 1.0f;
            alphaKeys[1].time = 1.0f;

            gradient.SetKeys(colorKeys, alphaKeys);

            return gradient;
        }
    }
}
