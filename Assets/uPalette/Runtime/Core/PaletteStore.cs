using System;
using UnityEngine;
using uPalette.Runtime.Core.Model;
using uPalette.Runtime.Foundation.CharacterStyles;
using Object = UnityEngine.Object;
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
            var asset = LoadFromAssetDatabase<PaletteStore>();
            return asset;
        }

        internal static PaletteStore CreateAsset(bool registerToPreloadedAssets)
        {
            var asset = LoadFromAssetDatabase<PaletteStore>();

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

            return CreateAsset(assetPath, registerToPreloadedAssets);
        }

        internal static PaletteStore CreateAsset(string assetPath, bool registerToPreloadedAssets)
        {
            if (string.IsNullOrEmpty(assetPath)) throw new ArgumentNullException(nameof(assetPath));

            // Create folders if needed.
            var folderPath = Path.GetDirectoryName(assetPath);
            if (!string.IsNullOrEmpty(folderPath) && !Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var instance = CreateInstance<PaletteStore>();
            AssetDatabase.CreateAsset(instance, assetPath);
            AssetDatabase.SaveAssets();
            
            if (registerToPreloadedAssets)
                RegisterToPreloadedAssets();

            return instance;
        }

        internal static void RemoveAsset()
        {
            var asset = LoadFromAssetDatabase<PaletteStore>();
            if (asset == null)
                return;

            UnregisterFromPreloadedAssets();

            DestroyImmediate(asset, true);
        }

        internal static void RegisterToPreloadedAssets(bool saveAsset = true)
        {
            var asset = LoadFromAssetDatabase<PaletteStore>();

            if (asset == null)
                throw new InvalidOperationException($"{nameof(PaletteStore)} does not exists.");

            var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            if (preloadedAssets.Contains(asset))
                return;
            
            preloadedAssets.Add(asset);
            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
            if (saveAsset)
                AssetDatabase.SaveAssets();
        }
        
        internal static void UnregisterFromPreloadedAssets(bool saveAsset = true)
        {
            var asset = LoadFromAssetDatabase<PaletteStore>();

            if (asset == null)
                throw new InvalidOperationException($"{nameof(PaletteStore)} does not exists.");

            var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            if (!preloadedAssets.Contains(asset))
                return;
            
            preloadedAssets.Remove(asset);
            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
            if (saveAsset)
                AssetDatabase.SaveAssets();
        }

        private static T LoadFromAssetDatabase<T>() where T : Object
        {
            var asset = AssetDatabase.FindAssets($"t:{typeof(T).Name}")
                .Select(x =>
                {
                    var path = AssetDatabase.GUIDToAssetPath(x);
                    return AssetDatabase.LoadAssetAtPath<T>(path);
                })
                .FirstOrDefault();
            return asset;
        }
#endif
    }
}
