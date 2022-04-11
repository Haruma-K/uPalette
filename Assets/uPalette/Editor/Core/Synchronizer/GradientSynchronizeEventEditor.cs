using UnityEditor;
using UnityEngine;
using uPalette.Runtime.Core.Synchronizer.Gradient;

namespace uPalette.Editor.Core.Synchronizer
{
    [CustomEditor(typeof(GradientSynchronizeEvent), true)]
    public sealed class GradientSynchronizeEventEditor : ValueSynchronizeEventEditor<Gradient>
    {
    }
}
