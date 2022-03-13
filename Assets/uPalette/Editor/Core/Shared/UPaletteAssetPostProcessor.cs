using UnityEditor;
using UnityEditorInternal;
using uPalette.Runtime.Core;

namespace uPalette.Editor.Core.Shared
{
    public sealed class UPaletteAssetPostProcessor : AssetPostprocessor
    {
        private static bool _isUnityEditorFocused;
        private static bool _needReloading;

        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            var paletteStore = PaletteStore.LoadAsset();
            if (paletteStore == null)
                return;

            var assetPath = AssetDatabase.GetAssetPath(paletteStore);
            foreach (var importedAsset in importedAssets)
            {
                if (assetPath != importedAsset)
                    continue;

                _needReloading = false;
                EditorApplication.delayCall += OnPaletteStoreImported;
                return;
            }
        }

        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            EditorApplication.update += Update;
        }

        private static void Update()
        {
            if (!_isUnityEditorFocused && InternalEditorUtility.isApplicationActive)
            {
                _isUnityEditorFocused = InternalEditorUtility.isApplicationActive;
                OnApplicationFocused();
            }
            else if (_isUnityEditorFocused && !InternalEditorUtility.isApplicationActive)
            {
                _isUnityEditorFocused = InternalEditorUtility.isApplicationActive;
            }
        }

        private static void OnApplicationFocused()
        {
            _needReloading = true;
        }

        private static void OnPaletteStoreImported()
        {
            if (!_needReloading)
                return;

            // Reload on import for when assets are changed outside of the application, such as by version control tools.
            var app = UPaletteEditorApplication.RequestInstance();
            app.Reload();
            UPaletteEditorApplication.ReleaseInstance();
        }
    }
}
