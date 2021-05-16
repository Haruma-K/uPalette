using System;
using uPalette.Runtime.Core;

namespace uPalette.Editor.Core
{
    public class UPaletteEditorApplication : IDisposable
    {
        private static int _referenceCount;
        private static UPaletteEditorApplication _instance;

        private UPaletteEditorApplication()
        {
            var runtimeApplication = UPaletteApplication.RequestInstance();
            var store = runtimeApplication.UPaletteStore;
            ColorEntryEditorPresenter = new ColorEntryEditorPresenter(store);
            ColorEntryEditorController = new ColorEntryEditorController(store);
        }

        public ColorEntryEditorPresenter ColorEntryEditorPresenter { get; }
        public ColorEntryEditorController ColorEntryEditorController { get; }

        public void Dispose()
        {
            ColorEntryEditorPresenter.Dispose();
            ColorEntryEditorController.Dispose();
            UPaletteApplication.ReleaseInstance();
        }

        public static UPaletteEditorApplication RequestInstance()
        {
            if (_referenceCount++ == 0)
            {
                _instance = new UPaletteEditorApplication();
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