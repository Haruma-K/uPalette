using System;

namespace uPalette.Runtime.Core.Synchronizer.Color
{
    public sealed class ColorSynchronizerAttribute : ValueSynchronizerAttribute
    {
        public ColorSynchronizerAttribute(Type attachTargetType, string targetPropertyDisplayName)
            : base(typeof(UnityEngine.Color), attachTargetType, targetPropertyDisplayName)
        {
        }
    }
}
