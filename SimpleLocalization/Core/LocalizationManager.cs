using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BetterSerialization.Runtime.Types;


namespace SimpleLocalization.Core
{
    public class LocalizationManager : BetterBehaviour
    {
        private static LocalizationManager _instance;

        public static LocalizationManager Instance
        {
            get { return _instance ?? (_instance = FindObjectOfType<LocalizationManager>()); }
        }

        public bool DestroyOnLoad = true;
        public event Action LanguageChanged;

        public Dictionary<string, LocalizationPackage> Packages = new Dictionary<string, LocalizationPackage>();

        public static bool Exists
        {
            get { return Instance != null; }
        }

        public LocalizationPackage this[string key]
        {
            get { return Packages[key]; }
        }

        public SystemLanguage DefaultLanguage
        {
            get { return AllLangauges[DefaultLanguageIndex]; }
        }

        public SystemLanguage CurrentLanguage
        {
            get { return AllLangauges[CurrentLanguageIndex]; }
        }

        public SystemLanguage[] AllLangauges
        {
            get; private set;
        }

        public string[] LanguagesNames
        {
            get; private set;
        }

        public int LanguagesCount
        {
            get { return AllLangauges.Length; }
        }

        public static int CurrentLanguageIndex
        {
            get { return Instance._currentLanguageIndex; }
            set { Instance._currentLanguageIndex = value; }
        }

        public int _currentLanguageIndex;
        public int DefaultLanguageIndex;
#if UNITY_EDITOR
        [UnityEditor.MenuItem("GameObject/Create Other/Localization Manager")]
#endif
        public static LocalizationManager Create()
        {
            return new GameObject("Localization Manager").AddComponent<LocalizationManager>();
        }

        private void Awake()
        {
            if (Packages.Count > 0)
                RefreshLanguages();

            OnLanguageChanged();
        }

        public void LoadPackage(string name)
        {
            var pkg = Resources.Load<LocalizationPackage>("Localization/" + name);
            Packages.Add(pkg.Name, pkg);
            RefreshLanguages();
        }

        public void LoadAllPackages()
        {
            Packages = Resources.LoadAll<LocalizationPackage>("Localization")
                .ToDictionary(key => key.Name, key => key);

            RefreshLanguages();
        }

        public void AddPackage(LocalizationPackage package)
        {
            Packages.Add(package.Name, package);
            RefreshLanguages();
        }

        public void RemovePackage(string name)
        {
            Packages.Remove(name);
            RefreshLanguages();
        }

        public LocalizationPackage GetPackage(string name)
        {
            return Packages[name];
        }

        public bool ContainsPackage(string packagaName)
        {
            if (string.IsNullOrEmpty(packagaName))
                return false;
            if (!Packages.ContainsKey(packagaName))
                return false;
            return Packages[packagaName] != null;
        }

        public string[] GetKeys<T>(string package) where T : class
        {
            return Packages[package].GetData<T>().Keys;
        }

        public string[] GetPackagesNames()
        {
            return Packages.Keys.ToArray();
        }

        public void RefreshLanguages()
        {
            AllLangauges = Packages
                .SelectMany(p => p.Value.GetLanguages())
                .Distinct()
                .ToArray();

            if(AllLangauges != null)
                LanguagesNames = AllLangauges.Select(l => l.ToString()).ToArray();
        }

        public static SystemLanguage GetLanguage(int index)
        {
            if (Instance.AllLangauges != null && Instance.AllLangauges.Length >= index - 1)
                return Instance.AllLangauges[index];
            else
                return default(SystemLanguage);
        }

        private ILocalizationData GetLocalizationData(string pkg, Type type)
        {
            return Packages[pkg].GetData(type);
        }

        public T GetTranslation<T>(string pkg, string key) where T : class
        {
            if (!ContainsPackage(pkg))
                return null;

            var data = Packages[pkg].GetData<T>();
            if (data.Languages.Contains(Instance.CurrentLanguage))
                return data.GetTranslation(CurrentLanguage, key) as T;

            return data.GetTranslation(DefaultLanguage, key) as T;
        }

        public static void ChangeLanguage(int index)
        {
            if (Instance.AllLangauges.Length >= index - 1)
            {
                CurrentLanguageIndex = index;
                Instance.OnLanguageChanged();
            }
        }

        public static void NextLanguage()
        {
            ChangeLanguage(Instance.LanguagesCount == CurrentLanguageIndex + 1 ? 0 : CurrentLanguageIndex + 1);
        }

        protected virtual void OnLanguageChanged()
        {
            var handler = LanguageChanged;
            if (handler != null) handler();
        }

        #region GetLocalizedData

        public static T GetLocalization<T>(string package, string key) where T : class 
        {
            return GetLocalization<T>(package, key, Instance.CurrentLanguage);
        }

        public static T GetLocalization<T>(string package, string key, SystemLanguage language) where T : class 
        {
            return (T)Instance.Packages[package].GetData<T>().GetTranslation(language, key);
        }

        #endregion
    } 
}
