using System;
using UnityEngine;
using uPalette.Runtime.Foundation.LocalPersistence;
using uPalette.Runtime.Foundation.Observable;

namespace uPalette.Runtime.Core
{
    public class UPaletteApplication : IDisposable
    {
        private static int _referenceCount;
        private static UPaletteApplication _instance;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly ILocalPersistence<UPaletteStore> _persistence;

        private UPaletteApplication()
        {
            var folderPath = $"{Application.streamingAssetsPath}/uPalette";
            const string fileName = "AppData";
            var persistence = new UnityJsonLocalPersistence<UPaletteStore>(folderPath, fileName);
            persistence.PrettyPrint = true;
            _persistence = persistence;
#if UNITY_EDITOR
            if (!_persistence.Exists())
            {
                UPaletteStore = new UPaletteStore();
                Save();
            }
#endif

            UPaletteStore = _persistence.Load();

#if UNITY_EDITOR
            UPaletteStore.IsDirty.Subscribe(x =>
            {
                if (x)
                {
                    Save();
                }
            }).DisposeWith(_disposables);
#endif
        }

        public UPaletteStore UPaletteStore { get; }

        public void Dispose()
        {
#if UNITY_EDITOR
            Save();
            _disposables.Dispose();
#endif
        }

#if UNITY_EDITOR
        public void Save()
        {
            _persistence.Save(UPaletteStore);
            UPaletteStore.IsDirty.Value = false;
        }
#endif

        public static UPaletteApplication RequestInstance()
        {
            if (_referenceCount++ == 0)
            {
                _instance = new UPaletteApplication();
            }

            return _instance;
        }

        public static void ReleaseInstance()
        {
            if (--_referenceCount == 0)
            {
                _instance.Dispose();
                _instance = null;
            }
        }
    }
}
