using System;
using UnityEngine;
using uPalette.Runtime.Core.Model;
using uPalette.Runtime.Foundation.CharacterStyles;
#if UNITY_EDITOR
using System.IO;
using System.Linq;
using UnityEditor;
#endif

namespace uPalette.Runtime.Core
{
    [Serializable]
    public sealed class PaletteStore : ScriptableObject
    {
        private static PaletteStore _instance;

        [SerializeField] private ColorPalette _colorPalette = new ColorPalette();
        [SerializeField] private GradientPalette _gradientPalette = new GradientPalette();
        [SerializeField] private CharacterStylePalette _characterStylePalette = new CharacterStylePalette();
        [SerializeField] private CharacterStyleTMPPalette _characterStyleTMPPalette = new CharacterStyleTMPPalette();
        [SerializeField] private MissingEntryErrorLevel _missingEntryErrorLevel = MissingEntryErrorLevel.Warning;

        public Palette<Color> ColorPalette => _colorPalette;
        public Palette<Gradient> GradientPalette => _gradientPalette;
        public Palette<CharacterStyle> CharacterStylePalette => _characterStylePalette;
        public Palette<CharacterStyleTMP> CharacterStyleTMPPalette => _characterStyleTMPPalette;

        public MissingEntryErrorLevel MissingEntryErrorLevel
        {
            get => _missingEntryErrorLevel;
            set => _missingEntryErrorLevel = value;
        }

        public static PaletteStore Instance
        {
            get
            {
#if UNITY_EDITOR
                if (_instance == null)
                    _instance = LoadAsset();
#else
                if (_instance == null)
                    _instance = CreateInstance<PaletteStore>();
#endif
                return _instance;
            }
        }

        private void OnEnable()
        {
#if !UNITY_EDITOR
            _instance = this;
#endif
        }

#if UNITY_EDITOR
        internal static PaletteStore LoadAsset()
        {
            var asset = PlayerSettings.GetPreloadedAssets().OfType<PaletteStore>().FirstOrDefault();
            return asset;
        }

        internal static PaletteStore CreateAsset()
        {
            var asset = PlayerSettings.GetPreloadedAssets().OfType<PaletteStore>().FirstOrDefault();
            if (asset != null)
            {
                var path = AssetDatabase.GetAssetPath(asset);
                throw new InvalidOperationException($"{nameof(PaletteStore)} already exists at {path}");
            }

            var assetPath = EditorUtility.SaveFilePanelInProject($"Save {nameof(PaletteStore)}",
                nameof(PaletteStore),
                "asset", "", "Assets");

            if (string.IsNullOrEmpty(assetPath))
                // Return if canceled.
                return null;
            
            return CreateAsset(assetPath);
        }

        internal static PaletteStore CreateAsset(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                throw new ArgumentNullException(nameof(assetPath));
            }

            // Create folders if needed.
            var folderPath = Path.GetDirectoryName(assetPath);
            if (!string.IsNullOrEmpty(folderPath) && !Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var instance = CreateInstance<PaletteStore>();
            AssetDatabase.CreateAsset(instance, assetPath);
            var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            preloadedAssets.Add(instance);
            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
            AssetDatabase.SaveAssets();

            return instance;
        }

        internal static void RemoveAsset()
        {
            var preloadedAssetList = PlayerSettings.GetPreloadedAssets().ToList();
            var asset = preloadedAssetList.FirstOrDefault();
            
            if (asset == null)
                return;

            preloadedAssetList.Remove(asset);
            PlayerSettings.SetPreloadedAssets(preloadedAssetList.ToArray());
            DestroyImmediate(asset, true);
        }
#endif
    }
}
