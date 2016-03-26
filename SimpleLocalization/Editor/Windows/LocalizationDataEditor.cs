using UnityEngine;
using System.Collections;
using SimpleLocalization.Core;
using SimpleLocalization.Helpers;
using UnityEditor;
using UnityHelpers;
using UnityHelpers.GUI;

namespace SimpleLocalization.Editor
{
    public class LocalizationDataEditor : EditorWindow
    {
        private ILocalizationData _data;

        private Vector2 scroll;

        private string _newKeyName = "";
        private string _newKey = "";

        private GUIStyle _backgroudnStyle;

        private GUIStyle _langStyle1;
        private GUIStyle _langStyle2;

        private GUIStyle _keyStyle1;
        private GUIStyle _keyStyle2;

        public static void Init(ILocalizationData data)
        {
            var window = GetWindow<LocalizationDataEditor>();
            window._data = data;
        }

        private void OnEnable()
        {
            SetupStyles();
        }

        private void OnGUI()
        {
            if (_data == null)
            {
                GUILayout.Label("No data loaded");
                return;
            }
            AddKeyArea();
            GUILayout.Space(10f);

            using (new ScrollviewBlock(ref scroll, GUILayout.ExpandHeight(true)))
            {
                using (new VerticalBlock(_backgroudnStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    using (new HorizontalBlock())
                    {
                        //Languages row
                        GUILayout.Label(" ", _langStyle1, GUILayout.Width(122));
                        for (var i = 0; i < _data.Languages.Length; i++)
                        {
                            var language = _data.Languages[i];
                            GUILayout.Label(language.ToString(), i % 2 == 1 ? _langStyle1 : _langStyle2, GUILayout.Width(250));
                        }

                    }

                    for (var i = 0; i < _data.Keys.Length; i++)
                    {
                        var key = _data.Keys[i];

                        using (new HorizontalBlock(i % 2 == 1 ? _keyStyle1 : _keyStyle2))
                        {
                            if (GUILayout.Button("x", EditorStyles.miniButton, GUILayout.Width(18)))
                                EditorApplication.delayCall += () =>
                                {
                                    _data.RemoveKey(key);
                                    Repaint();
                                };
                            //Key and value row
                            GUILayout.Label(key, GUILayout.Width(100), GUILayout.ExpandHeight(true));

                            for (var j = 0; j < _data.Languages.Length; j++)
                            {
                                var language = _data.Languages[j];

                                if (_data.DataType == typeof(string))// check if value is string or UnityObject
                                    _data.SetTranslation(language, key,
                                        EditorGUILayout.TextArea((string)_data.GetTranslation(language, key),
                                            GUILayout.Width(246)));
                                else
                                    _data.SetTranslation(language, key,
                                        EditorGUILayout.ObjectField(
                                            (UnityEngine.Object)_data.GetTranslation(language, key),
                                            _data.DataType,
                                            false, GUILayout.Width(246)));
                            }
                        }
                    }
                }
                GUILayout.FlexibleSpace();
            }
        }

        private void AddKeyArea()
        {
            using (new ColoredBlock(new Color32(188, 188, 255, 255)))
            {
                using (new HorizontalBlock(EditorStyles.toolbar))
                {
                    GUI.color = Color.white;
                    ImportCsvButton();
                    GUILayout.Label("Add key");
                    GUI.color = new Color32(188, 188, 255, 255);
                    _newKeyName = EditorGUILayout.TextField(_newKeyName, EditorStyles.toolbarTextField,
                        GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("Add", EditorStyles.toolbarButton, GUILayout.ExpandWidth(true)))
                    {
                        _data.AddKey(_newKeyName);
                        _newKeyName = "";
                        GUI.FocusControl(null);
                    }
                }
            }
        }

        private void SetupStyles()
        {
            _backgroudnStyle = ColorHelpers.MakeBackgroudnStyle(ColorHelpers.HexToColor("242424FF"));

            _langStyle1 = ColorHelpers.MakeBackgroudnStyle(ColorHelpers.HexToColor("323A45FF"));
            _langStyle2 = ColorHelpers.MakeBackgroudnStyle(ColorHelpers.HexToColor("0F1A2BFF"));

            _langStyle1.padding = _langStyle2.padding = new RectOffset(5, 5, 10, 10);
            _langStyle1.alignment = _langStyle2.alignment = TextAnchor.MiddleCenter;

            _langStyle1.normal.textColor = _langStyle2.normal.textColor = Color.white;

            _keyStyle1 = ColorHelpers.MakeBackgroudnStyle(ColorHelpers.HexToColor("1B2E24FF"));
            _keyStyle2 = ColorHelpers.MakeBackgroudnStyle(ColorHelpers.HexToColor("2E5942FF"));

            _keyStyle1.padding = _keyStyle2.padding = new RectOffset(5, 5, 3, 3);
            _keyStyle1.alignment = _keyStyle2.alignment = TextAnchor.MiddleCenter;

            _keyStyle1.normal.textColor = _keyStyle2.normal.textColor = Color.white;
        }

        private void ImportCsvButton()
        {
            if(_data.DataType != typeof(string))
                return;

            if (GUILayout.Button("ImportCsv", EditorStyles.toolbarButton))
            {
                var path = EditorUtility.OpenFilePanel("Import translation", "", "csv").Replace("/", "\\");
                var tempData = CSVHelper.Import(path);
                _data.CopyFrom(tempData);
            }

            if (GUILayout.Button("ExportCsv", EditorStyles.toolbarButton))
            {
                var path = EditorUtility.SaveFilePanel("Import translation", "", _data.Name,"csv").Replace("/", "\\");
                _data.Export(path);
            }
        }
    }
}