using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using uPalette.Runtime.Core;
using uPalette.Runtime.Foundation.TinyRx.ObservableCollection;
using uPalette.Runtime.Foundation.TinyRx.ObservableProperty;

namespace uPalette.Editor.Core.Updater
{
    internal static class VersionUpdater
    {
        private const string AppDataFilePath = "Assets/StreamingAssets/uPalette/AppData.json";

        public static void Update1To2()
        {
            if (!EditorUtility.DisplayDialog("Warning",
                    "Execute version update and reload the active scene. Do you want to run it?", "OK", "Cancel"))
                return;

            if (!File.Exists(AppDataFilePath))
            {
                EditorUtility.DisplayDialog("Error", $"The target file does not exists at {AppDataFilePath}", "OK");
                return;
            }

            var json = File.ReadAllText(AppDataFilePath);
            var obsoleteStore = JsonUtility.FromJson<UPaletteStore>(json);

            var palette = PaletteStore.LoadAsset();
            if (palette != null)
            {
                if (!EditorUtility.DisplayDialog("Warning",
                        $"{nameof(PaletteStore)} already exists. Do you want to overwrite it?", "OK", "Cancel"))
                    return;

                var assetPath = AssetDatabase.GetAssetPath(palette);
                PaletteStore.RemoveAsset();
                palette = PaletteStore.CreateAsset(assetPath);
            }
            else
            {
                // Open save file panel and create PaletteStore asset.
                palette = PaletteStore.CreateAsset();

                // If cancelled, do nothing.
                if (palette == null)
                    return;
            }

            var theme = palette.ColorPalette.ActiveTheme.Value;
            foreach (var obsoleteEntry in obsoleteStore.Entries)
            {
                var entry = palette.ColorPalette.AddEntry(obsoleteEntry.ID);
                entry.Name.Value = obsoleteEntry.Name.Value;
                entry.Values[theme.Id].Value = obsoleteEntry.Value.Value;
            }

            EditorUtility.SetDirty(palette);
            AssetDatabase.SaveAssets();
            AssetDatabase.DeleteAsset(AppDataFilePath);

            // Restart the current scene.
            EditorSceneManager.OpenScene(SceneManager.GetActiveScene().path, OpenSceneMode.Single);
        }

        [Serializable]
        public sealed class UPaletteStore
        {
            [SerializeField] private ObservableList<ColorEntry> _entries = new ObservableList<ColorEntry>();

            public ObservableList<ColorEntry> Entries => _entries;
            public ObservableProperty<bool> IsDirty { get; } = new ObservableProperty<bool>();
        }

        [Serializable]
        public sealed class ColorEntry
        {
            [SerializeField] private string _id;
            [SerializeField] private ObservableProperty<string> _name = new ObservableProperty<string>();
            [SerializeField] private ObservableProperty<Color> _value = new ColorObservableProperty();

            public ColorEntry(string name = null, Color? color = null)
            {
                _id = Guid.NewGuid().ToString();
                if (!string.IsNullOrEmpty(name)) _name.Value = name;

                SetColor(color ?? Color.white);
            }

            public string ID => _id;

            public ObservableProperty<string> Name => _name;

            public IReadOnlyObservableProperty<Color> Value => _value;

            public void SetColor(Color color)
            {
                _value.Value = color;
                if (string.IsNullOrEmpty(Name.Value)
                    || Name.Value.StartsWith("#") && ColorUtility.TryParseHtmlString(Name.Value, out _))
                    Name.Value = $"#{ColorUtility.ToHtmlStringRGBA(_value.Value)}";
            }
        }
    }
}
