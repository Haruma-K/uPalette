using System;
using UnityEngine;

namespace uPalette.Runtime.Core
{
    [Serializable]
    public abstract class EntryId
    {
        [SerializeField] private string _value;

        public string Value
        {
            get => _value;
            set => _value = value;
        }
    }

    [Serializable]
    public sealed class ColorEntryId : EntryId
    {
    }

    [Serializable]
    public sealed class GradientEntryId : EntryId
    {
    }

    [Serializable]
    public sealed class CharacterStyleEntryId : EntryId
    {
    }

    [Serializable]
    public sealed class CharacterStyleTMPEntryId : EntryId
    {
    }
}
