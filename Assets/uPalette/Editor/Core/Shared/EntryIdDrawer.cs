using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using uPalette.Runtime.Core;
using uPalette.Runtime.Core.Model;
using uPalette.Runtime.Foundation.CharacterStyles;

namespace uPalette.Editor.Core.Shared
{
    [CustomPropertyDrawer(typeof(ColorEntryId))]
    internal sealed class ColorEntryIdDrawer : EntryIdDrawer<Color>
    {
        protected override Palette<Color> GetPalette(PaletteStore store)
        {
            return store.ColorPalette;
        }
    }

    [CustomPropertyDrawer(typeof(GradientEntryId))]
    internal sealed class GradientEntryIdDrawer : EntryIdDrawer<Gradient>
    {
        protected override Palette<Gradient> GetPalette(PaletteStore store)
        {
            return store.GradientPalette;
        }
    }

    [CustomPropertyDrawer(typeof(CharacterStyleEntryId))]
    internal sealed class CharacterStyleEntryIdDrawer : EntryIdDrawer<CharacterStyle>
    {
        protected override Palette<CharacterStyle> GetPalette(PaletteStore store)
        {
            return store.CharacterStylePalette;
        }
    }

    [CustomPropertyDrawer(typeof(CharacterStyleTMPEntryId))]
    internal sealed class CharacterStyleEntryIdTMPDrawer : EntryIdDrawer<CharacterStyleTMP>
    {
        protected override Palette<CharacterStyleTMP> GetPalette(PaletteStore store)
        {
            return store.CharacterStyleTMPPalette;
        }
    }

    internal abstract class EntryIdDrawer<T> : PropertyDrawer
    {
        private string[] _entryIds = Array.Empty<string>();
        private string[] _displayNames = Array.Empty<string>();
        private int _selectedIndex = -1;

        protected abstract Palette<T> GetPalette(PaletteStore store);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var valueProperty = property.FindPropertyRelative("_value");
            var entryId = valueProperty.stringValue;

            var store = PaletteStore.Instance;
            if (store == null)
            {
                Array.Resize(ref _entryIds, 0);
                Array.Resize(ref _displayNames, 0);
            }
            else
            {
                var palette = GetPalette(store);
                if (_entryIds.Length != palette.Entries.Count)
                {
                    Array.Resize(ref _entryIds, palette.Entries.Count);
                    Array.Resize(ref _displayNames, palette.Entries.Count);
                }

                var index = 0;
                foreach (var entry in palette.Entries.Values.OrderBy(x => palette.GetEntryOrder(x.Id)))
                {
                    _entryIds[index] = entry.Id;
                    _displayNames[index] = entry.Name.Value;
                    if (entryId == entry.Id)
                        _selectedIndex = index;
                    index++;
                }
            }

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                using (var ccs = new EditorGUI.ChangeCheckScope())
                {
                    var newValue = EditorGUI.Popup(position, label.text, _selectedIndex, _displayNames);
                    if (ccs.changed)
                    {
                        var newEntryId = _entryIds[newValue];
                        valueProperty.stringValue = newEntryId;
                    }
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
