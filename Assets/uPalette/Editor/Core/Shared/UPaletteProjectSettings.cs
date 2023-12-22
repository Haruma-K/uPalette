using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using uPalette.Runtime.Core;

namespace uPalette.Editor.Core.Shared
{
    [FilePath("ProjectSettings/uPaletteSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public sealed class UPaletteProjectSettings : ScriptableSingleton<UPaletteProjectSettings>
    {
        [SerializeField]
        private NameEnumsFileGenerateMode _nameEnumsFileGenerateMode = NameEnumsFileGenerateMode.Manual;

        [SerializeField] private bool _containsFolderNameToNameEnums = true;

        [FormerlySerializedAs("_useFolderMode")] [SerializeField]
        private bool useFolderViewInPaletteEditor = true;

        [SerializeField] private DefaultAsset _nameEnumsFolder;
        [SerializeField] private MonoScript _nameEnumsFile;
        [SerializeField] private bool _automaticRuntimeDataLoading = true;

        public char FolderDelimiter => '/';

        public bool ContainsFolderNameToNameEnums
        {
            get => _containsFolderNameToNameEnums;
            set
            {
                if (value != _containsFolderNameToNameEnums)
                {
                    _containsFolderNameToNameEnums = value;
                    Save(true);
                }
            }
        }

        public bool UseFolderViewInPaletteEditor
        {
            get => useFolderViewInPaletteEditor;
            set
            {
                if (value != useFolderViewInPaletteEditor)
                {
                    useFolderViewInPaletteEditor = value;
                    Save(true);
                }
            }
        }

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

        public bool AutomaticRuntimeDataLoading
        {
            get => _automaticRuntimeDataLoading;
            set
            {
                if (value != _automaticRuntimeDataLoading)
                {
                    if (value)
                        PaletteStore.RegisterToPreloadedAssets();
                    else
                        PaletteStore.UnregisterFromPreloadedAssets();


                    _automaticRuntimeDataLoading = value;
                    Save(true);
                }
            }
        }
    }
}