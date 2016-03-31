using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using SimpleLocalization.Core;
using SimpleLocalization.Helpers;
using UnityHelpers;
using UnityHelpers.GUI;

namespace SimpleLocalization.Editor
{
    [CustomEditor(typeof(LocalizationPackage))]
    public class LocalizationPackageInspector : UnityEditor.Editor
    {
        private SystemLanguage[] _languages;
        private SystemLanguage _newLang;

        private TabsBlock _tabsMain;
        private bool _editPkgName;

        private GUIStyle _backgroundStyle;

        private LocalizationPackage pkg { get { return (LocalizationPackage)target; } }

        [MenuItem("Assets/Create/Localization package")]
        public static void CreatePackage()
        {
            var asset = ScriptableObject.CreateInstance<LocalizationPackage>();
            asset.Name = "New package";
            ProjectWindowUtil.CreateAsset(asset, "NewLocalizationPackage.asset");
        }

        private void OnEnable()
        {
            RefreshLanguages();

            _tabsMain = new TabsBlock(new Dictionary<string, Action>()
            {
                {"Languages", LanguagesTab},
                {"Data", DatasTab}
            });

            _backgroundStyle = ColorHelpers.MakeBackgroudnStyle(new Color(0.1f, 0.1f, 0.1f, 0.5f));

            _tabsMain.SetCurrentMethod(0);
        }

        public override void OnInspectorGUI()
        {
            Undo.RecordObject(pkg, "localization Pkg");
            Toolbar();
            EditorUtility.SetDirty(pkg);
        }

        private void Toolbar()
        {
            using (new HorizontalBlock(EditorStyles.toolbar))
            {
                if (_editPkgName)
                    pkg.Name = EditorGUILayout.TextField(pkg.Name, EditorStyles.toolbarTextField,
                        GUILayout.ExpandWidth(true));
                else
                    GUILayout.Label(pkg.Name);

                GUILayout.FlexibleSpace();

                using(new ColoredBlock(_editPkgName ? Color.grey : Color.white))
                    if (GUILayout.Button("Edit", EditorStyles.toolbarButton))
                        _editPkgName = !_editPkgName;
            }
            EditorGUILayout.Space();
            _tabsMain.DrawBody(_backgroundStyle);
        }

        #region LanguagesTab

        private void LanguagesTab()
        {
            LanguagesList();
            AddLanguage();
        }

        private void LanguagesList()
        {
            for (var i = 0; i < _languages.Length; i++)
            {
                using (new HorizontalBlock(EditorStyles.toolbar))
                {
                    var language = _languages[i];

                    GUILayout.Label(i.ToString(), EditorStyles.toolbarButton);
                    GUILayout.Label(language.ToString());
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("x", EditorStyles.toolbarButton))
                        EditorApplication.delayCall += () =>
                        {
                            pkg.RemoveLanguage(language);
                            RefreshLanguages();
                        };
                }
                GUILayout.Space(3f);
            }
        }

        private void AddLanguage()
        {
            EditorGUILayout.Space();
            using (new HorizontalBlock(EditorStyles.toolbar))
            {
                _newLang = (SystemLanguage)EditorGUILayout.EnumPopup("New language", _newLang, EditorStyles.toolbarPopup, GUILayout.ExpandWidth(true));
                if (GUILayout.Button("Add", EditorStyles.toolbarButton))
                    EditorApplication.delayCall += () =>
                    {
                        pkg.AddLanguage(_newLang);
                        RefreshLanguages();
                    };
            }
        }

        #endregion

        #region Data Editor Tab

        private void DatasTab()
        {
            DataList();
        }

        private void DataList()
        {

            for (var i = 0; i < Core.Constants.SupportedTypes.Length; i++)
            {
                var type = Core.Constants.SupportedTypes[i];

                using (new HorizontalBlock(EditorStyles.toolbar))
                {
                    GUILayout.Label(i.ToString(), EditorStyles.toolbarButton);
                    GUILayout.Label(type.Name, EditorStyles.toolbarButton);
                    GUILayout.FlexibleSpace();

                    if (pkg.ContainsData(type))
                    {
                        if (GUILayout.Button("Edit", EditorStyles.toolbarButton)) 
                            LocalizationDataEditor.Init(pkg.GetData(type));
                        if (GUILayout.Button("Remove", EditorStyles.toolbarButton))
                            EditorApplication.delayCall += () => pkg.RemoveLocalizationData(type);
                    }
                    else
                    {
                        if (GUILayout.Button("Add", EditorStyles.toolbarButton))
                            EditorApplication.delayCall += () => pkg.AddLocalizationData(type);
                    }
                }
            }
        }

        #endregion

        private void RefreshLanguages()
        {
            Repaint();
            _languages = pkg.GetLanguages();
        }
    }
    
}