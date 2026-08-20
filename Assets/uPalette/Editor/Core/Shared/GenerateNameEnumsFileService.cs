using UnityEditor;
using uPalette.Editor.Core.Templates;
using uPalette.Editor.Foundation.LocalPersistence;
using uPalette.Runtime.Core;
using uPalette.Runtime.Core.Model;

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
            ClearDirty();
        }

        internal void RunIfNeeded()
        {
            if (!IsDirty)
                return;

            EditorUtility.DisplayProgressBar("Processing", "Generating Name Enum File...", 0.0f);
            try
            {
                Run();
            }
            finally
            {
                EditorUtility.DisplayProgressBar("Processing", "Generating Name Enum File...", 1.0f);
                EditorUtility.ClearProgressBar();
            }
        }

        internal void MarkDirty()
        {
            IsDirty = true;
        }

        internal void ClearDirty()
        {
            IsDirty = false;
        }

        internal bool IsDirty
        {
            get => EditorPrefs.GetBool(EditorPrefsKey.IsIdOrNameDirtyPrefsKey, false);
            set => EditorPrefs.SetBool(EditorPrefsKey.IsIdOrNameDirtyPrefsKey, value);
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
            var settings = UPaletteProjectSettings.instance;
            var folderDelimiter = settings.FolderDelimiter;
            var containsFolderNameToNameEnums = settings.ContainsFolderNameToNameEnums;

            return CreatePaletteData(
                typeName,
                palette,
                folderDelimiter,
                containsFolderNameToNameEnums,
                settings.UseFolderViewInPaletteEditor);
        }

        internal static NameEnumsTemplateInput.PaletteData CreatePaletteData<T>(
            string typeName,
            Palette<T> palette,
            char folderDelimiter,
            bool containsFolderNameToNameEnums,
            bool useFolderViewInPaletteEditor
        )
        {
            var paletteData = new NameEnumsTemplateInput.PaletteData(typeName);

            foreach (var idAndName in palette.GetThemeIdAndNames(folderDelimiter, containsFolderNameToNameEnums))
                paletteData.AddThemeInfo(idAndName.name, idAndName.id);

            var entries = PaletteEntryOrdering.GetOrderedEntries(
                palette,
                useFolderViewInPaletteEditor,
                folderDelimiter);
            foreach (var idAndName in palette.GetEntryIdAndNames(
                         entries,
                         folderDelimiter,
                         containsFolderNameToNameEnums))
                paletteData.AddEntryInfo(idAndName.name, idAndName.id);

            return paletteData;
        }
    }
}
