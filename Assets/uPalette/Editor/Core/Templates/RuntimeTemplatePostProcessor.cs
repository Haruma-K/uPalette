using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;

namespace uPalette.Editor.Core.Templates
{
    public sealed class RuntimeTemplatePostProcessor : AssetPostprocessor
    {
        private static readonly Regex _linePragmaRegex = new Regex("^.*#line.*\n", RegexOptions.Multiline);

        private static void OnPostprocessAllAssets(string[] importedAssetPaths, string[] deletedAssetPaths,
            string[] movedAssetPaths, string[] movedFromAssetPaths)
        {
            foreach (var assetPath in importedAssetPaths)
            {
                if (!assetPath.EndsWith(".cs"))
                    continue;

                if (!assetPath.Contains("/Templates/"))
                    continue;

                if (Path.GetFileNameWithoutExtension(assetPath) == $"{nameof(RuntimeTemplatePostProcessor)}")
                    continue;

                var script = File.ReadAllText(assetPath);
                script = _linePragmaRegex.Replace(script, string.Empty);
                File.WriteAllText(assetPath, script);
            }
        }
    }
}
