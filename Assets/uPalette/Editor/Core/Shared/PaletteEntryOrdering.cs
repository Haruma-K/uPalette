using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using uPalette.Runtime.Core.Model;

namespace uPalette.Editor.Core.Shared
{
    internal static class PaletteEntryOrdering
    {
        public static IEnumerable<Entry<T>> GetOrderedEntries<T>(
            Palette<T> palette,
            bool folderMode,
            char folderDelimiter
        )
        {
            var entries = palette.Entries.Values.OrderBy(entry => palette.GetEntryOrder(entry.Id));
            if (!folderMode)
                return entries;

            return entries.OrderBy(entry => entry.Name.Value, new FolderPathComparer(folderDelimiter));
        }

        public static int CompareSiblingNames(
            string leftName,
            bool leftIsFolder,
            string rightName,
            bool rightIsFolder
        )
        {
            var nameComparison = CompareNames(leftName, rightName);
            if (nameComparison != 0 || leftIsFolder == rightIsFolder)
                return nameComparison;

            return leftIsFolder ? -1 : 1;
        }

        private static int CompareNames(string leftName, string rightName)
        {
            var naturalComparison = EditorUtility.NaturalCompare(leftName, rightName);
            if (naturalComparison != 0)
                return naturalComparison;

            return string.Compare(leftName, rightName, StringComparison.Ordinal);
        }

        private sealed class FolderPathComparer : IComparer<string>
        {
            private readonly char _folderDelimiter;

            public FolderPathComparer(char folderDelimiter)
            {
                _folderDelimiter = folderDelimiter;
            }

            public int Compare(string leftPath, string rightPath)
            {
                var leftNames = leftPath.Split(_folderDelimiter);
                var rightNames = rightPath.Split(_folderDelimiter);
                var sharedDepth = Math.Min(leftNames.Length, rightNames.Length);

                for (var i = 0; i < sharedDepth; i++)
                {
                    var nameComparison = CompareNames(leftNames[i], rightNames[i]);
                    if (nameComparison != 0)
                        return nameComparison;
                }

                if (leftNames.Length == rightNames.Length)
                    return 0;

                // A folder and an entry can have the same display name. Placing the folder first gives
                // the hierarchical TreeView and the flattened Name Enums a deterministic common order.
                return leftNames.Length > rightNames.Length ? -1 : 1;
            }
        }
    }
}
