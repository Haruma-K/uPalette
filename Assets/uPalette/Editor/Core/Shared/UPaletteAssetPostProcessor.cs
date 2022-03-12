using UnityEditor;
using uPalette.Runtime.Core;

namespace uPalette.Editor.Core.Shared
{
    public sealed class UPaletteAssetPostProcessor : AssetPostprocessor
    {
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

                // Reload on import for when assets are changed outside of the application, such as by version control tools.
                EditorApplication.delayCall += ReloadUPaletteEditorApplication;
            }
        }

        private static void ReloadUPaletteEditorApplication()
        {
            var app = UPaletteEditorApplication.RequestInstance();
            app.Reload();
            UPaletteEditorApplication.ReleaseInstance();
        }
    }
}
