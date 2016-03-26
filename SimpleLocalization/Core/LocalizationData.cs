using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using BetterSerialization.Runtime.Types;

namespace SimpleLocalization.Core
{
    public class LocalizationData<T> : ILocalizationData<T> where T : class
    {
        [Serialize]
        private Dictionary<SystemLanguage, Dictionary<string, T>> _data = new Dictionary<SystemLanguage, Dictionary<string, T>>();

        public string Name { get; set; }

        [Serialize]
        private List<string> _keys = new List<string>();

        public string[] Keys
        {
            get { return _keys.ToArray(); }
        }

        public SystemLanguage[] Languages
        {
            get { return _data.Keys.ToArray(); }
        }

        public Type DataType
        {
            get { return typeof(T); }
        }

        public T this[SystemLanguage language, string key]
        {
            get { return (T)GetTranslation(language, key); }
            set { SetTranslation(language, key, value); }
        }

        public bool AddLanguage(SystemLanguage language)
        {
            if (_data.ContainsKey(language))
                return false;

            _data.Add(language, Keys.ToDictionary(key => key, key => default(T)));
            return true;
        }

        public bool RemoveLanguage(SystemLanguage language)
        {
            return _data.Remove(language);
        }

        public bool AddKey(string key)
        {
            if (Keys.Contains(key))
                return false;

            _keys.Add(key);

            if (Languages.Length > 0)
                foreach (var kvp in _data)
                    kvp.Value.Add(key, default(T));

            return true;
        }

        public bool RemoveKey(string key)
        {
            if (!Keys.Contains(key))
                return false;

            _keys.Remove(key);

            if (Languages.Length > 0)
                foreach (var kvp in _data)
                    kvp.Value.Remove(key); ;

            return true;
        }

        public object GetTranslation(SystemLanguage language, string key)
        {
            return _data[language][key];
        }

        public bool SetTranslation(SystemLanguage language, string key, object value)
        {
            if (!Languages.Contains(language))
                return false;
            if (!Keys.Contains(key))
                return false;
            if (value == null)
                return false;

            _data[language][key] = (T)value;
            return true;
        }

        public void CopyFrom(ILocalizationData data)
        {
            _data.Clear();

            var keys = data.Keys;
            var langs = data.Languages;

            foreach (var lang in langs)
            {
                AddLanguage(lang);
                foreach (var key in keys)
                {
                    AddKey(key);
                    SetTranslation(lang, key, data.GetTranslation(lang, key));
                }
            }
        }
    } 
}
