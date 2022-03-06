using UnityEditor;
using uPalette.Editor.Core.Templates;
using uPalette.Runtime.Core;
using uPalette.Runtime.Core.Model;
using uPalette.Runtime.Foundation.LocalPersistence;

namespace uPalette.Editor.Core.Shared
{
    public sealed class GenerateNameEnumsFileService
    {
        private readonly PaletteStore _store;

        public GenerateNameEnumsFileService(PaletteStore store)
        {
            _store = store;
        }

        public void Run()
        {
            var settings = UPaletteProjectSettings.instance;
            var folderPath = settings.NameEnumsFolder == null
                ? "Assets"
                : AssetDatabase.GetAssetPath(settings.NameEnumsFolder);
            var filePath = $"{folderPath}/NameEnums.cs";
            RunInternal(filePath);
        }

        private void RunInternal(string filePath)
        {
            var settings = UPaletteProjectSettings.instance;

            // Delete the old file if needed.
            if (settings.NameEnumsFile != null)
            {
                var oldFilePath = AssetDatabase.GetAssetPath(settings.NameEnumsFile);
                if (oldFilePath != filePath)
                {
                    var lp = new TextLocalPersistence(oldFilePath);
                    if (lp.Exists())
                        lp.Delete();
                }
            }

            var template = new NameEnumsTemplate(CreateTemplateInput(_store));
            var text = template.TransformText();
            var localPersistence = new TextLocalPersistence(filePath);
            localPersistence.Save(text);

            settings.NameEnumsFile = AssetDatabase.LoadAssetAtPath<MonoScript>(filePath);
        }

        private static NameEnumsTemplateInput CreateTemplateInput(PaletteStore store)
        {
            var input = new NameEnumsTemplateInput();

            var colorPaletteData = CreatePaletteData("Color", store.ColorPalette);
            input.PaletteDataList.Add(colorPaletteData);
            var gradientPaletteData = CreatePaletteData("Gradient", store.GradientPalette);
            input.PaletteDataList.Add(gradientPaletteData);
            var characterStylePaletteData = CreatePaletteData("CharacterStyle", store.CharacterStylePalette);
            input.PaletteDataList.Add(characterStylePaletteData);
            var characterStyleTMPPaletteData = CreatePaletteData("CharacterStyleTMP", store.CharacterStyleTMPPalette);
            input.PaletteDataList.Add(characterStyleTMPPaletteData);

            return input;
        }

        private static NameEnumsTemplateInput.PaletteData CreatePaletteData<T>(string typeName, Palette<T> palette)
        {
            var paletteData = new NameEnumsTemplateInput.PaletteData(typeName);

            foreach (var idAndName in palette.GetThemeIdAndNames())
                paletteData.AddThemeInfo(idAndName.name, idAndName.id);

            foreach (var idAndName in palette.GetEntryIdAndNames())
                paletteData.AddEntryInfo(idAndName.name, idAndName.id);

            return paletteData;
        }
    }
}
