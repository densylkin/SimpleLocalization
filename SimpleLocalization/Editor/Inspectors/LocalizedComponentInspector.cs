using UnityEngine;
using System.Collections;
using SimpleLocalization.Core;
using SimpleLocalization.Core.Components;
using UnityEditor;

namespace SimpleLocalization.Editor
{
    public class LocalizedComponentInspector<T> : UnityEditor.Editor where T : Component, ILocalizedComponent
    {
        private ILocalizedComponent component { get { return (ILocalizedComponent)target; } }

        protected string[] _packages;
        protected string[] _keys;

        public string CurrentPackage { get { return _packages[component.PackageIndex]; } }

        private int CurrentPkg
        {
            get { return component.PackageIndex; }
            set
            {
                if (component.PackageIndex != value && _packages != null)
                {
                    component.PackageIndex = value;
                    component.PackageName = _packages[component.PackageIndex];
                    if (component.DataExists)
                        GetKeys();
                    EditorUtility.SetDirty((Object)component);
                }
            }
        }

        private int CurrentKey
        {
            get { return component.KeyIndex; }
            set
            {
                if ((component.KeyIndex != value || _keys.Length == 1) && _keys != null)
                {
                    component.KeyIndex = value;
                    component.Key = _keys[component.KeyIndex];
                    EditorUtility.SetDirty((Object)component);
                }
            }
        }

        private void OnEnable()
        {
            _packages = LocalizationManager.Instance.GetPackagesNames();
            if (CurrentPkg != -1 && component.PackageExists)
                if (component.DataExists)
                    GetKeys();

            Repaint();

        }

        public override void OnInspectorGUI()
        {
            Undo.RecordObject((Object)component, "localizedtext");

            if (Application.isPlaying)
                PlayModeUI();
            else
            {
                PackageSelector();
                if (component.PackageExists)
                    if (component.DataExists)
                        KeySelector();
                    else
                        GUILayout.Label("Package does not contain data of this type.", EditorStyles.wordWrappedLabel);
            }

            EditorUtility.SetDirty((Object)component);
        }

        protected virtual void PackageSelector()
        {
            CurrentPkg = EditorGUILayout.Popup("Package", CurrentPkg, _packages);
        }

        protected virtual void KeySelector()
        {
            if (_keys == null)
                return;

            CurrentKey = EditorGUILayout.Popup("Key", CurrentKey, _keys);
        }

        protected virtual void PlayModeUI()
        {
            GUILayout.Label(component.PackageName, EditorStyles.boldLabel);
            GUILayout.Label(component.Key, EditorStyles.boldLabel);
        }

        protected virtual void GetKeys()
        {

        }
    }
    
}