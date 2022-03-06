using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using uPalette.Runtime.Core.Model;
using uPalette.Runtime.Foundation.TinyRx;

namespace uPalette.Tests.Runtime
{
    internal sealed class PaletteTest
    {
        #region ActiveValue

        [Test]
        public void ActiveValue_CanObserve()
        {
            var palette = new ColorPalette();
            var entry = palette.AddEntry();
            var value = palette.GetActiveValue(entry.Id);
            var color = Color.black;
            var disposable = value.Subscribe(x => color = x);
            entry.Values.First().Value.Value = Color.cyan;
            Assert.That(color, Is.EqualTo(Color.cyan));
            disposable.Dispose();
        }

        #endregion

        #region Constructor

        [Test]
        public void Instantiate_DefaultThemeIsCreated()
        {
            var palette = new ColorPalette();

            Assert.That(palette.Themes.Count, Is.EqualTo(1));
        }

        [Test]
        public void Instantiate_FirstThemeIsSetAsActiveTheme()
        {
            var palette = new ColorPalette();

            var firstTheme = palette.Themes.First();
            Assert.That(firstTheme.Value, Is.EqualTo(palette.ActiveTheme.Value));
        }

        [Test]
        public void Instantiate_NoEntryIsCreated()
        {
            var palette = new ColorPalette();

            Assert.That(palette.Entries.Count, Is.Zero);
        }

        #endregion

        #region Theme

        [Test]
        public void SetThemeOrder_OrderIsSetProperly()
        {
            var palette = new ColorPalette();
            var theme1 = palette.Themes.First().Value;
            var theme2 = palette.AddTheme();
            var theme3 = palette.AddTheme();

            Assert.That(palette.GetThemeOrder(theme1.Id), Is.EqualTo(0));
            Assert.That(palette.GetThemeOrder(theme2.Id), Is.EqualTo(1));
            Assert.That(palette.GetThemeOrder(theme3.Id), Is.EqualTo(2));

            palette.SetThemeOrder(theme3.Id, 1);

            Assert.That(palette.GetThemeOrder(theme1.Id), Is.EqualTo(0));
            Assert.That(palette.GetThemeOrder(theme2.Id), Is.EqualTo(2));
            Assert.That(palette.GetThemeOrder(theme3.Id), Is.EqualTo(1));
        }

        [Test]
        public void AddTheme_OrderIsSet()
        {
            var palette = new ColorPalette();
            var theme1 = palette.Themes.First().Value;
            var theme2 = palette.AddTheme();

            Assert.That(palette.GetThemeOrder(theme1.Id), Is.EqualTo(0));
            Assert.That(palette.GetThemeOrder(theme2.Id), Is.EqualTo(1));
        }

        [Test]
        public void AddTheme_ThemeIsAdded()
        {
            var palette = new ColorPalette();

            Assert.That(palette.Themes.Count, Is.EqualTo(1));
            palette.AddTheme();
            Assert.That(palette.Themes.Count, Is.EqualTo(2));
        }

        [Test]
        public void AddTheme_EntryValueIsAdded()
        {
            var palette = new ColorPalette();
            var entry = palette.AddEntry();
            var theme = palette.AddTheme();

            Assert.That(entry.Values.Count, Is.EqualTo(2));
            Assert.That(entry.Values.ContainsKey(theme.Id), Is.True);
        }

        [Test]
        public void RemoveTheme_ThemeRemoved()
        {
            var palette = new ColorPalette();
            var theme = palette.AddTheme();
            Assert.That(palette.Themes.Count, Is.EqualTo(2));
            palette.RemoveTheme(theme.Id);

            Assert.That(palette.Themes.Count, Is.EqualTo(1));
        }

        [Test]
        public void RemoveTheme_RemoveDefaultTheme_ThrowException()
        {
            var palette = new ColorPalette();
            Assert.Throws<InvalidOperationException>(() => palette.RemoveTheme(palette.ActiveTheme.Value.Id));
        }

