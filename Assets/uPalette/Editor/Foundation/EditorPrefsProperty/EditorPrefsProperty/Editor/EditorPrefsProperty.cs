using System;
using UnityEditor;

namespace uPalette.Editor.Foundation.EditorPrefsProperty.EditorPrefsProperty.Editor
{
    /// <summary>
    ///     Property whose value is persisted in EditorPrefs.
    /// </summary>
    public abstract class EditorPrefsProperty<T>
    {
        private readonly T _defaultValue;
        private readonly Action<string> _delete;
        private readonly Func<string, T, T> _get;
        private readonly string _key;
        private readonly Action<string, T> _set;

        private bool _isLoaded;
        private T _value;

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue">Returns this value if <see cref="key" /> does not exit in the EditorPrefs.</param>
        /// <param name="get"></param>
        /// <param name="set"></param>
        protected EditorPrefsProperty(string key, T defaultValue, Func<string, T, T> get, Action<string, T> set,
            Action<string> delete)
        {
            _key = key;
            _defaultValue = defaultValue;
            _get = get;
            _set = set;
            _delete = delete;
        }

        public T Value
        {
            get
            {
                if (!_isLoaded)
                {
                    _value = _get(_key, _defaultValue);
                    _isLoaded = true;
                }

                return _value;
            }
            set
            {
                if (_isLoaded && _value.Equals(value))
                {
                    return;
                }

                _value = value;
                _set(_key, value);
            }
        }

        /// <summary>
        ///     Reset the value and remove the key from the EditorPrefs.
        /// </summary>
        public void Reset()
        {
            _delete.Invoke(_key);
            _isLoaded = false;
        }
    }

    /// <inheritdoc />
    public sealed class IntEditorPrefsProperty : EditorPrefsProperty<int>
    {
        /// <inheritdoc />
        public IntEditorPrefsProperty(string key, int defaultValue) : base(key, defaultValue, EditorPrefs.GetInt,
            EditorPrefs.SetInt, EditorPrefs.DeleteKey)
        {
        }
    }

    /// <inheritdoc />
    public sealed class FloatEditorPrefsProperty : EditorPrefsProperty<float>
    {
        /// <inheritdoc />
        public FloatEditorPrefsProperty(string key, float defaultValue) : base(key, defaultValue, EditorPrefs.GetFloat,
            EditorPrefs.SetFloat, EditorPrefs.DeleteKey)
        {
        }
    }

    /// <inheritdoc />
    public sealed class BoolEditorPrefsProperty : EditorPrefsProperty<bool>
    {
        /// <inheritdoc />
        public BoolEditorPrefsProperty(string key, bool defaultValue) : base(key, defaultValue, EditorPrefs.GetBool,
            EditorPrefs.SetBool, EditorPrefs.DeleteKey)
        {
        }
    }

    /// <inheritdoc />
    public sealed class StringEditorPrefsProperty : EditorPrefsProperty<string>
    {
        /// <inheritdoc />
        public StringEditorPrefsProperty(string key, string defaultValue) : base(key, defaultValue,
            EditorPrefs.GetString, EditorPrefs.SetString, EditorPrefs.DeleteKey)
        {
        }
    }
}
