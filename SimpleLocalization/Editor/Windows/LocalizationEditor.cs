using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using SimpleLocalization.Core;
using SimpleLocalization.Helpers;
using UnityEditor.Callbacks;
using UnityHelpers.GUI;

namespace SimpleLocalization.Editor
{
    public class LocalizationEditor : EditorWindow
    {
        [SerializeField]
        private LocalizationPackage _package;
        private Type _currentData;

        private ILocalizationData CurrentData
        {
            get
            {
                if (_package != null && _currentData != null && _package.ContainsData(_currentData))
                    return _package.GetData(_currentData);
                else
                    return null;
            }
        }

        private string _newKeyName = "";
        private SystemLanguage _newLang;

        private bool _editName;

        public static void Init(LocalizationPackage pkg)
        {
            var window = GetWindow<LocalizationEditor>();
            window.Show();
            window._package = pkg;
        }

        

        [OnOpenAsset]
        public static bool OnOpenPackage(int instanceId, int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceId);

            if (obj.GetType() != typeof(LocalizationPackage))
                return false;

            Init(obj as LocalizationPackage);
            return true;
        }

        private void OnGUI()
        {
            if (_package == null)
                return;

            Undo.RecordObject(_package, "localization package");
            Toolbar();
            MainArea();
            EditorUtility.SetDirty(_package);
        }

        private void Toolbar()
        {
            using (new HorizontalBlock(EditorStyles.toolbar))
            {
                if (_editName)
                    _package.Name = EditorGUILayout.TextField(_package.Name, EditorStyles.toolbarTextField, GUILayout.Width(150));
                else
                    GUILayout.Label(_package.Name, GUILayout.Width(150));

                if (GUILayout.Button("Edit", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
                    _editName = !_editName;

                var selectedPackage = Selection.activeObject != null &&
                                      Selection.activeObject.GetType() == typeof(LocalizationPackage) &&
                                      Selection.activeObject != _package;
                using (new ColoredBlock(selectedPackage ? Color.white : Color.grey))
                {
                    if (GUILayout.Button(new GUIContent("Copy langs", "Copies languages from currently selected package in project view"), EditorStyles.toolbarButton))
                    {
                        if (selectedPackage)
                        {
                            foreach (var lang in (Selection.activeObject as LocalizationPackage).GetLanguages())
                                _package.AddLanguage(lang);
                        }
                    }
                }

                GUILayout.FlexibleSpace();

                if (_currentData == typeof(string))
                {
                    if (GUILayout.Button("Import csv", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
                    {
                        var path = EditorUtility.OpenFilePanel("Import translation", "", "csv").Replace("/", "\\");
                        _package.SetData(typeof(string), CSVHelper.Import(path));
                        EditorUtility.SetDirty(_package);
                    }
                    if (GUILayout.Button("Export csv", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
                    {
                        var path = EditorUtility.SaveFilePanel("Export translation", "", "", "csv").Replace("/", "\\");

                    }
                }

            }

            using (new HorizontalBlock())
            {
                foreach (var type in Core.Constants.SupportedTypes)
                {
                    var b = _package.ContainsData(type);
                    var btnStyle = b ? EditorStyles.miniButtonLeft : EditorStyles.miniButton;

                    using (new ColoredBlock(b ? Color.white : Color.grey))
                    {
                        if (GUILayout.Button(type.Name, btnStyle))
                        {
                            if (b)
                                _currentData = type;
                            else
                                _package.AddLocalizationData(type);
                        }
                        if (b)
                            if (GUILayout.Button("X", EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false)))
                                if (EditorUtility.DisplayDialog("Delete localization data", "Atatata", "Yes", "Cancel"))
                                {
                                    var type1 = type;
                                    EditorApplication.delayCall += () =>
                                    {
                                        _package.RemoveLocalizationData(type1);
                                        Repaint();
                                    };
                                }
                    }
                }
            }
        }

        private void MainArea()
        {
            if (CurrentData == null)
                return;

            using (new VerticalBlock(EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
            {
                using (new HorizontalBlock())
                {
                    GUILayout.Space(150f);
                    foreach (var language in CurrentData.Languages)
                    {
                        using (new HorizontalBlock(EditorStyles.helpBox, GUILayout.MaxWidth(150)))
                        {
                            GUILayout.Label(language.ToString());
                            if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                                EditorApplication.delayCall += () =>
                                {
                                    _package.RemoveLanguage(language);
                                    Repaint();
                                };
                        }
                    }
                }
                foreach (var key in CurrentData.Keys)
                {
                    using (new HorizontalBlock())
                    {
                        using (new HorizontalBlock(EditorStyles.helpBox, GUILayout.MaxWidth(150)))
                        {
                            GUILayout.Label(key);
                            if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                                EditorApplication.delayCall += () =>
                                {
                                    CurrentData.RemoveKey(key);
                                    Repaint();
                                };
                        }
                        foreach (var language in CurrentData.Languages)
                        {

                            if (_currentData == typeof(string))
                                CurrentData.SetTranslation(language, key,
                                    EditorGUILayout.TextArea((string)CurrentData.GetTranslation(language, key),
                                        GUILayout.MaxWidth(150)));
                            else
                                CurrentData.SetTranslation(language, key,
                                    EditorGUILayout.ObjectField(
                                        (UnityEngine.Object)CurrentData.GetTranslation(language, key),
                                        _currentData, false, GUILayout.Width(150)));
                        }
                    }
                }

                GUILayout.FlexibleSpace();
                using (new HorizontalBlock())
                {
                    AddKeyArea();
                    AddLanguageArea();
                }
            }
        }

        private void AddKeyArea()
        {
            using (new HorizontalBlock())
            {
                _newKeyName = EditorGUILayout.TextField(_newKeyName);
                if (GUILayout.Button("Add", EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                {
                    CurrentData.AddKey(_newKeyName);
                    _newKeyName = "";
                    GUI.FocusControl(null);
                }
            }
        }

        private void AddLanguageArea()
        {
            using (new HorizontalBlock())
            {
                _newLang = (SystemLanguage)EditorGUILayout.EnumPopup(_newLang);
                if (GUILayout.Button("Add", EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                {
                    _package.AddLanguage(_newLang);
                    _newLang = 0;
                }
            }
        }
    }
    
}