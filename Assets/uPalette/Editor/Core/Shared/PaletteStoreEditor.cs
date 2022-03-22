using UnityEditor;
using UnityEngine;
using uPalette.Runtime.Core;

namespace uPalette.Editor.Core.Shared
{
    [CustomEditor(typeof(PaletteStore))]
    public class PaletteStoreEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            base.OnInspectorGUI();
            GUI.enabled = true;
        }
    }
}
