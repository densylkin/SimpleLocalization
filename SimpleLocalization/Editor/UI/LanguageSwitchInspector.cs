using UnityEngine;
using System.Collections;
using SimpleLocalization.Core;
using UnityEngine.UI;
using UnityHelpers.GUI;

namespace SimpleLocalization.UI
{
    using UnityEditor;

    [CustomEditor(typeof(LanguageSwitch))]
    public class LanguageSwitchInspector : Editor
    {
        private LanguageSwitch _switch { get { return (LanguageSwitch) target; } }
        private LocalizationManager _manager { get { return LocalizationManager.Instance; } }

        public override void OnInspectorGUI()
        {
            if (!LocalizationManager.Exists)
            {
                GUILayout.Label("No localization manager found");
                return;
            }

            _switch.Image = (Image) EditorGUILayout.ObjectField("Image", _switch.Image, typeof (Image), true);
            for (var i = 0; i < _switch.Sprites.Count; i++)
            {
                using (new HorizontalBlock(GUI.skin.box))
                {
                    GUILayout.Label(_manager.AllLangauges[i].ToString());
                    _switch.Sprites[i] = (Sprite) EditorGUILayout.ObjectField(_switch.Sprites[i], typeof (Sprite), false);
                    if (GUILayout.Button("x", EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                    {
                        _switch.Sprites.RemoveAt(i);
                    }
                }
            }
            GUI.enabled = _switch.Sprites.Count < _manager.LanguagesCount;
            if (GUILayout.Button("Add"))
            {
                _switch.Sprites.Add(null);
            }
        }
    }
}