        [Test]
        public void RemoveTheme_Reordered()
        {
            var palette = new ColorPalette();
            var theme1 = palette.Themes.First().Value;
            var theme2 = palette.AddTheme();
            var theme3 = palette.AddTheme();

            Assert.That(palette.GetThemeOrder(theme1.Id), Is.EqualTo(0));
            Assert.That(palette.GetThemeOrder(theme2.Id), Is.EqualTo(1));
            Assert.That(palette.GetThemeOrder(theme3.Id), Is.EqualTo(2));

            palette.RemoveTheme(theme2.Id);

            Assert.That(palette.GetThemeOrder(theme1.Id), Is.EqualTo(0));
            Assert.That(palette.GetThemeOrder(theme3.Id), Is.EqualTo(1));
        }

        [Test]
        public void RemoveTheme_RemoveEntryValues()
        {
            var palette = new ColorPalette();
            var entry = palette.AddEntry();
            var theme = palette.AddTheme();

            Assert.That(entry.TryGetValue(theme.Id, out _), Is.True);

            palette.RemoveTheme(theme.Id);

            Assert.That(entry.TryGetValue(theme.Id, out _), Is.False);
        }

        [Test]
        public void RemoveTheme_RemoveActiveTheme_ChangeToDefaultTheme()
        {
            var palette = new ColorPalette();
            var defaultTheme = palette.Themes.First().Value;
            var theme = palette.AddTheme();
            palette.SetActiveTheme(theme.Id);

            Assert.That(palette.ActiveTheme.Value, Is.EqualTo(theme));

            palette.RemoveTheme(theme.Id);

            Assert.That(palette.ActiveTheme.Value, Is.EqualTo(defaultTheme));
        }

        [Test]
        public void RestoreTheme_ThemeIsRestored()
        {
            var palette = new ColorPalette();
            var theme = palette.AddTheme();

            Assert.That(palette.HasTheme(theme.Id), Is.True);

            palette.RemoveTheme(theme.Id);

            Assert.That(palette.HasTheme(theme.Id), Is.False);

            palette.RestoreTheme(theme.Id);

            Assert.That(palette.HasTheme(theme.Id), Is.True);
        }

        [Test]
        public void RestoreTheme_OrderIsRestored()
        {
            var palette = new ColorPalette();
            var theme1 = palette.Themes.First().Value;
            var theme2 = palette.AddTheme();
            var theme3 = palette.AddTheme();

            Assert.That(palette.GetThemeOrder(theme1.Id), Is.EqualTo(0));
            Assert.That(palette.GetThemeOrder(theme2.Id), Is.EqualTo(1));
            Assert.That(palette.GetThemeOrder(theme3.Id), Is.EqualTo(2));

            palette.RemoveTheme(theme2.Id);

            Assert.That(palette.GetThemeOrder(theme1.Id), Is.EqualTo(0));
            Assert.That(palette.GetThemeOrder(theme3.Id), Is.EqualTo(1));

            palette.RestoreTheme(theme2.Id);

            Assert.That(palette.GetThemeOrder(theme1.Id), Is.EqualTo(0));
            Assert.That(palette.GetThemeOrder(theme2.Id), Is.EqualTo(1));
            Assert.That(palette.GetThemeOrder(theme3.Id), Is.EqualTo(2));
        }

        [Test]
        public void RestoreTheme_EntryValueIsRestored()
        {
            var palette = new ColorPalette();
            var entry = palette.AddEntry();
            var theme = palette.AddTheme();

            Assert.That(entry.TryGetValue(theme.Id, out _), Is.True);

            palette.RemoveTheme(theme.Id);

            Assert.That(entry.TryGetValue(theme.Id, out _), Is.False);

            palette.RestoreTheme(theme.Id);

            Assert.That(entry.TryGetValue(theme.Id, out _), Is.True);
        }

