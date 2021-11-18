using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace uPalette.Runtime.Core
{
    public static class UPaletteUtility
    {
        private static readonly Dictionary<string, Color> ColorCache = new Dictionary<string, Color>();

        public static Color GetColor(string name, bool useCache = true)
        {
            if (useCache && ColorCache.TryGetValue(name, out var color))
            {
                return color;
            }

            var app = UPaletteApplication.RequestInstance();
            var entry = app.UPaletteStore.Entries.FirstOrDefault(x => x.Name.Value.Equals(name));
            if (entry == null)
            {
                UPaletteApplication.ReleaseInstance();
                throw new Exception($"uPalette color ${name} is not found.");
            }

            UPaletteApplication.ReleaseInstance();
            color = entry.Value.Value;
            ColorCache[name] = color;
            return color;
        }

        public static void ClearColorCache()
        {
            ColorCache.Clear();
        }
    }
}