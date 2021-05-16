using UnityEditor;
using UnityEngine;
using uPalette.Runtime.Core;

namespace uPalette.Editor.Core
{
    [CustomEditor(typeof(ColorSetter), true)]
    public class ColorSetterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Open uPalette Editor"))
            {
                UPaletteEditorWindow.Open();
            }
        }
    }
}