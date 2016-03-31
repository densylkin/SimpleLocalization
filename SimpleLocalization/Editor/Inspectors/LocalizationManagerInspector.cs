using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;
using SimpleLocalization.Core;
using UnityHelpers;
using UnityHelpers.GUI;

namespace SimpleLocalization.Editor
{
    [CustomEditor(typeof(LocalizationManager))]
    public class LocalizationManagerInspector : UnityEditor.Editor
    {
        private LocalizationManager Manager { get { return (LocalizationManager)target; } }
        private Vector2 _scroll;

        private GUIStyle _dropBoxStyle;
        private GUIStyle _pkgListBackground;

        private void OnEnable()
        {
            _dropBoxStyle = new GUIStyle
            {
                normal = {background = ColorHelpers.MakeTex(new Color(0.3f, 0.3f, 0.3f, 0.5f))},
                alignment = TextAnchor.MiddleCenter,
                fontSize = 14,
                margin = new RectOffset(5, 5, 5, 5)
            };
            _dropBoxStyle.normal.textColor = Color.white;

            _pkgListBackground = ColorHelpers.MakeBackgroudnStyle(new Color(0.1f, 0.1f, 0.1f, 0.8f));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Undo.RecordObject(Manager, "Localization manager");

            if (Manager.AllLangauges != null)
            {
                Manager.DefaultLanguageIndex = EditorGUILayout.Popup("DefaultLanguage", Manager.DefaultLanguageIndex,
                    Manager.LanguagesNames);

                LocalizationManager.CurrentLanguageIndex = EditorGUILayout.Popup("Current", LocalizationManager.CurrentLanguageIndex,
                    Manager.LanguagesNames);
            }

            EditorGUIExtentions.DropZone<LocalizationPackage>(PrefabType.None, list =>
            {
                foreach (var package in list)
                {
                    Manager.AddPackage(package);
                }
            }, _dropBoxStyle, GUILayout.Height(25));
            PackagesList();

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(Manager);
        }

        private void PackagesList()
        {
            using (new ScrollviewBlock(ref _scroll, GUILayout.ExpandHeight(false)))
            {
                using (new VerticalBlock(_pkgListBackground))
                {
                    foreach (var kvp in Manager.Packages)
                    {
                        var pkg = kvp.Value;
                        using (new HorizontalBlock())
                        {
                            GUILayout.Label(kvp.Key);
                            GUILayout.FlexibleSpace();
                            if (GUILayout.Button("Select", EditorStyles.miniButtonLeft))
                            {
                                Selection.activeObject = pkg;
                            }
                            if (GUILayout.Button("x", EditorStyles.miniButtonRight))
                            {
                                EditorApplication.delayCall += () => Manager.RemovePackage(kvp.Key);
                                Repaint();
                            }
                        }
                    }
                }
            }
        }
    }
    
}