        [Test]
        public void RestoreTheme_RemovedThemeWasActiveTheme_SetAsActiveTheme()
        {
            var palette = new ColorPalette();
            var defaultTheme = palette.Themes.First().Value;
            var theme = palette.AddTheme();
            palette.SetActiveTheme(theme.Id);

            Assert.That(palette.ActiveTheme.Value, Is.EqualTo(theme));

            palette.RemoveTheme(theme.Id);

            Assert.That(palette.ActiveTheme.Value, Is.EqualTo(defaultTheme));

            palette.RestoreTheme(theme.Id);

            Assert.That(palette.ActiveTheme.Value.Id, Is.EqualTo(theme.Id));
        }

        [Test]
        public void RestoreTheme_AddEntryAfterRemove_NewEntryHasValueForRestoredTheme()
        {
            var palette = new ColorPalette();
            var theme = palette.AddTheme();

            palette.RemoveTheme(theme.Id);
            var entry = palette.AddEntry();

            Assert.That(entry.Values.ContainsKey(theme.Id), Is.False);

            palette.RestoreTheme(theme.Id);

            Assert.That(entry.Values.ContainsKey(theme.Id), Is.True);
        }

        [Test]
        public void RestoreTheme_RemoveEntryAfterRemove_NoEntryIsCreated()
        {
            var palette = new ColorPalette();
            var theme = palette.AddTheme();
            var entry = palette.AddEntry();

            palette.RemoveTheme(theme.Id);

            Assert.That(palette.HasEntry(entry.Id), Is.True);

            palette.RemoveEntry(entry.Id);

            Assert.That(palette.HasEntry(entry.Id), Is.False);

            palette.RestoreTheme(theme.Id);

            Assert.That(palette.HasEntry(entry.Id), Is.False);
        }

        #endregion

        #region Entry

        [Test]
        public void AddEntry_OrderIsSet()
        {
            var palette = new ColorPalette();
            var entry1 = palette.AddEntry();
            var entry2 = palette.AddEntry();

            Assert.That(palette.GetEntryOrder(entry1.Id), Is.Zero);
            Assert.That(palette.GetEntryOrder(entry2.Id), Is.EqualTo(1));
        }

        [Test]
        public void SetEntryOrder_OrderIsSetProperly()
        {
            var palette = new ColorPalette();
            var entry1 = palette.AddEntry();
            var entry2 = palette.AddEntry();
            palette.SetEntryOrder(entry2.Id, 0);

            Assert.That(palette.GetEntryOrder(entry1.Id), Is.EqualTo(1));
            Assert.That(palette.GetEntryOrder(entry2.Id), Is.Zero);
        }

