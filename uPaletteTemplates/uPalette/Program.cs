using System;
using uPalette.Editor.Core.Templates;

namespace uPalette
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var input = new NameEnumsTemplateInput();

            // Color
            var colorPaletteData = new NameEnumsTemplateInput.PaletteData("Color");
            colorPaletteData.AddThemeInfo("Theme1", "123");
            colorPaletteData.AddThemeInfo("Theme2", "456");
            colorPaletteData.AddEntryInfo("KeyColor", "789");
            colorPaletteData.AddEntryInfo("KeyColor", "123");
            colorPaletteData.AddEntryInfo("KeyColor_2", "123");
            input.PaletteDataList.Add(colorPaletteData);

            // Character Style
            var characterStylePaletteData = new NameEnumsTemplateInput.PaletteData("CharacterStyle");
            characterStylePaletteData.AddThemeInfo("Theme1", "789");
            input.PaletteDataList.Add(characterStylePaletteData);

            var template = new NameEnumsTemplate(input);
            Console.WriteLine(template.TransformText());
        }
    }
}
