using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using uPalette.Runtime.Foundation;
using uPalette.Runtime.Foundation.TinyRx;
using uPalette.Runtime.Foundation.TinyRx.ObservableCollection;
using uPalette.Runtime.Foundation.TinyRx.ObservableProperty;

namespace uPalette.Runtime.Core.Model
{
    [Serializable]
    public abstract class Palette<T> : ISerializationCallbackReceiver
    {
        private const string DefaultThemeName = "Default";

        private static readonly Regex _nonDuplicatedNameSuffixRegex = new Regex("_([0-9]+)");

        [SerializeField]
        private ObservableDictionary<string, Entry<T>> _entries = new ObservableDictionary<string, Entry<T>>();

        [SerializeField]
        private ObservableDictionary<string, Theme> _themes = new ObservableDictionary<string, Theme>();

        [SerializeField] private OrderCollection<string> _entryOrders = new OrderCollection<string>();

        [SerializeField] private OrderCollection<string> _themeOrders = new OrderCollection<string>();

        // NOTE: Serialize _activeThemeId instead of _activeTheme as an ad-hoc solution to a serialization bug (probably in Unity).
        [SerializeField] private string _activeThemeId;

        private readonly CompositeDisposable _activeThemeDisposables = new CompositeDisposable();

        private readonly Dictionary<string, ObservableProperty<T>> _activeValues =
            new Dictionary<string, ObservableProperty<T>>();

        private readonly Subject<(string entryId, int index)> _entryOrderChangedSubject =
            new Subject<(string entryId, int index)>();

        private readonly Subject<(string themeId, int index)> _themeOrderChangedSubject =
            new Subject<(string themeId, int index)>();

        private ObservableProperty<Theme> _activeTheme = new ObservableProperty<Theme>();

        private Dictionary<string, RemovedEntryInfo> _removedEntryInfos = new Dictionary<string, RemovedEntryInfo>();
        private Dictionary<string, RemovedThemeInfo> _removedThemeInfos = new Dictionary<string, RemovedThemeInfo>();

        protected Palette()
        {
            var theme = AddTheme();
            theme.Name.Value = DefaultThemeName;
            SetActiveTheme(theme.Id);
        }

        public IObservable<(string entryId, int index)> EntryOrderChangedAsObservable => _entryOrderChangedSubject;
        public IObservable<(string themeId, int index)> ThemeOrderChangedAsObservable => _themeOrderChangedSubject;

        public IReadOnlyObservableProperty<Theme> ActiveTheme => _activeTheme;
        public IReadOnlyObservableDictionary<string, Theme> Themes => _themes;
        public IReadOnlyObservableDictionary<string, Entry<T>> Entries => _entries;

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            _activeThemeId = _activeTheme.Value.Id;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _activeTheme = new ObservableProperty<Theme>(_themes[_activeThemeId]);
            var activeThemeId = _activeTheme.Value.Id;

            foreach (var entry in _entries.Values)
            {
                var value = entry.Values[activeThemeId];
                var colorProperty = new ObservableProperty<T>();
                _activeValues[entry.Id] = colorProperty;
                value.Subscribe(x => { colorProperty.SetValueAndNotify(x); })
                    .DisposeWith(_activeThemeDisposables);
            }

            SetActiveTheme(_activeTheme.Value.Id);
        }

        public IReadOnlyObservableProperty<T> GetActiveValue(string entryId)
        {
            return _activeValues[entryId];
        }

        public bool TryGetActiveValue(string entryId, out IReadOnlyObservableProperty<T> value)
        {
            if (_activeValues.TryGetValue(entryId, out var v))
            {
                value = v;
                return true;
            }

            value = null;
            return false;
        }

