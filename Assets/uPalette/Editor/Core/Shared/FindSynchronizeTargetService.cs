using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using uPalette.Runtime.Core.Synchronizer;

namespace uPalette.Editor.Core.Shared
{
    internal sealed class FindSynchronizeTargetService
    {
        public IReadOnlyList<Result> Run<TValue>(GameObject gameObject)
        {
            var results = new List<Result>();

            foreach (var synchronizerType in TypeCache.GetTypesWithAttribute<ValueSynchronizerAttribute>())
            {
                var synchronizerAttribute = synchronizerType.GetCustomAttribute<ValueSynchronizerAttribute>();
                if (synchronizerAttribute.ValueType != typeof(TValue)) continue;

                var targetType = synchronizerAttribute.AttachTargetType;
                if (!gameObject.TryGetComponent(targetType, out var target)) continue;

                var componentType = target.GetType();
                results.Add(new Result
                {
                    ComponentType = componentType,
                    SynchronizerType = synchronizerType,
                    PropertyDisplayName = synchronizerAttribute.TargetPropertyDisplayName
                });
            }

            return results;
        }

        public class Result
        {
            public Type ComponentType { get; set; }
            public Type SynchronizerType { get; set; }
            public string PropertyDisplayName { get; set; }
        }
    }
}
