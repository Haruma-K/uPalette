using System;
using UnityEngine;
using uPalette.Runtime.Foundation.Observable.ObservableProperty;

namespace uPalette.Runtime.Core
{
    [Serializable]
    public class ColorEntry
    {
        [SerializeField] private string _id;
        [SerializeField] private ObservableProperty<string> _name = new ObservableProperty<string>();
        [SerializeField] private ObservableProperty<Color> _color = new ColorObservableProperty();

        public ColorEntry(string name = null, Color? color = null)
        {
            _id = Guid.NewGuid().ToString();
            if (!string.IsNullOrEmpty(name))
            {
                _name.Value = name;
            }

            SetColor(color ?? UnityEngine.Color.white);
        }

        public string ID => _id;

        public ObservableProperty<string> Name => _name;

        public IReadOnlyObservableProperty<Color> Color => _color;

        public void SetColor(Color color)
        {
            _color.Value = color;
            if (string.IsNullOrEmpty(Name.Value) || ColorUtility.TryParseHtmlString(Name.Value, out _))
            {
                Name.Value = $"#{ColorUtility.ToHtmlStringRGBA(_color.Value)}";
            }
        }
    }
}