        public void SetActiveTheme(string themeId)
        {
            if (string.IsNullOrEmpty(themeId))
                throw new ArgumentNullException(nameof(themeId));

            if (!_themes.ContainsKey(themeId))
                throw new InvalidOperationException($"The theme with ID {themeId} does not exist.");

            if (_activeTheme.Value != null && themeId == _activeTheme.Value.Id)
                return;

            _activeThemeDisposables.Clear();

            // Synchronize with active values.
            foreach (var entry in _entries.Values)
            {
                var valueProperty = _activeValues[entry.Id];
                entry.Values[themeId]
                    .Subscribe(x => valueProperty.SetValueAndNotify(x))
                    .DisposeWith(_activeThemeDisposables);
            }

            var theme = _themes[themeId];
            _activeTheme.Value = theme;
        }

        public bool HasTheme(string themeId)
        {
            return _themes.ContainsKey(themeId);
        }

        public int GetThemeOrder(string themeId)
        {
            return _themeOrders.GetIndex(themeId);
        }

        public void SetThemeOrder(string themeId, int index)
        {
            _themeOrders.SetIndex(themeId, index);
            _themeOrderChangedSubject.OnNext((themeId, index));
        }

        /// <summary>
        ///     Add a new theme.
        /// </summary>
        /// <returns></returns>
        public Theme AddTheme()
        {
            var theme = new Theme();

            _themeOrders.Add(theme.Id);
            _themes.Add(theme.Id, theme);

            // Add values for a new theme to each entry.
            foreach (var entry in _entries.Values)
                entry.AddValue(theme.Id, GetDefaultValue());

            return theme;
        }

        /// <summary>
        ///     Remove a theme.
        /// </summary>
        /// <param name="themeId"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void RemoveTheme(string themeId)
        {
            if (_themes.Count == 1) throw new InvalidOperationException("You cannot delete all themes.");

            var theme = _themes[themeId];
            theme.Dispose();

            var index = _themeOrders.GetIndex(themeId);
            var previousThemeId = index == 0 ? null : _themeOrders.GetElementAt(index - 1);
            var isActiveTheme = _activeTheme.Value != null && themeId == _activeTheme.Value.Id;
            var entryValues = new Dictionary<string, T>();

            // Remove all entries for the theme.
            foreach (var entry in _entries.Values)
            {
                entryValues.Add(entry.Id, entry.Values[themeId].Value);
                entry.RemoveValue(themeId);
            }

            // If the active theme was removed, set the other theme to active.
            if (isActiveTheme)
                foreach (var t in _themes.Values)
                {
                    if (t.Id == themeId) continue;

                    SetActiveTheme(t.Id);
                    break;
                }

            var removedThemeInfo = new RemovedThemeInfo(theme, previousThemeId, isActiveTheme, entryValues);
            _removedThemeInfos.Add(theme.Id, removedThemeInfo);
            _themeOrders.Remove(themeId);
            _themes.Remove(themeId);
        }

        public Theme RestoreTheme(string removedThemeId)
        {
            var removedThemeInfo = _removedThemeInfos[removedThemeId];
            var removedTheme = removedThemeInfo.Theme;

            int themeIndex;
            if (string.IsNullOrEmpty(removedThemeInfo.PreviousThemeId))
            {
                // If the previous theme not exists, it is the first entry.
                themeIndex = 0;
            }
            else
            {
                if (!_themeOrders.TryGetIndex(removedThemeInfo.PreviousThemeId, out var previousThemeIndex))
                    previousThemeIndex = -1;

                // If the previous theme was already removed, insert this theme to the last.
                themeIndex = previousThemeIndex == -1 ? _themes.Count : previousThemeIndex + 1;
            }

            // Create new theme and copy the values.
            var theme = new Theme(removedThemeId);
            theme.Name.Value = removedTheme.Name.Value;

            // Add values for this theme to each entry.
            foreach (var entry in _entries.Values)
                if (removedThemeInfo.EntryValues.TryGetValue(entry.Id, out var v))
                {
                    var value = entry.AddValue(removedThemeId, GetDefaultValue());
                    value.Value = v;
                }
                else
                {
                    entry.AddValue(theme.Id, GetDefaultValue());
                }

            _removedThemeInfos.Remove(removedThemeId);
            _themeOrders.Add(removedThemeId);
            _themeOrders.SetIndex(removedThemeId, themeIndex);
            _themeOrderChangedSubject.OnNext((removedThemeId, themeIndex));
            _themes.Add(removedThemeId, theme);

            if (removedThemeInfo.WasActiveTheme)
                SetActiveTheme(removedThemeId);

            return theme;
        }

