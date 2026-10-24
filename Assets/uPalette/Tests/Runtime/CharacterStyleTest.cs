using NUnit.Framework;
using UnityEngine;
using uPalette.Runtime.Core.Model;
using uPalette.Runtime.Foundation.CharacterStyles;

namespace uPalette.Tests.Runtime
{
    internal sealed class CharacterStyleTest
    {
        [Test]
        public void Default_HasUsableFontAndDefaultSettings()
        {
            // Read the default character style in the running Unity version.
            var style = CharacterStyle.Default;

            // The default font can display text, and the other defaults remain unchanged.
            Assert.That(style.font, Is.Not.Null);
            Assert.That(style.font.HasCharacter('A'), Is.True);
            Assert.That(style.fontStyle, Is.EqualTo(FontStyle.Normal));
            Assert.That(style.fontSize, Is.EqualTo(14));
            Assert.That(style.lineSpacing, Is.EqualTo(1));
        }

        [Test]
        public void AddEntry_InitializesCharacterStyleForEveryTheme()
        {
            // Prepare a character style palette with multiple themes before adding an entry.
            var palette = new CharacterStylePalette();
            palette.AddTheme();

            // Add an entry that needs a default character style for each theme.
            var entry = palette.AddEntry();

            // Every theme receives a character style with a usable font.
            Assert.That(entry.Values.Count, Is.EqualTo(palette.Themes.Count));
            foreach (var value in entry.Values.Values)
                Assert.That(value.Value.font, Is.Not.Null);
        }

        [Test]
        public void AddTheme_InitializesCharacterStyleWithoutChangingExistingValues()
        {
            // Prepare an entry whose active theme has a customized character style.
            var palette = new CharacterStylePalette();
            var entry = palette.AddEntry();
            var activeThemeId = palette.ActiveTheme.Value.Id;
            var customStyle = entry.Values[activeThemeId].Value;
            customStyle.fontStyle = FontStyle.Bold;
            customStyle.fontSize = 24;
            customStyle.lineSpacing = 2;
            entry.Values[activeThemeId].Value = customStyle;

            // Add a theme that needs a new default character style for the existing entry.
            var theme = palette.AddTheme();
            var newStyle = entry.Values[theme.Id].Value;

            // The new theme has a usable default style, and the customized theme is preserved.
            Assert.That(newStyle.font, Is.Not.Null);
            Assert.That(newStyle.fontStyle, Is.EqualTo(FontStyle.Normal));
            Assert.That(newStyle.fontSize, Is.EqualTo(14));
            Assert.That(newStyle.lineSpacing, Is.EqualTo(1));
            Assert.That(entry.Values[activeThemeId].Value, Is.EqualTo(customStyle));
        }
    }
}
