using System;
using UnityEditor;
using UnityEngine;
using uPalette.Editor.Foundation;
using uPalette.Editor.Foundation.CommandBasedUndo;
using uPalette.Runtime.Core;
using uPalette.Runtime.Foundation.TinyRx.ObservableProperty;

namespace uPalette.Editor.Core.Shared
{
    internal sealed class EditPaletteStoreService : IDisposable
    {
        private readonly GenerateNameEnumsFileService _generateNameEnumsFileService;
        private readonly AutoIncrementHistory _history = new AutoIncrementHistory();
        private readonly ObservableProperty<bool> _isDirty;
        private readonly PaletteStore _paletteStore;
        private readonly AssetSaveService _saveService = new AssetSaveService();

        private bool _saveReserved;

        public bool IsIdOrNameDirty
        {
            get => EditorPrefs.GetBool(EditorPrefsKey.IsIdOrNameDirtyPrefsKey, false);
            set => EditorPrefs.SetBool(EditorPrefsKey.IsIdOrNameDirtyPrefsKey, value);
        }

        public EditPaletteStoreService(PaletteStore paletteStore,
            GenerateNameEnumsFileService generateNameEnumsFileService)
        {
            _paletteStore = paletteStore;
            _generateNameEnumsFileService = generateNameEnumsFileService;
            _isDirty = new ObservableProperty<bool>(EditorUtility.IsDirty(paletteStore));
            EditorApplication.update += OnUpdate;
        }

        public int SaveIntervalFrame { get; set; } = 10;

        public IReadOnlyObservableProperty<bool> IsDirty => _isDirty;

        public void Dispose()
        {
            if (_saveReserved)
                Save();

            _isDirty.Dispose();
            EditorApplication.update -= OnUpdate;
        }

        public void EditWithoutUndo(Action editAction, bool markAsDirty = true, bool markAsIdOrNameDirty = false,
            bool enableAutoSave = true)
        {
            editAction();

            if (markAsDirty)
                MarkAsDirty();

            if (markAsIdOrNameDirty)
                MarkAsIdOrNameDirty();

            if (enableAutoSave)
                ReserveSave();
        }

        public void Edit(string commandName, Action redoAction, Action undoAction, bool markAsDirty = true,
            bool markAsIdOrNameDirty = false, bool enableAutoSave = true)
        {
            _history.Register(commandName, () =>
            {
                redoAction.Invoke();

                if (markAsDirty)
                    MarkAsDirty();

                if (markAsIdOrNameDirty)
                    MarkAsIdOrNameDirty();

                if (enableAutoSave)
                    ReserveSave();
            }, () =>
            {
                undoAction.Invoke();

                if (markAsDirty)
                    MarkAsDirty();

                if (markAsIdOrNameDirty)
                    MarkAsIdOrNameDirty();

                if (enableAutoSave)
                    ReserveSave();
            });
        }

        public void ReserveSave()
        {
            _saveReserved = true;
        }

        public void Save()
        {
            _saveService.Run(_paletteStore);
        }

        public void GenerateNameEnumsFileIfNeeded()
        {
            if (!IsIdOrNameDirty)
                return;

            _generateNameEnumsFileService.Run();
            ClearIdOrNameDirty();
        }

        public void MarkAsDirty()
        {
            EditorUtility.SetDirty(_paletteStore);
        }

        public void MarkAsIdOrNameDirty()
        {
            IsIdOrNameDirty = true;
        }

        public void Undo()
        {
            _history.Undo();
        }

        public void Redo()
        {
            _history.Redo();
        }

        public void ClearDirty()
        {
            EditorUtility.ClearDirty(_paletteStore);
        }

        public void ClearIdOrNameDirty()
        {
            IsIdOrNameDirty = false;
        }

        private void OnUpdate()
        {
            CheckIsDirty();

            if (_saveReserved && Time.frameCount % SaveIntervalFrame == 0)
            {
                Save();
                _saveReserved = false;
            }
        }

        private void CheckIsDirty()
        {
            var isDirty = EditorUtility.IsDirty(_paletteStore);
            if (isDirty != _isDirty.Value) _isDirty.Value = isDirty;
        }
    }
}