        public bool HasEntry(string entryId)
        {
            return _activeValues.ContainsKey(entryId);
        }

        public int GetEntryOrder(string entryId)
        {
            return _entryOrders.GetIndex(entryId);
        }

        public void SetEntryOrder(string entryId, int index)
        {
            _entryOrders.SetIndex(entryId, index);
            _entryOrderChangedSubject.OnNext((entryId, index));
        }

        /// <summary>
        ///     Add a new entry.
        /// </summary>
        /// <returns></returns>
        public Entry<T> AddEntry()
        {
            return AddEntry(null);
        }

        internal Entry<T> AddEntry(string id)
        {
            var entry = string.IsNullOrEmpty(id) ? new Entry<T>() : new Entry<T>(id);
            // Create a value for each theme.
            foreach (var themeId in _themes.Keys)
            {
                var value = entry.AddValue(themeId, GetDefaultValue());

                if (_activeTheme.Value != null && themeId == _activeTheme.Value.Id)
                {
                    var colorProperty = new ObservableProperty<T>();
                    _activeValues[entry.Id] = colorProperty;
                    value.Subscribe(x => { colorProperty.SetValueAndNotify(x); })
                        .DisposeWith(_activeThemeDisposables);
                }
            }

            _entryOrders.Add(entry.Id);
            _entries.Add(entry.Id, entry);
            return entry;
        }

        /// <summary>
        ///     Remove a entry.
        /// </summary>
        /// <param name="entryId"></param>
        public void RemoveEntry(string entryId)
        {
            var entry = _entries[entryId];
            entry.Dispose();

            // Create a remove entry info.
            var index = _entryOrders.GetIndex(entryId);
            var previousEntryId = index == 0 ? null : _entryOrders.GetElementAt(index - 1);
            var removedEntryInfo = new RemovedEntryInfo(entry, previousEntryId);
            _removedEntryInfos.Add(entryId, removedEntryInfo);

            // Dispose a active value.
            var value = _activeValues[entryId];
            value.Dispose();

            _activeValues.Remove(entryId);
            _entryOrders.Remove(entryId);
            _entries.Remove(entryId);
        }

        /// <summary>
        ///     Restore a removed entry.
        /// </summary>
        /// <param name="removedEntryId"></param>
        /// <returns></returns>
        public Entry<T> RestoreEntry(string removedEntryId)
        {
            var removedEntryInfo = _removedEntryInfos[removedEntryId];
            var removedEntry = removedEntryInfo.Entry;

            int entryIndex;
            if (string.IsNullOrEmpty(removedEntryInfo.PreviousEntryId))
            {
                // If the previous entry not exists, it is the first entry.
                entryIndex = 0;
            }
            else
            {
                if (!_entryOrders.TryGetIndex(removedEntryInfo.PreviousEntryId, out var previousEntryIndex))
                    previousEntryIndex = -1;

                // If the previous entry was already removed, insert this entry to the last.
                entryIndex = previousEntryIndex == -1 ? _entries.Count : previousEntryIndex + 1;
            }

            // Create new entry and copy the values.
            var entry = new Entry<T>(removedEntry.Id);
            entry.Name.Value = removedEntry.Name.Value;
            foreach (var theme in _themes.Values)
            {
                var themeId = theme.Id;
                var value = entry.AddValue(themeId, GetDefaultValue());
                if (removedEntry.Values.TryGetValue(themeId, out var v)) value.Value = v.Value;

                if (_activeTheme.Value != null && themeId == _activeTheme.Value.Id)
                {
                    var colorProperty = new ObservableProperty<T>();
                    _activeValues[removedEntry.Id] = colorProperty;
                    value.Subscribe(x => { colorProperty.SetValueAndNotify(x); })
                        .DisposeWith(_activeThemeDisposables);
                }
            }

            _removedEntryInfos.Remove(entry.Id);
            _entryOrders.Add(entry.Id);
            _entryOrders.SetIndex(entry.Id, entryIndex);
            _entryOrderChangedSubject.OnNext((entry.Id, entryIndex));
            _entries.Add(entry.Id, entry);
            return entry;
        }

