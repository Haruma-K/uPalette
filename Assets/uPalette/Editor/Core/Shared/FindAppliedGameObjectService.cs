using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using uPalette.Runtime.Core.Synchronizer.Color;

namespace uPalette.Editor.Core.Shared
{
    public class FindAppliedGameObjectService
    {
        public GameObject[] Execute(params string[] entryIds)
        {
            var result = new List<GameObject>();
            var synchronizers = Object.FindObjectsOfType<ColorSynchronizer>();
            foreach (var synchronizer in synchronizers)
            {
                if (result.Contains(synchronizer.gameObject))
                {
                    continue;
                }

                var entryId = synchronizer.EntryId;
                if (entryIds.Contains(entryId))
                {
                    result.Add(synchronizer.gameObject);
                }
            }

            return result.ToArray();
        }
    }
}
