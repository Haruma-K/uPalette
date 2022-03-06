using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using uPalette.Editor.Core.Updater;
using uPalette.Runtime.Core;

namespace uPalette.Editor.Core.Shared
{
    public sealed class UPaletteProjectSettingsProvider : SettingsProvider
    {
        public UPaletteProjectSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null)
            : base(path, scopes, keywords)
        {
        }

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            var keywords = new[] { "uPalette" };
            return new UPaletteProjectSettingsProvider("Project/uPalette", SettingsScope.Project, keywords);
        }

        public override void OnGUI(string searchContext)
        {
            using (new GUIScope())
            {
                var store = PaletteStore.LoadAsset();

                if (store == null)
                {
                    EditorGUILayout.HelpBox(
                        "To use Project Settings, you need to create a PaletteStore to initialize uPalette.",
                        MessageType.Warning);

                    if (GUILayout.Button("Create Palette Store"))
                        PaletteStore.CreateAsset();

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Update Version 1 to 2", GUILayout.Width(EditorGUIUtility.labelWidth));
                        if (GUILayout.Button("Execute"))
                            VersionUpdater.Update1To2();
                    }

                    return;
                }

                var projectSettings = UPaletteProjectSettings.instance;

                using (var ccs = new EditorGUI.ChangeCheckScope())
                {
                    store.MissingEntryErrorLevel =
                        (MissingEntryErrorLevel)EditorGUILayout.EnumPopup("Missing Entry Error",
                            store.MissingEntryErrorLevel);

                    if (ccs.changed)
                        EditorUtility.SetDirty(store);
                }

                using (var ccs = new EditorGUI.ChangeCheckScope())
                {
                    projectSettings.NameEnumsFileGenerateMode =
                        (NameEnumsFileGenerateMode)EditorGUILayout.EnumPopup("Name Enums File Generation",
                            projectSettings.NameEnumsFileGenerateMode);

                    projectSettings.NameEnumsFolder =
                        (DefaultAsset)EditorGUILayout.ObjectField("Name Enums File Location",
                            projectSettings.NameEnumsFolder,
                            typeof(DefaultAsset), false);

                    if (ccs.changed && projectSettings.NameEnumsFileGenerateMode ==
                        NameEnumsFileGenerateMode.WhenWindowLosesFocus)
                        EditorPrefs.SetBool(EditorPrefsKey.IsIdOrNameDirtyPrefsKey, true);
                }
            }
        }

        private sealed class GUIScope : GUI.Scope
        {
            private const float LabelWidth = 250;
            private const float MarginLeft = 10;
            private const float MarginTop = 10;

            private readonly float _labelWidth;

            public GUIScope()
            {
                _labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = LabelWidth;
                GUILayout.BeginHorizontal();
                GUILayout.Space(MarginLeft);
                GUILayout.BeginVertical();
                GUILayout.Space(MarginTop);
            }

            protected override void CloseScope()
            {
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                EditorGUIUtility.labelWidth = _labelWidth;
            }
        }
    }
}