        [Test]
        public void AddEntry_EntryIsAdded()
        {
            var palette = new ColorPalette();

            Assert.That(palette.Entries.Count, Is.Zero);
            palette.AddEntry();
            Assert.That(palette.Entries.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddEntry_AddedEntryHasValueForActiveTheme()
        {
            var palette = new ColorPalette();
            var activeTheme = palette.ActiveTheme.Value;
            var entry = palette.AddEntry();

            Assert.That(entry.Values.Count, Is.EqualTo(1));
            Assert.That(entry.Values.ContainsKey(activeTheme.Id), Is.True);
        }

        [Test]
        public void AddEntry_ActiveValueIsAdded()
        {
            var palette = new ColorPalette();
            var entry = palette.AddEntry();

            Assert.That(palette.TryGetActiveValue(entry.Id, out _), Is.True);
        }

        [Test]
        public void RemoveEntry_EntryRemoved()
        {
            var palette = new ColorPalette();
            var entry = palette.AddEntry();
            palette.RemoveEntry(entry.Id);

            Assert.That(palette.Entries.Count, Is.Zero);
        }

        [Test]
        public void RemoveEntry_Reordered()
        {
            var palette = new ColorPalette();
            var entry1 = palette.AddEntry();
            var entry2 = palette.AddEntry();
            var entry3 = palette.AddEntry();

            Assert.That(palette.GetEntryOrder(entry1.Id), Is.EqualTo(0));
            Assert.That(palette.GetEntryOrder(entry2.Id), Is.EqualTo(1));
            Assert.That(palette.GetEntryOrder(entry3.Id), Is.EqualTo(2));

            palette.RemoveEntry(entry2.Id);

            Assert.That(palette.GetEntryOrder(entry1.Id), Is.EqualTo(0));
            Assert.That(palette.GetEntryOrder(entry3.Id), Is.EqualTo(1));
        }

        [Test]
        public void RestoreEntry_EntryIsRestored()
        {
            var palette = new ColorPalette();
            var entry = palette.AddEntry();
            palette.RemoveEntry(entry.Id);
            var value = entry.Values.First();

            Assert.That(palette.Entries.Count, Is.Zero);

            var restoredEntry = palette.RestoreEntry(entry.Id);
            var restoredValue = restoredEntry.Values.First();

            Assert.That(palette.Entries.Count, Is.EqualTo(1));
            Assert.That(value.Key, Is.EqualTo(restoredValue.Key));
            Assert.That(value.Value.Value, Is.EqualTo(restoredValue.Value.Value));
        }

        [Test]
        public void RestoreEntry_OrderIsRestored()
        {
            var palette = new ColorPalette();
            var entry1 = palette.AddEntry();
            var entry2 = palette.AddEntry();
            var entry3 = palette.AddEntry();

            Assert.That(palette.GetEntryOrder(entry1.Id), Is.EqualTo(0));
            Assert.That(palette.GetEntryOrder(entry2.Id), Is.EqualTo(1));
            Assert.That(palette.GetEntryOrder(entry3.Id), Is.EqualTo(2));

            palette.RemoveEntry(entry2.Id);

            Assert.That(palette.GetEntryOrder(entry1.Id), Is.EqualTo(0));
            Assert.That(palette.GetEntryOrder(entry3.Id), Is.EqualTo(1));

            palette.RestoreEntry(entry2.Id);

            Assert.That(palette.GetEntryOrder(entry1.Id), Is.EqualTo(0));
            Assert.That(palette.GetEntryOrder(entry2.Id), Is.EqualTo(1));
            Assert.That(palette.GetEntryOrder(entry3.Id), Is.EqualTo(2));
        }

        [Test]
        public void RestoreEntry_AddThemeAfterRemove_NewEntryHasValueForRestoredTheme()
        {
            var palette = new ColorPalette();
            var entry = palette.AddEntry();

            Assert.That(palette.Entries.Count, Is.EqualTo(1));
            Assert.That(entry.Values.Count, Is.EqualTo(1));

            palette.RemoveEntry(entry.Id);

            Assert.That(palette.Entries.Count, Is.EqualTo(0));

            var theme = palette.AddTheme();

            var restoredEntry = palette.RestoreEntry(entry.Id);

            Assert.That(palette.Entries.Count, Is.EqualTo(1));
            Assert.That(restoredEntry.Values.Count, Is.EqualTo(2));
            Assert.That(restoredEntry.Values.ContainsKey(theme.Id), Is.True);
        }

        [Test]
        public void RestoreEntry_RemoveThemeAfterRemove_NoValueIsCreated()
        {
            var palette = new ColorPalette();
            var entry = palette.AddEntry();
            var theme = palette.AddTheme();

            Assert.That(palette.Entries.Count, Is.EqualTo(1));
            Assert.That(entry.Values.Count, Is.EqualTo(2));

            palette.RemoveEntry(entry.Id);

            Assert.That(palette.Entries.Count, Is.EqualTo(0));

            palette.RemoveTheme(theme.Id);

            var restoredEntry = palette.RestoreEntry(entry.Id);

            Assert.That(palette.Entries.Count, Is.EqualTo(1));
            Assert.That(restoredEntry.Values.Count, Is.EqualTo(1));
            Assert.That(restoredEntry.Values.ContainsKey(theme.Id), Is.False);
        }

        #endregion
    }
}
