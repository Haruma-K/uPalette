using System.Collections.Generic;
using System.Linq;
using TMPro;
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
                var projectSettings = UPaletteProjectSettings.instance;

                if (store == null)
                {
                    EditorGUILayout.HelpBox(
                        "To use Project Settings, you need to create a PaletteStore to initialize uPalette.",
                        MessageType.Warning);

                    if (GUILayout.Button("Create Palette Store"))
                        PaletteStore.CreateAsset(projectSettings.AutomaticRuntimeDataLoading);

                    if (GUILayout.Button("Update Version 1 to 2"))
                        VersionUpdater.Update1To2();

                    return;
                }

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

                    projectSettings.ContainsFolderNameToNameEnums = EditorGUILayout.Toggle(
                        "Contains Folder Name to Name Enums",
                        projectSettings.ContainsFolderNameToNameEnums);

                    projectSettings.NameEnumsFolder =
                        (DefaultAsset)EditorGUILayout.ObjectField("Name Enums File Location",
                            projectSettings.NameEnumsFolder,
                            typeof(DefaultAsset), false);

                    if (ccs.changed && projectSettings.NameEnumsFileGenerateMode ==
                        NameEnumsFileGenerateMode.WhenWindowLosesFocus)
                        EditorPrefs.SetBool(EditorPrefsKey.IsIdOrNameDirtyPrefsKey, true);
                }

                projectSettings.UseFolderViewInPaletteEditor = EditorGUILayout.Toggle(
                    "Use Folder View in Palette Editor",
                    projectSettings.UseFolderViewInPaletteEditor);

                projectSettings.AutomaticRuntimeDataLoading = EditorGUILayout.Toggle("Automatic Runtime Data Loading",
                    projectSettings.AutomaticRuntimeDataLoading);

                if (projectSettings.AutomaticRuntimeDataLoading && !PlayerSettings.GetPreloadedAssets().Contains(store))
                    EditorGUILayout.HelpBox(
                        $"\"Automatic Runtime Data Loading\" is turned on, but the preloaded assets does not contains the \"{nameof(PaletteStore)}\". To fix it you need to add the \"{nameof(PaletteStore)}\" to the preloaded assets manually.",
                        MessageType.Error);

                if (!projectSettings.AutomaticRuntimeDataLoading)
                    EditorGUILayout.HelpBox(
                        $"\"Automatic Runtime Data Loading\" is turned off, so you must load the \"{nameof(PaletteStore)}\" manually before loading GUIs that use uPalette.",
                        MessageType.Warning);

                // This is an implementation for backward compatibility.
                var message =
                    "Enable TextMeshPro Auto Size Options for all data and set the initial value for each option. Is it OK?";
                if (GUILayout.Button("Enable TextMeshPro Auto Size Options")
                    && EditorUtility.DisplayDialog("Enable TextMeshPro Auto Size Options", message, "OK", "Cancel"))
                {
                    foreach (var entry in store.CharacterStyleTMPPalette.Entries.Values)
                    foreach (var value in entry.Values.Values)
                    {
                        if (value.Value.enableAutoSizeOptions)
                            continue;

                        var style = value.Value;
                        style.enableAutoSizeOptions = true;
                        style.fontSizeMin = TMP_Settings.defaultFontSize * TMP_Settings.defaultTextAutoSizingMinRatio;
                        style.fontSizeMax = TMP_Settings.defaultFontSize * TMP_Settings.defaultTextAutoSizingMaxRatio;
                        style.characterWidthAdjustment = 0.0f;
                        style.lineSpacingAdjustment = 0.0f;
                        value.Value = style;
                    }

                    EditorUtility.SetDirty(store);
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
