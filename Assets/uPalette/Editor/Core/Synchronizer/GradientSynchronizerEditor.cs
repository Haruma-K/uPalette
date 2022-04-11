using UnityEditor;
using UnityEngine;
using uPalette.Runtime.Core.Synchronizer.Gradient;

namespace uPalette.Editor.Core.Synchronizer
{
    [CustomEditor(typeof(GradientSynchronizer), true)]
    public sealed class GradientSynchronizerEditor : ValueSynchronizerEditor<Gradient>
    {
    }
}
