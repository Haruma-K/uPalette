using System;

namespace uPalette.Runtime.Core.Synchronizer.Gradient
{
    public sealed class GradientSynchronizerAttribute : ValueSynchronizerAttribute
    {
        public GradientSynchronizerAttribute(Type attachTargetType, string targetPropertyDisplayName)
            : base(typeof(UnityEngine.Gradient), attachTargetType, targetPropertyDisplayName)
        {
        }
    }
}
