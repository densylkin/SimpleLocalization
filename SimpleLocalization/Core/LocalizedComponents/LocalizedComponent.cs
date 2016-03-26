using System;
using UnityEngine;
using System.Collections;
using BetterSerialization.Runtime.Types;

namespace SimpleLocalization.Core.Components
{
    public interface ILocalizedComponent
    {
        string PackageName { get; set; }
        string Key { get; set; }

        int PackageIndex { get; set; }
        int KeyIndex { get; set; }

        bool PackageExists { get; }
        bool DataExists { get; }

        Type GetDataType();
    }

    public class LocalizedComponent<T> : BetterBehaviour, ILocalizedComponent where T : Component
    {
        protected T _component;

        public string PackageName { get; set; }
        public string Key { get; set; }

        public int PackageIndex { get; set; }
        public int KeyIndex { get; set; }

        public bool PackageExists
        {
            get { return LocalizationManager.Instance.ContainsPackage(PackageName); }
        }

        public bool DataExists
        {
            get { return LocalizationManager.Instance.GetPackage(PackageName).ContainsData(GetDataType()); }
        }

        public LocalizedComponent()
        {
            PackageIndex = -1;
            KeyIndex = -1;
        }

        private void OnEnable()
        {
            _component = gameObject.GetComponent<T>();
            LocalizationManager.Instance.LanguageChanged += OnLanguageChanged;
        }

        protected virtual void OnLanguageChanged()
        {
            if (!PackageExists)
                return;
            if (!DataExists)
                return;
        }

        public virtual Type GetDataType()
        {
            return null;
        }
    }
}