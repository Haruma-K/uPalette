using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using uPalette.Runtime.Core;

namespace uPalette.Editor.Core
{
    public class FindAppliedGameObjectService
    {
        public GameObject[] Execute(params string[] entryIds)
        {
            var result = new List<GameObject>();
            var setters = Object.FindObjectsOfType<ColorSetter>();
            foreach (var setter in setters)
            {
                if (result.Contains(setter.gameObject))
                {
                    continue;
                }

                var entryId = setter._entryId.Value;
                if (entryIds.Contains(entryId))
                {
                    result.Add(setter.gameObject);
                }
            }

            return result.ToArray();
        }
    }
}