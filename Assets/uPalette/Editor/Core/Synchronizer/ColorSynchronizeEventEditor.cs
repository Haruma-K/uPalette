using UnityEditor;
using UnityEngine;
using uPalette.Runtime.Core.Synchronizer.Color;

namespace uPalette.Editor.Core.Synchronizer
{
    [CustomEditor(inspectedType: typeof(ColorSynchronizeEvent), editorForChildClasses: true)]
    public sealed class ColorSynchronizeEventEditor : ValueSynchronizeEventEditor<Color>
    {
    }
}
