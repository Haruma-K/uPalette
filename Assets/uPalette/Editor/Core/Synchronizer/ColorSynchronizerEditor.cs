using UnityEditor;
using UnityEngine;
using uPalette.Runtime.Core.Synchronizer.Color;

namespace uPalette.Editor.Core.Synchronizer
{
    [CustomEditor(inspectedType: typeof(ColorSynchronizer), editorForChildClasses: true)]
    public sealed class ColorSynchronizerEditor : ValueSynchronizerEditor<Color>
    {
    }
}
