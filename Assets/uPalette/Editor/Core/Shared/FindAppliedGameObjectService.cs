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
#if UNITY_2022_2_OR_NEWER
            var synchronizers = Object.FindObjectsByType<ColorSynchronizer>(FindObjectsSortMode.InstanceID);
#else
            var synchronizers = Object.FindObjectsOfType<ColorSynchronizer>();
#endif
            foreach (var synchronizer in synchronizers)
            {
                if (result.Contains(synchronizer.gameObject))
                {
                    continue;
                }

                var entryId = synchronizer.EntryId.Value;
                if (entryIds.Contains(entryId))
                {
                    result.Add(synchronizer.gameObject);
                }
            }

            return result.ToArray();
        }
    }
}
