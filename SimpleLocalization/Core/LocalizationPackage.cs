using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BetterSerialization.Runtime.Types;
#if UNITY_EDITOR
using UnityEditor; 
#endif

namespace SimpleLocalization.Core
{
    public class LocalizationPackage : BetterScriptableObject, ILocalizationPackage
    {
        [Serialize]
        private readonly Dictionary<Type, ILocalizationData> _localizationDatas = new Dictionary<Type, ILocalizationData>();

        public string Name { get; set; }

        public bool AddLocalizationData(Type type)
        {
            if (_localizationDatas.ContainsKey(type) || !type.IsSupported())
                return false;

            var t = typeof(LocalizationData<>).MakeGenericType(type);
            var data = Activator.CreateInstance(t) as ILocalizationData;

            foreach (var language in GetLanguages())
                data.AddLanguage(language);

            _localizationDatas.Add(type, data);

            return true;
        }

        public bool RemoveLocalizationData(Type type)
        {
            if (!_localizationDatas.ContainsKey(type) || !type.IsSupported())
                return false;

            _localizationDatas.Remove(type);

            return true;
        }

        public ILocalizationData GetData(Type type)
        {
            return _localizationDatas[type];
        }

        public ILocalizationData GetData<T>() where T : class
        {
            return GetData(typeof(T));
        }

        public void SetData(Type type, ILocalizationData value)
        {
            _localizationDatas[type] = value;
        }

        public bool ContainsData(Type type)
        {
            return _localizationDatas.ContainsKey(type);
        }

        public void AddLanguage(SystemLanguage language)
        {
            foreach (var data in _localizationDatas)
                data.Value.AddLanguage(language);
        }

        public void RemoveLanguage(SystemLanguage language)
        {
            foreach (var data in _localizationDatas)
                data.Value.RemoveLanguage(language);
        }

        public SystemLanguage[] GetLanguages()
        {
            return _localizationDatas
                .SelectMany(d => d.Value.Languages)
                .Distinct()
                .ToArray();
        }
    }
    
}