        public void ClearRemovedThemes()
        {
            _removedThemeInfos.Clear();
        }

        public void ClearRemovedEntries()
        {
            _removedEntryInfos.Clear();
        }

        protected abstract T GetDefaultValue();

        internal IEnumerable<(string id, string name)> GetThemeIdAndNames(
            char folderDelimiter,
            bool containsFolderName,
            bool nicifyNames = true
        )
        {
            var processedNames = new List<string>();

            foreach (var theme in _themes.Values.OrderBy(x => GetThemeOrder(x.Id)))
            {
                var name = theme.Name.Value;

                if (nicifyNames)
                {
                    name = GetDisplayName(name, folderDelimiter, containsFolderName);

                    while (processedNames.Contains(name))
                        name = GetIncrementedName(name);
                }

                processedNames.Add(name);
                yield return (theme.Id, name);
            }
        }

        internal IEnumerable<(string id, string name)> GetEntryIdAndNames(
            char folderDelimiter,
            bool containsFolderName,
            bool nicifyNames = true
        )
        {
            var processedNames = new List<string>();

            foreach (var entry in _entries.Values.OrderBy(x => GetEntryOrder(x.Id)))
            {
                var name = entry.Name.Value;

                if (nicifyNames)
                {
                    name = GetDisplayName(name, folderDelimiter, containsFolderName);

                    while (processedNames.Contains(name))
                        name = GetIncrementedName(name);
                }

                processedNames.Add(name);
                yield return (entry.Id, name);
            }
        }

        private static string GetDisplayName(string name, char folderDelimiter, bool containsFolderName = false)
        {
            if (!containsFolderName)
            {
                var hasFolderName = name.Contains(folderDelimiter);
                if (hasFolderName)
                {
                    var index = name.LastIndexOf(folderDelimiter);
                    if (index != -1)
                        name = name.Substring(index + 1);
                }
            }

            return name.Replace(" ", "").Replace("/", "_").Replace("-", "_");
        }

        private static string GetIncrementedName(string name)
        {
            // Calculate Number
            var num = 1;
            var match = _nonDuplicatedNameSuffixRegex.Match(name);
            var numStr = match.Groups[1].Value;
            if (!string.IsNullOrEmpty(numStr)) num = int.Parse(numStr);

            num++;

            // Remove Suffix
            name = _nonDuplicatedNameSuffixRegex.Replace(name, "");

            // Add Suffix
            name += $"_{num}";

            return name;
        }

        private sealed class RemovedEntryInfo
        {
            public RemovedEntryInfo(Entry<T> entry, string previousEntryId)
            {
                Entry = entry;
                PreviousEntryId = previousEntryId;
            }

            public Entry<T> Entry { get; }
            public string PreviousEntryId { get; }
        }

        private sealed class RemovedThemeInfo
        {
            public readonly IReadOnlyDictionary<string, T> EntryValues;

            public RemovedThemeInfo(
                Theme theme,
                string previousThemeId,
                bool wasActiveTheme,
                Dictionary<string, T> entryValues
            )
            {
                Theme = theme;
                PreviousThemeId = previousThemeId;
                WasActiveTheme = wasActiveTheme;
                EntryValues = entryValues;
            }

            public Theme Theme { get; }
            public string PreviousThemeId { get; }
            public bool WasActiveTheme { get; }
        }
    }
}