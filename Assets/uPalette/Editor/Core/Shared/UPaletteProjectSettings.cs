using UnityEditor;
using UnityEngine;

namespace uPalette.Editor.Core.Shared
{
    [FilePath("ProjectSettings/uPaletteSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public sealed class UPaletteProjectSettings : ScriptableSingleton<UPaletteProjectSettings>
    {
        [SerializeField]
        private NameEnumsFileGenerateMode _nameEnumsFileGenerateMode = NameEnumsFileGenerateMode.Manual;

        [SerializeField] private DefaultAsset _nameEnumsFolder;
        [SerializeField] private MonoScript _nameEnumsFile;

        public NameEnumsFileGenerateMode NameEnumsFileGenerateMode
        {
            get => _nameEnumsFileGenerateMode;
            set
            {
                if (value != _nameEnumsFileGenerateMode)
                {
                    _nameEnumsFileGenerateMode = value;
                    Save(true);
                }
            }
        }

        public DefaultAsset NameEnumsFolder
        {
            get => _nameEnumsFolder;
            set
            {
                if (value != _nameEnumsFolder)
                {
                    _nameEnumsFolder = value;
                    Save(true);
                }
            }
        }

        public MonoScript NameEnumsFile
        {
            get => _nameEnumsFile;
            set
            {
                if (value != _nameEnumsFile)
                {
                    _nameEnumsFile = value;
                    Save(true);
                }
            }
        }
    }
}
