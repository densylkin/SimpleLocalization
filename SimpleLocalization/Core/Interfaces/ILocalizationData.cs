using UnityEngine;
using System;

namespace SimpleLocalization.Core
{
    public interface ILocalizationData
    {
        string Name { get; set; }
        Type DataType { get; }

        string[] Keys { get; }
        SystemLanguage[] Languages { get; }

        bool AddLanguage(SystemLanguage langauge);
        bool RemoveLanguage(SystemLanguage language);

        bool AddKey(string key);
        bool RemoveKey(string key);

        object GetTranslation(SystemLanguage language, string key);
        bool SetTranslation(SystemLanguage language, string key, object value);

        void CopyFrom(ILocalizationData data);
    }

    public interface ILocalizationData<T> : ILocalizationData
    {
        T this[SystemLanguage language, string key] { get; set; }
    }
    
}