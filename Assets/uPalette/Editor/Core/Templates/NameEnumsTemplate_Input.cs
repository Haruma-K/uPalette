using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace uPalette.Editor.Core.Templates
{
    public sealed class NameEnumsTemplateInput
    {
        public readonly List<PaletteData> PaletteDataList = new List<PaletteData>();

        public sealed class PaletteData
        {
            private readonly List<(string name, string id)> _entryInfos = new List<(string name, string id)>();
            private readonly List<(string name, string id)> _themeInfos = new List<(string name, string id)>();

            public PaletteData(string typeName)
            {
                TypeName = typeName;
            }

            public IReadOnlyList<(string name, string id)> ThemeInfos => _themeInfos;
            public IReadOnlyList<(string name, string id)> EntryInfos => _entryInfos;

            public string TypeName { get; }

            public void AddThemeInfo(string name, string id)
            {
                _themeInfos.Add((name, id));
            }

            public void AddEntryInfo(string name, string id)
            {
                _entryInfos.Add((name, id));
            }
        }
    }

    public partial class NameEnumsTemplate
    {
        private readonly NameEnumsTemplateInput _input;

        public NameEnumsTemplate(NameEnumsTemplateInput input)
        {
            _input = input;
        }
    }
}
